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
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

            this._pathToJob = Path.Combine(binPath, "job.json");
            this._pathToConfig = Path.Combine(binPath, "config.json");
            JsonHelperClassJsonReadMultipleObj jsonHelperClassJsonReadMultipleObj = new JsonHelperClassJsonReadMultipleObj();
            _jobList = jsonHelperClassJsonReadMultipleObj.ReadMultipleObj<JobObj>(this._pathToJob);
            
        }
        public void JobCreation(string name, string sourcePath, string targetPath, jobType type)
        {
            _jobList.Add(_backupJob.CreateJob(name, sourcePath, targetPath, type));
            jsonHelperClassJsonUpdate.Update(this._pathToJob, _jobList);
        }
        public void JobDeletion(int jobNum)
        {
            _backupJob.DeleteJob(_jobList[jobNum]);
            _jobList.RemoveAt(jobNum);
            jsonHelperClassJsonUpdate.Update(this._pathToJob, _jobList);
            

        }
        public int LaunchBackup(int jobNum)
        {
            if( jobNum == 0)
            {
                _executeBackup.ExecuteJobAll(_jobList);
                return 0;
            }
            int jobexit = _executeBackup.ExecuteJob(_jobList[jobNum-1]);
            if(jobexit == 0) { return 0; }
            else { return 1; }


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
    }
}
