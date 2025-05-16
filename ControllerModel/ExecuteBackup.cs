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
            Console.WriteLine($"Total file size: {totalFilesize} bytes");
            // transfer files
            foreach (var file in System.IO.Directory.GetFiles(sourcePath))
            {
                string fileName = System.IO.Path.GetFileName(file);
                string targetFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(file, targetFile, true);
                int progression = (int)(((double)(totalFiles - totalFilesLeft) / totalFiles) * 100);
                totalFilesLeft--;

            }
            stopwatch.Stop();

        }
        public void gerenateLog(string name, string FileSource, string FileTarget, string desPath, double fileSize, float FileTransferTime,DateTime time)
        {
        }
    }
}
