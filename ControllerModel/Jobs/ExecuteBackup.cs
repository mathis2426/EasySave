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
                Thread thread = new Thread(() => ExecuteJob(job));
                thread.Start();
                threads.Add(thread);
            }
        }

        /// <summary>
        /// Runs a backup for a given job.
        /// Checks the existence of source and target paths, measures execution time,
        /// and logs job information.
        /// </summary>
        /// <param name="job">The backup job to run.</param>
        /// <returns>0 if the backup was successful, 1 otherwise (e.g. invalid path).</returns>
        public int ExecuteJob(JobObj job)
        {
            if (_saveConfig.BlockingApp != null && _saveConfig.BlockingApp != "")
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
            }
            string sourcePath = job.SourcePath;
            foreach (var file in Directory.GetFiles(sourcePath))
            {
                if (file.Contains(_saveConfig.BlockingApp) && file.EndsWith("exe"))
                {
                    Console.WriteLine($"Application {_saveConfig.BlockingApp} detecté demarage annulé");
                    return 1;
                }
            }

            Console.WriteLine("Execute job");
            // Simulate file transfer
            string targetPath = job.TargetPath;
            string name = job.Name;

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
                    FullBackup(name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft, fileEncryptionTimes);
                    break;
                case JobType.Differential:
                    DifferentialBackup(name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft, fileEncryptionTimes);
                    break;
            }
            stopwatch.Stop();

            _logDaily.sendParamToLog(
                name,
                sourcePath,
                targetPath,
                totalFileSize,
                stopwatch.ElapsedMilliseconds,
                DateTime.Now,
                fileEncryptionTimes
            );
            Console.WriteLine($"[END] Job {name} terminé en {stopwatch.ElapsedMilliseconds} ms");
            return 0;
        }
        // Backup methods

        /// <summary>
        /// Performs a full backup: deletes all files in the target
        /// and copies all files from the source to the target.
        /// Updates the progress in the report.
        /// </summary>
        /// <param name="name">Job name.</param>
        /// <param name="sourcePath">Source path for files to be backed up.</param>
        /// <param name="targetPath">Target path for backup. </param>
        /// <param name="totalFiles">Total number of files to be backed up.</param>
        /// <param name="totalFileSize">Total size of files to be backed up in bytes.</param>
        /// <param name="totalFilesLeft">Number of files remaining to be processed.</param>
        public void FullBackup(string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes)
        {
            foreach (string file in Directory.GetFiles(targetPath))
            {
                File.Delete(file);
            }
            foreach (var file in Directory.GetFiles(sourcePath))
            {
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
            }

        }

        /// <summary>
        /// Performs a differential backup: copies only modified or new files.
        /// Updates the progress in the report.
        /// </summary>
        /// <param name="name">Job name.</param>
        /// <param name="sourcePath">Source path for files to be backed up. </param>
        /// <param name="targetPath">Target path for backup.</param>
        /// <param name="totalFiles">Total number of files to scan.</param>
        /// <param name="totalFileSize">Total size of files to scan in bytes.</param>
        /// <param name="totalFilesLeft">Number of files remaining to process.</param>
        public void DifferentialBackup(string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes)
        {
            foreach (string sourceFilePath in Directory.GetFiles(sourcePath))
            {
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
            }
        }
    }
}