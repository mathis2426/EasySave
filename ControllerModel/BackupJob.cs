using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibrairieJsonHelper;

namespace ControllerModel
{
    public class BackupJob
    {
        public State state = new State();
        public JobObj CreateJob(string name, string sourcePath, string targetPath, jobType type)
        {
           JobObj job = new (name, sourcePath, targetPath, type);
           state.StateAddDelete(job);
           return job;
        }
        public void DeleteJob(JobObj jobs)
        {
            state.StateAddDelete(jobs);
        }
    }
}
