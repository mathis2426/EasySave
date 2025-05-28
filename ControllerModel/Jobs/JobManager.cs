using System.Diagnostics;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using ControllerModel.JsonHelper;
using ControllerModel.LanguagesHelper;

namespace ControllerModel.Jobs
{
    public class JobManager
    {
        /// <summary>
        /// List of backup jobs currently loaded.
        /// </summary>
        public List<JobObj> JobList = new();

        
        public static Dictionary<int, (Thread Thread, CancellationTokenSource TokenSource, ManualResetEventSlim PauseEvent, int ButtonStatus, double progressBarPercent)> threadsByJob = [];

       

        private readonly BackupJob _backupJob = new();
        private readonly ExecuteBackup _executeBackup = new();
        public ExecuteBackup ExecuteBackup { get { return _executeBackup; } }
        public JsonHelperFactory JsonHelperFactory = new();
        public JsonHelperClassJsonUpdate JsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();
        public JsonHelperClassJsonReadSingleObj jsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();

        public FileParam ExtensionFileParam;

        public SaveConfig SaveConfigObj;

        private readonly string _pathToJob = "";
        private readonly string _pathToConfig = "";

        /// <summary>
        /// Initializes a new job manager,
        /// loads existing jobs from the JSON file.
        /// </summary>
        public JobManager()
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

            _pathToJob = Path.Combine(binPath, "job.json");
            _pathToConfig = Path.Combine(binPath, "config.json");
            JsonHelperClassJsonReadMultipleObj jsonHelperClassJsonReadMultipleObj = new JsonHelperClassJsonReadMultipleObj();
            JobList = jsonHelperClassJsonReadMultipleObj.ReadMultipleObj<JobObj>(_pathToJob);
            FileParam ExtensionFileParamtemp = new(_executeBackup._saveConfig);
            ExtensionFileParam = ExtensionFileParamtemp;
        }

        /// <summary>
        /// Creates a new job with the parameters provided, adds it to the list,
        /// then updates the jobs JSON file.
        /// </summary>
        /// <param name="name">Job name. </param>
        /// <param name="sourcePath">Source path for backup.</param>
        /// <param name="targetPath">Target path for backup.</param>
        /// <param name="type">Job type.</param>
        public void JobCreation(string name, string sourcePath, string targetPath, JobType type)
        {
            int nextId = Enumerable.Range(1, JobList.Count + 1)
                           .Except(JobList.Select(j => j.Id))
                           .First();

            JobObj job = _backupJob.CreateJob(nextId, name, sourcePath, targetPath, type);
            JobList.Add(job);
            JsonHelperClassJsonUpdate.Update(_pathToJob, JobList);
        }

        /// <summary>
        /// Deletes a job identified by its index in the list,
        /// updates the list and the corresponding JSON file.
        /// </summary>
        /// <param name="jobNum">Index of the job to be deleted.</param>
        public void JobDeletion(int jobNum)
        {
            _backupJob.DeleteJob(JobList[jobNum]);
            JobList.RemoveAt(jobNum);
            JsonHelperClassJsonUpdate.Update(_pathToJob, JobList);
        }

        /// <summary>
        /// Starts the backup of a specific job or all jobs.
        /// If jobNum is 0, runs all jobs.
        /// </summary>
        /// <param name="jobNum">Index of the job to run (1-based), or 0 for all jobs.</param>
        /// <returns>Returns 0 if the backup went well, otherwise 1.</returns>
        public int LaunchBackup(int jobNum)
        {
            var tokenSource = new CancellationTokenSource();
            var pauseEvent = new ManualResetEventSlim(true);

            string appToDetect = _executeBackup._saveConfig.BlockingApp;
            if (appToDetect != null && appToDetect != "")
            {
                Thread monitoringThread = new Thread(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        var runningProcesses = Process.GetProcessesByName(appToDetect);
                        if (runningProcesses.Length > 0)
                        {
                            foreach (var kvp in threadsByJob.Values)
                            {
                                kvp.PauseEvent.Reset();
                            }
                        }
                        else
                        {
                            foreach (var kvp in threadsByJob.Values)
                            {
                                kvp.PauseEvent.Set();
                            }
                        }

                        Thread.Sleep(50);
                    }

                });
                monitoringThread.Start();
                }
                if (jobNum == 0)
            {
                _executeBackup.ExecuteJobAll(JobList);
                return 0;
            }

            

            Thread thread = new Thread(() =>
            {
                try
                {
                    _executeBackup.ExecuteJob(JobList[jobNum - 1], tokenSource.Token, pauseEvent);
                }
                catch (OperationCanceledException)
                {
                    //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH
                }
                finally { threadsByJob.Remove(jobNum); }
            });
            threadsByJob[jobNum] = (thread, tokenSource, pauseEvent, 1, 0);
            thread.Start();         
            return 0;

        }

        /// <summary>
        /// Starts saving a job by name from the command line.
        /// Displays a message if the job is not found.
        /// </summary>
        /// <param name="job">Name of the job to run.</param>
        public void LaunchBackupCommandLine(string job)
        {
            int indexJob = JobList.FindIndex(x => x.Name == job);
            if (indexJob == -1)
            {
                Console.WriteLine("Job not found");
                return;
            }
            else
            {
                var tokenSource = new CancellationTokenSource();
                var pauseEvent = new ManualResetEventSlim(true);

                Thread thread = new Thread(() =>
                {
                    try
                    {
                        _executeBackup.ExecuteJob(JobList[indexJob], tokenSource.Token, pauseEvent);
                    }
                    catch (OperationCanceledException e)
                    {
                        throw e;
                    }

                });

            }
        }

        /// <summary>
        /// Updates the configuration file with the file extension parameters.
        /// </summary>
        /// <param name="extensionFile">List of extensions to encrypt</param>
        public void UpdateExtensionFileCryptoSoft(string[] extensionFile)
        {
            ExtensionFileParam.SetExtensionFileCryptoSoft(extensionFile);
        }

        /// <summary>
        /// Retrieves the list of file extensions to be encrypted from the configuration file.
        /// </summary>
        /// <returns></returns>
        public string[] getListExtensionFilesCryptoSoft()
        {
            return ExtensionFileParam.getListExtensionFilesCryptoSoft();
        }

        /// <summary>
        /// Updates the configuration file with file extension priorities.
        /// </summary>
        /// <param name="extensionPriorityFile">List of priority extensions</param>
        public void UpdateExtensionPriorityFile(string[] extensionPriorityFile)
        {
            ExtensionFileParam.SetExtensionPriorityFile(extensionPriorityFile);
        }

        /// <summary>
        /// Retrieves the list of priority file extensions from the configuration file.
        /// </summary>
        /// <returns></returns>
        
        public string[] getListExtensionPriorityFiles()
        {
            return ExtensionFileParam.getListExtensionPriorityFiles();
        }
        public string GetBlockingApp()
        {
            return _executeBackup._saveConfig.BlockingApp;
        }

        public void SetBlockingApp(string app)
        {
            _executeBackup._saveConfig.BlockingApp = app;
            JsonHelperClassJsonUpdate.UpdateSingleObj(_pathToConfig, _executeBackup._saveConfig);
        }

        public int GetLargeFileThreshold()
        {
            return _executeBackup._saveConfig.LargeFileThreshold;
        }

        public void SetLargeFileThreshold(int largeFileThreshold)
        {
            _executeBackup._saveConfig.LargeFileThreshold = largeFileThreshold;
            JsonHelperClassJsonUpdate.UpdateSingleObj(_pathToConfig, _executeBackup._saveConfig);
        }
    }
}
