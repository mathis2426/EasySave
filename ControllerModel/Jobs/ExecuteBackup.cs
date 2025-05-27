    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string[] _extensionPriorityFile;

        public JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();

        public SaveConfig _saveConfig;
        public string binPathGlobal;

        // Lock object to synchronize access to the priority file count
        private readonly object _lockPriorityFile = new();

        // Counter for how many priority files are currently being processed
        private int _priorityFileCount = 0;

        // ManualResetEvent used as a signal to control when non-priority files can be processed.
        // Initially set to false, meaning non-priority files must wait until all priority files are done.
        private ManualResetEvent _canProcessNonPriorityFiles = new(false);

        private readonly object _lockHeavyFile = new();
        private int _heavyFileInProgress = 0;

        // Property to safely get/set the count of priority files with locking
        public int PriorityFile
        {
            get
            {
                lock (_lockPriorityFile)
                {
                    return _priorityFileCount;
                }
            }
            set
            {
                lock (_lockPriorityFile)
                {
                    _priorityFileCount = value;
                    if (_priorityFileCount == 0)
                        _canProcessNonPriorityFiles.Set();   // Signal that non-priority files can proceed
                    else
                        _canProcessNonPriorityFiles.Reset(); // Non-priority files must wait
                }
                Console.WriteLine("Priority files count updated: " + _priorityFileCount);
            }
        }

        public ExecuteBackup()
        {
            // Load configuration from JSON file on startup
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPath, "config.json"));
            this._saveConfig = SaveConfig;
            _listExtensionFileCrypt = this._saveConfig.ExtensionFileCrypt;
            _extensionPriorityFile = this._saveConfig.ExtensionPriorityFile;
        }

        // Starts backup jobs for all provided JobObj instances in separate threads
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

            Stopwatch stopwatch = new();
            stopwatch.Start();

            // Return error if either source or target directory doesn't exist
            if (!Directory.Exists(sourcePath)) return 1;
            if (!Directory.Exists(targetPath)) return 1;

            int totalFiles = Directory.GetFiles(sourcePath).Length;
            int totalFilesLeft = totalFiles;
            long totalFileSize = new DirectoryInfo(job.SourcePath).GetFiles().Sum(f => f.Length);
            Dictionary<string, long> fileEncryptionTimes = new();

            // Count how many files in the source folder are priority files (based on extension)
            int nbPriorityFiles = Directory.GetFiles(sourcePath).Count(f => _extensionPriorityFile.Contains(Path.GetExtension(f)));

            // Increase the priority file count atomically and manage signaling
            lock (_lockPriorityFile)
            {
                PriorityFile += nbPriorityFiles;
            }

            // Execute the appropriate backup type
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

        private bool IsHeavyFile(long fileSize)
        {
            return fileSize > _saveConfig.LargeFileThreshold * 1024;
        }

        private void WaitIfHeavyFile(long fileSize)
        {
            if (!IsHeavyFile(fileSize)) return;

            Console.WriteLine($"Waiting to process large file > {_saveConfig.LargeFileThreshold} kB");

            while (true)
            {
                lock (_lockHeavyFile)
                {
                    if (_heavyFileInProgress < 1)
                    {
                        _heavyFileInProgress++;
                        Console.WriteLine("Large file allowed to proceed.");
                        break;
                    }
                }
                Thread.Sleep(100);
            }
        }

        private void DoneProcessingHeavyFile(long fileSize)
        {
            if (!IsHeavyFile(fileSize)) return;

            lock (_lockHeavyFile)
            {
                _heavyFileInProgress--;
                Console.WriteLine("Large file finished. Slot released.");
            }
        }

        // Performs a full backup of all files from source to target folder
        public void FullBackup(int id, string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes, ManualResetEventSlim pauseEvent, CancellationToken token)
        {

            token.ThrowIfCancellationRequested();

            foreach (string file in Directory.GetFiles(targetPath))
            {
               
                File.Delete(file);
            }

            // Get all files and separate priority and non-priority files based on extensions
            var allFiles = Directory.GetFiles(sourcePath);
            var priorityFiles = allFiles.Where(f => _extensionPriorityFile.Contains(Path.GetExtension(f))).ToList();
            var nonPriorityFiles = allFiles.Where(f => !_extensionPriorityFile.Contains(Path.GetExtension(f))).ToList();

            // Process priority files first, then non-priority files
            foreach (var file in priorityFiles.Concat(nonPriorityFiles))
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
                string extension = Path.GetExtension(file);
                long fileSize = new FileInfo(file).Length;
                bool isPriority = _extensionPriorityFile.Contains(extension);
                long timeToEncrypt = 0;

                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} → processing {fileName}");

                if (!isPriority)
                {
                    // Non-priority files wait until all priority files are processed
                    Console.WriteLine("Waiting for non-priority files...");
                    _canProcessNonPriorityFiles.WaitOne();
                }

                WaitIfHeavyFile(fileSize);

                try
                {
                    if (_listExtensionFileCrypt.Contains(extension))
                    {
                        // Encrypt files with extensions requiring encryption
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
                    }
                    else
                    {
                        // For other files, just copy normally
                        File.Copy(file, targetFile, true);
                    }

                    fileEncryptionTimes[fileName] = timeToEncrypt;
                }
                finally
                {
                    DoneProcessingHeavyFile(fileSize);
                    // Decrement the priority file count once a priority file is processed
                    if (isPriority)
                    {
                        lock (_lockPriorityFile)
                        {
                            _priorityFileCount--;
                            if (_priorityFileCount == 0)
                            {
                                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} → ✅ All priority files processed!");
                                // Signal that non-priority files can now proceed
                                _canProcessNonPriorityFiles.Set();
                            }
                        }
                    }
                }
                //fileEncryptionTimes[fileName] = timeToEncrypt;
                totalFilesLeft--;

                // Calculate progress percentage
                int progression = (int)((double)(totalFiles - totalFilesLeft) / totalFiles * 100);

                // Send current progress to the state logger
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
                string extension = Path.GetExtension(sourceFilePath);
                long fileSize = new FileInfo(sourceFilePath).Length;
                bool isPriority = _extensionPriorityFile.Contains(extension);
                long timeToEncrypt = 0;

                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} → processing {fileName}");

                if (!isPriority)
                {
                    // Non-priority files wait until all priority files are done
                    Console.WriteLine("Waiting for non-priority files...");
                    _canProcessNonPriorityFiles.WaitOne();
                }

                WaitIfHeavyFile(fileSize);

                try
                {
                    // Copy file only if it doesn't exist or is newer than the one in target
                    if (!File.Exists(destFilePath) || File.GetLastWriteTime(sourceFilePath) > File.GetLastWriteTime(destFilePath))
                    {
                        if (_listExtensionFileCrypt.Contains(Path.GetExtension(sourceFilePath)))
                        {
                            Stopwatch encryptTimer = Stopwatch.StartNew();
                            string basePath = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "win-x64/CryptoSoft.exe");
                            ProcessStartInfo processStartInfo = new ProcessStartInfo
                            {
                                FileName = basePath,
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
                }
                finally
                {
                    DoneProcessingHeavyFile(fileSize);
                    // Decrement priority file count and signal if done
                    if (isPriority)
                    {
                        lock (_lockPriorityFile)
                        {
                            _priorityFileCount--;
                            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} → priority file processed, remaining {_priorityFileCount}");

                            if (_priorityFileCount == 0)
                            {
                                Console.WriteLine("✅ All priority files processed!");
                                _canProcessNonPriorityFiles.Set();
                            }
                        }
                    }
                }

                totalFilesLeft--;
                int progression = (int)((double)(totalFiles - totalFilesLeft) / totalFiles * 100);

                // Send progress update
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
