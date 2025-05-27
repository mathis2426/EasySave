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
            List<Thread> threads = new();

            foreach (var job in JobList)
            {
                // For each job, create and start a new thread executing ExecuteJob()
                Thread thread = new Thread(() => ExecuteJob(job));
                thread.Start();
                threads.Add(thread);
            }
        }

        // Executes a single backup job based on its type (Full or Differential)
        public int ExecuteJob(JobObj job)
        {
            Console.WriteLine($"[START] Job {job.Name} started in thread {Thread.CurrentThread.ManagedThreadId}");

            string sourcePath = job.SourcePath;
            string targetPath = job.TargetPath;
            string name = job.Name;

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
                    FullBackup(name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft, fileEncryptionTimes);
                    break;
                case JobType.Differential:
                    DifferentialBackup(name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft, fileEncryptionTimes);
                    break;
            }

            stopwatch.Stop();

            // Log job completion details
            _logDaily.sendParamToLog(name, sourcePath, targetPath, totalFileSize, stopwatch.ElapsedMilliseconds, DateTime.Now, fileEncryptionTimes);
            Console.WriteLine($"[END] Job {name} completed in {stopwatch.ElapsedMilliseconds} ms");
            return 0;
        }


        // Performs a full backup of all files from source to target folder
        public void FullBackup(string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes)
        {
            // Clean the target directory before starting backup
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
                string fileName = Path.GetFileName(file);
                string targetFile = Path.Combine(targetPath, fileName);
                string extension = Path.GetExtension(file);
                bool isPriority = _extensionPriorityFile.Contains(extension);
                long timeToEncrypt = 0;

                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} → processing {fileName}");

                if (!isPriority)
                {
                    // Non-priority files wait until all priority files are processed
                    Console.WriteLine("Waiting for non-priority files...");
                    _canProcessNonPriorityFiles.WaitOne();
                }

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
            }
        }


        // Performs differential backup: copies only changed or new files since last backup
        public void DifferentialBackup(string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes)
        {
            foreach (string sourceFilePath in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string destFilePath = Path.Combine(targetPath, fileName);
                string extension = Path.GetExtension(sourceFilePath);
                bool isPriority = _extensionPriorityFile.Contains(extension);
                long timeToEncrypt = 0;

                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} → processing {fileName}");

                if (!isPriority)
                {
                    // Non-priority files wait until all priority files are done
                    Console.WriteLine("Waiting for non-priority files...");
                    _canProcessNonPriorityFiles.WaitOne();
                }

                try
                {
                    // Copy file only if it doesn't exist or is newer than the one in target
                    if (!File.Exists(destFilePath) || File.GetLastWriteTime(sourceFilePath) > File.GetLastWriteTime(destFilePath))
                    {
                        if (_listExtensionFileCrypt.Contains(extension))
                        {
                            // Encrypt files requiring encryption
                            Stopwatch encryptTimer = Stopwatch.StartNew();
                            ProcessStartInfo processStartInfo = new()
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
                        }
                        else
                        {
                            // Copy normally otherwise
                            File.Copy(sourceFilePath, destFilePath, true);
                        }

                        fileEncryptionTimes[fileName] = timeToEncrypt;
                    }
                }
                finally
                {
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
                _state.SendParamToLog(name, sourcePath, targetPath, StateEnumeration.In_progress, totalFiles, totalFileSize, totalFilesLeft, progression);
            }
        }
    }
}
