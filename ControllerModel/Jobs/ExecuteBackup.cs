    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Diagnostics;
using ControllerModel.Logs2;
using ControllerModel.JsonHelper;
using System.Reflection;



namespace ControllerModel.Jobs
{
    public class ExecuteBackup
    {

        public delegate void StatusHandler(int status);
        public event StatusHandler Status;
        public delegate void ProgresseBarHandler(double progressBar);
        public event ProgresseBarHandler ProgressBar;

        public static Dictionary<int, ProgresseBarHandler> ProgressDelegatesByJobId = [];
        public static Dictionary<int, StatusHandler> StatusDelegatesByJobId = [];

        // Properties
        private readonly Daily _logDaily = new();
        private readonly State _state = new();
        private string[] _listExtensionFileCrypt;

        public JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();
        public SaveConfig _saveConfig;
        //public SaveConfig saveConfigObjJob;
        public string binPathGlobal;

        public ExecuteBackup()
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPath, "config.json"));
            this._saveConfig = SaveConfig;
            _listExtensionFileCrypt = this._saveConfig.ExtensionFileCrypt;
        }

        private object _lockPriorityFile;
        private object _priorityFileProperty = null;
        public int PriorityFile
        {
            get
            {
                lock (_lockPriorityFile)
                {
                    if (_priorityFileProperty == null)
                    {
                        _priorityFileProperty = 0;
                    }
                    return (int)_priorityFileProperty;
                }
            }
            set
            {
                lock (_lockPriorityFile)
                {
                    // add a mutex => 0 to * = mutex | * to 0 = release
                    _priorityFileProperty = (object)value;
                }
            }
        }

        /// <summary>
        /// Runs the backup for all jobs in the list.
        /// </summary>
        /// <param name="JobList">List of backup jobs to run.</param>
        public void ExecuteJobAll(List<JobObj> JobList)
        {
            List<Thread> threads = new List<Thread>();

            foreach (var job in JobList)
            {
                var tokenSource = new CancellationTokenSource();
                var pauseEvent = new ManualResetEventSlim(true);
                Thread thread = new Thread(() => ExecuteJob(job, tokenSource.Token, pauseEvent));
                thread.Start();
                threads.Add(thread);
            }
        }

        /// <summary>
        /// Runs a backup for a given job.
        /// Checks the existence of source and target paths, measures execution time,
        /// and logs job information.
        /// </summary>
        /// <param name="job">Le job de sauvegarde à exécuter.</param>
        /// <returns>0 si la sauvegarde a réussi, 1 sinon (ex : chemin non valide).</returns>
        public int ExecuteJob(JobObj job, CancellationToken token, ManualResetEventSlim pauseEvent)
        {
            /*if (_saveConfig.BlockingApp != null && _saveConfig.BlockingApp != "")
            {
                Process[] processes = Process.GetProcessesByName(_saveConfig.BlockingApp);
                if (processes.Length > 0)
                {
                    Console.WriteLine($"Fermer le process {_saveConfig.BlockingApp}");
                    while (processes.Length > 0)
                    {
                        processes = Process.GetProcessesByName(_saveConfig.BlockingApp);
                    }
                }
            }*/
            string sourcePath = job.SourcePath;
            /*foreach (var file in Directory.GetFiles(sourcePath))
            {
                if (file.Contains(_saveConfig.BlockingApp) && file.EndsWith("exe"))
                {
                    Console.WriteLine($"Application {_saveConfig.BlockingApp} detecté demarage annulé");
                    return 1;
                }
            }*/

            Console.WriteLine("Execute job");
            // Simulate file transfer
            string targetPath = job.TargetPath;
            string name = job.Name;
            int id = job.Id;

            if (!ProgressDelegatesByJobId.ContainsKey(id))
            {
                ProgressDelegatesByJobId[id] = delegate { }; // délégué vide par défaut
            }

            // Timer
            Stopwatch stopwatch = new();
            stopwatch.Start();
            if (!Directory.Exists(sourcePath)) { return 1; }
            if (!Directory.Exists(targetPath)) { return 1; }

            int totalFiles = Directory.GetFiles(sourcePath).Length;
            int totalFilesLeft = totalFiles;
            long totalFileSize = new DirectoryInfo(job.SourcePath).GetFiles().Sum(f => f.Length);

            Dictionary<string, long> fileEncryptionTimes = new();

            // transfer files
            switch (job.Type)
            {
                case JobType.Full:
                    FullBackup(id, name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft, fileEncryptionTimes, pauseEvent, token);
                    break;
                case JobType.Differential:
                    DifferentialBackup(id, name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft, fileEncryptionTimes, pauseEvent, token);
                    break;
            }
            stopwatch.Stop();
            _state.SendParamToLog(
                    name,
                    sourcePath,
                    targetPath,
                    StateEnumeration.Inactive,
                    totalFiles,
                    totalFileSize,
                    totalFilesLeft,
                    100
                );

            _logDaily.sendParamToLog(
                name,
                sourcePath,
                targetPath,
                totalFileSize,
                stopwatch.ElapsedMilliseconds,
                DateTime.Now,
                fileEncryptionTimes
            );
            ChangeButtonStatus(id, 0);
            ChangeProgressionBar(id, 100);
            

            if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status))
            {
                Status?.Invoke(0);
            }


            if (ExecuteBackup.ProgressDelegatesByJobId.TryGetValue(id, out var callback))
            {
                callback?.Invoke(100);
            }

            Console.WriteLine($"[END] Job {name} terminé en {stopwatch.ElapsedMilliseconds} ms");
            return 0;
        }
        // Backup methods

        /// <summary>
        /// Performs a full backup: deletes all files in the target
        /// and copies all files from the source to the target.
        /// Updates the progress in the report.
        /// </summary>
        /// <param name="name">Nom du job.</param>
        /// <param name="sourcePath">Chemin source des fichiers à sauvegarder.</param>
        /// <param name="targetPath">Chemin cible pour la sauvegarde.</param>
        /// <param name="totalFiles">Nombre total de fichiers à sauvegarder.</param>
        /// <param name="totalFileSize">Taille totale des fichiers à sauvegarder en octets.</param>
        /// <param name="totalFilesLeft">Nombre de fichiers restant à traiter.</param>
        public void FullBackup(int id, string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes, ManualResetEventSlim pauseEvent, CancellationToken token)
        {

            token.ThrowIfCancellationRequested();

            foreach (string file in Directory.GetFiles(targetPath))
            {
               
                File.Delete(file);
            }
            foreach (var file in Directory.GetFiles(sourcePath))
            {
                if (!pauseEvent.IsSet)
                {

                    Debug.WriteLine("Le job est en pause. Libération des ressources...");
                    ChangeButtonStatus(id, 2);

                    if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status2))
                    {
                        Status2?.Invoke(2);
                    }
                    pauseEvent.Wait();
                }
                else if (token.IsCancellationRequested)
                {
                    ChangeButtonStatus(id, 0);

                    if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status3))
                    {
                        Status3?.Invoke(0);
                    }
                    token.ThrowIfCancellationRequested();
                }

                string fileName = Path.GetFileName(file);
                string targetFile = Path.Combine(targetPath, fileName);
                long timeToEncrypt = 0;

                if (_listExtensionFileCrypt.Contains(Path.GetExtension(file)))
                {
                    Stopwatch encryptTimer = Stopwatch.StartNew();

                    string basePath = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "win-x64/CryptoSoft.exe");

                    ProcessStartInfo processStartInfo = new ProcessStartInfo
                    {
                        FileName = basePath,
                        Arguments = $"\"{file}\" \"{targetFile}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    Process processCryptoSoft = Process.Start(processStartInfo);

                    processCryptoSoft.WaitForExit();
                    encryptTimer.Stop();
                    timeToEncrypt = encryptTimer.ElapsedMilliseconds;
                    if (token.IsCancellationRequested)
                    {
                        ChangeButtonStatus(id, 0);
                        if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status3))
                        {
                            Status3?.Invoke(0);
                        }
                        token.ThrowIfCancellationRequested();
                    }
                    //Console.WriteLine("Process terminé");
                }
                else
                {
                    File.Copy(file, targetFile, true);
                }
                fileEncryptionTimes[fileName] = timeToEncrypt;
                totalFilesLeft--;

                int progression = (int)((double)(totalFiles - totalFilesLeft) / totalFiles * 100);

                _state.SendParamToLog(
                    name,
                    sourcePath,
                    targetPath,
                    StateEnumeration.In_progress,
                    totalFiles,
                    totalFileSize,
                    totalFilesLeft,
                    progression
                );
                if (token.IsCancellationRequested)
                {
                    ChangeButtonStatus(id, 0);
                    if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status4))
                    {
                        Status4?.Invoke(0);
                    }
                    token.ThrowIfCancellationRequested();
                }

                ChangeButtonStatus(id, 1);
                ChangeProgressionBar(id, progression);

                if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status))
                {
                    Status?.Invoke(1);
                }
                //ProgressBar?.Invoke(progression);
                if (ExecuteBackup.ProgressDelegatesByJobId.TryGetValue(id, out var callback))
                {
                    callback?.Invoke(progression);
                }
            }

        }


        public void ChangeButtonStatus(int id, int status) 
        {
            if (JobManager.threadsByJob.TryGetValue(id, out var oldValue))
            {
                JobManager.threadsByJob[id] = (oldValue.Thread, oldValue.TokenSource, oldValue.PauseEvent, status, oldValue.progressBarPercent);
            }
        }

        public void ChangeProgressionBar(int id, double progress)
        {
            if (JobManager.threadsByJob.TryGetValue(id, out var oldValue))
            {
                JobManager.threadsByJob[id] = (oldValue.Thread, oldValue.TokenSource, oldValue.PauseEvent, oldValue.ButtonStatus, progress);
            }
        }


        /// <summary>
        /// Performs a differential backup: copies only modified or new files.
        /// Updates the progress in the report.
        /// </summary>
        /// <param name="name">Nom du job.</param>
        /// <param name="sourcePath">Chemin source des fichiers à sauvegarder.</param>
        /// <param name="targetPath">Chemin cible pour la sauvegarde.</param>
        /// <param name="totalFiles">Nombre total de fichiers à analyser.</param>
        /// <param name="totalFileSize">Taille totale des fichiers à analyser en octets.</param>
        /// <param name="totalFilesLeft">Nombre de fichiers restant à traiter.</param>
        public void DifferentialBackup(int id, string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes, ManualResetEventSlim pauseEvent, CancellationToken token)
        {
            
            
            foreach (string sourceFilePath in Directory.GetFiles(sourcePath))
            {
                if (!pauseEvent.IsSet)
                {

                    Debug.WriteLine("Le job est en pause. Libération des ressources...");
                    ChangeButtonStatus(id, 2);

                    if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status5))
                    {
                        Status5?.Invoke(2);
                    }
                    pauseEvent.Wait();
                }else if (token.IsCancellationRequested)
                {
                    ChangeButtonStatus(id, 0);
                    if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status6))
                    {
                        Status6?.Invoke(0);
                    }
                    token.ThrowIfCancellationRequested();
                }
                
                Thread.Sleep(500);
                
                string fileName = Path.GetFileName(sourceFilePath);
                string destFilePath = Path.Combine(targetPath, fileName);

                long timeToEncrypt = 0;

                if (!File.Exists(destFilePath) || File.GetLastWriteTime(sourceFilePath) > File.GetLastWriteTime(destFilePath))
                {
                    if (_listExtensionFileCrypt.Contains(Path.GetExtension(sourceFilePath)))
                    {
                        Stopwatch encryptTimer = Stopwatch.StartNew();
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = "C:\\Users\\Mathis\\OneDrive\\Bureau\\cesi temporaire\\A3\\Bloc Génie logiciel\\Prosit-5\\Prosit5\\Prosit5\\bin\\Release\\net8.0\\Prosit5.exe",
                            Arguments = $"\"{sourceFilePath}\" \"{destFilePath}\"",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process processCryptoSoft = Process.Start(processStartInfo);
                        processCryptoSoft.WaitForExit();

                        encryptTimer.Stop();
                        timeToEncrypt = encryptTimer.ElapsedMilliseconds;
                        if (token.IsCancellationRequested)
                        {
                            ChangeButtonStatus(id, 0);
                            if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status7))
                            {
                                Status7?.Invoke(0);
                            }
                            token.ThrowIfCancellationRequested();
                        }
                        Console.WriteLine("Process terminé");
                    }
                    else
                    {
                        File.Copy(sourceFilePath, destFilePath, true);
                    }
                    fileEncryptionTimes[fileName] = timeToEncrypt;
                }

                totalFilesLeft--;
                int progression = (int)((double)(totalFiles - totalFilesLeft) / totalFiles * 100);

                _state.SendParamToLog(
                    name,
                    sourcePath,
                    targetPath,
                    StateEnumeration.In_progress,
                    totalFiles,
                    totalFileSize,
                    totalFilesLeft,
                    progression
                );
                if (token.IsCancellationRequested)
                {
                    ChangeButtonStatus(id, 0);
                    if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status8))
                    {
                        Status8?.Invoke(0);
                    }
                    token.ThrowIfCancellationRequested();
                }
                ChangeButtonStatus(id, 1);
                ChangeProgressionBar(id, progression);
                if (ExecuteBackup.StatusDelegatesByJobId.TryGetValue(id, out var Status))
                {
                    Status?.Invoke(1);
                }


                //ProgressBar?.Invoke(progression);
                if (ExecuteBackup.ProgressDelegatesByJobId.TryGetValue(id, out var callback))
                {
                    callback?.Invoke(progression);
                }
            }
        }
    }
}