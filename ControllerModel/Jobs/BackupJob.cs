using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerModel.Logs2;


namespace ControllerModel.Jobs
{
    public class BackupJob
    {
        private readonly State _state = new();
        /// <summary>
        /// Creates a new backup job with the specified parameters,
        /// adds it to the current state and returns the created job.
        /// </summary>
        /// <param name="name">The name of the backup job.</param>
        /// <param name="sourcePath">The source path of the files to be backed up.</param>
        /// <param name="targetPath">The target path where the backup will be stored.</param>
        /// <param name="type">The type of job (jobType) to be created.</param>
        /// <returns>The new JobObj object representing the job created.</returns>
        public JobObj CreateJob(int id, string name, string sourcePath, string targetPath, JobType type)
        {
           JobObj job = new (id, name, sourcePath, targetPath, type);
           _state.StateAddDelete(job);
           return job;
        }
        /// <summary>
        /// Deletes an existing job from the current state.
        /// </summary>
        /// <param name="jobs">The JobObj object representing the job to be deleted.</param>
        public void DeleteJob(JobObj jobs)
        {
            _state.StateAddDelete(jobs);
        }
    }
}
