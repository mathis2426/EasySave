namespace ControllerModel
{
    public class JobManager
    {
        public List<JobObj> _jobList = new List<JobObj>();
        private BackupJob _backupJob = new BackupJob();
        private ExecuteBackup _executeBackup = new ExecuteBackup();
        public JobManager() { }
        public void JobCreation(string name, string sourcePath, string targetPath, jobType type)
        {
            _jobList.Add(_backupJob.createJob(name, sourcePath, targetPath, type));
        }
        public void JobDeletion(int jobNum)
        {
            _jobList.RemoveAt(jobNum);
            _backupJob.deleteJob(_jobList);
        }
        public void LaunchBackup(int jobNum)
        {
            _executeBackup.ExecuteJob(_jobList[jobNum]);
        }
        public void ReadJson()
        {
        }
    }
}
