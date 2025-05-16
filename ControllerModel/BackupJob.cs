using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class BackupJob
    {
        public JobObj createJob(string name, string sourcePath, string targetPath, jobType type)
        {
           JobObj job = new (name, sourcePath, targetPath, type);
           return job;
        }
        public void deleteJob(List<JobObj> jobs)
        {

        }
    }
}
