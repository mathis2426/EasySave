using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Diagnostics;

namespace ControllerModel
{
    public class ExecuteBackup
    {
        public void ExecuteJobAll(List<JobObj> jobList)
        {
            foreach (var job in jobList)
            {
                ExecuteJob(job);
            }
        }
        public void ExecuteJob(JobObj job)
        {
            // Simulate file transfer
            string sourcePath = job._sourcePath;
            string targetPath = job._targetPath;
            string name = job._name;

            // Timer
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int totalFiles = System.IO.Directory.GetFiles(sourcePath).Length;
            int totalFilesLeft = totalFiles;
            long totalFilesize = new DirectoryInfo(job._sourcePath).GetFiles().Sum(f => f.Length);
            // transfer files
            switch(job._type)
            {
                case jobType.Full:
                    FullBackup(sourcePath, targetPath, totalFiles, totalFilesLeft);
                    break;
                case jobType.Differential:
                    DifferentialBackup(sourcePath, targetPath, totalFiles, totalFilesLeft);
                    break;
            }
            
            stopwatch.Stop();

        }
        public void FullBackup(string sourcePath, string targetPath, int totalFiles, int totalFilesLeft)
        {
            foreach( string file in System.IO.Directory.GetFiles(targetPath))
            {
                File.Delete(file);
            }
            foreach (var file in System.IO.Directory.GetFiles(sourcePath))
            {
                string fileName = System.IO.Path.GetFileName(file);
                string targetFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(file, targetFile, true);
                int progression = (int)(((double)(totalFiles - totalFilesLeft) / totalFiles) * 100);
                totalFilesLeft--;
            }
        }
        public void DifferentialBackup(string sourcePath, string targetPath, int totalFiles, int totalFilesLeft)
        {
            foreach (string sourceFilePath in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string destFilePath = Path.Combine(targetPath, fileName);

                if (!File.Exists(destFilePath) ||
                    File.GetLastWriteTime(sourceFilePath) > File.GetLastWriteTime(destFilePath))
                {
                    File.Copy(sourceFilePath, destFilePath, true);
                }
            }
        }
    }
}
