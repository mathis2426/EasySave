using System.Reflection;
using System.Runtime.ConstrainedExecution;
using LibrairieJsonHelper;

namespace ControllerModel
{
    public class JobManager
    {
        public List<JobObj> _jobList = new List<JobObj>();
        private BackupJob _backupJob = new BackupJob();
        private ExecuteBackup _executeBackup = new ExecuteBackup();

        public JsonHelperFactory JsonHelperFactory = new JsonHelperFactory();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();

        public SaveConfig saveConfigObj;
        
        public string _pathToJob = "";
        public string _pathToConfig = "";
        public JobManager() 
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string binPath = Path.GetDirectoryName(asm.Location);

            _pathToJob = Path.Combine(binPath, "job.json");

            JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPath, "config.json"));
            saveConfigObj = SaveConfig;
            this._pathToConfig = Path.Combine(binPath, "config.json");
            JsonHelperClassJsonReadMultipleObj jsonHelperClassJsonReadMultipleObj = new JsonHelperClassJsonReadMultipleObj();
            _jobList = jsonHelperClassJsonReadMultipleObj.ReadMultipleObj<JobObj>(this._pathToJob);
            
        }
        public void JobCreation(string name, string sourcePath, string targetPath, jobType type)
        {
            _jobList.Add(_backupJob.createJob(name, sourcePath, targetPath, type));
            jsonHelperClassJsonUpdate.Update(this._pathToJob, _jobList);
        }
        public void JobDeletion(int jobNum)
        {
            _jobList.RemoveAt(jobNum);
            jsonHelperClassJsonUpdate.Update(this._pathToJob, _jobList);
            _backupJob.deleteJob(_jobList[jobNum]);

        }
        public void LaunchBackup(int jobNum)
        {
            if( jobNum == 0)
            {
                _executeBackup.ExecuteJobAll(_jobList);
                return;
            }
            _executeBackup.ExecuteJob(_jobList[jobNum-1]);
        }
        public void LaunchBackupCommandLine(string job)
        {
            int indexJob = _jobList.FindIndex(x => x._name == job);
            if (indexJob == -1)
            {
                Console.WriteLine("Job not found");
                return;
            }
            else
            {
                _executeBackup.ExecuteJob(_jobList[indexJob]);
            }
        }
        public void ChangeLogPath(string path)
        {
            this.saveConfigObj._pathTologStatus = Path.Combine(path, "state.json");
            this.saveConfigObj._pathToLogDaily = Path.Combine(path, "daily.json");
            jsonHelperClassJsonUpdate.UpdateSingleObj<SaveConfig>(this._pathToConfig, saveConfigObj);
        }
    }
}
