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
        // Properties
        public Daily logDaily = new Daily();
        public State state = new State();

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
            long totalFileSize = new DirectoryInfo(job._sourcePath).GetFiles().Sum(f => f.Length);

            // transfer files
            switch (job._type)
            {
                case jobType.Full:
                    FullBackup(name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft);
                    break;
                case jobType.Differential:
                    DifferentialBackup(name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft);
                    break;
            }
            stopwatch.Stop();

            logDaily.sendParamToLog(
                name,
                sourcePath,
                targetPath,
                totalFileSize,
                stopwatch,
                DateTime.Now
            );

        }
        // Backup methods
        public void FullBackup(string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft)
        {
            foreach (string file in System.IO.Directory.GetFiles(targetPath))
            {
                File.Delete(file);
            }
             foreach (var file in System.IO.Directory.GetFiles(sourcePath))
            {
                string fileName = System.IO.Path.GetFileName(file);
                string targetFile = System.IO.Path.Combine(targetPath, fileName);
                System.IO.File.Copy(file, targetFile, true);
                int progression = (int)(((double)(totalFiles - totalFilesLeft) / totalFiles) * 100);
                state.sendParamToLog(
                    name,
                    sourcePath,
                    targetPath,
                    StateEnumeration.in_progress,
                    totalFiles,
                    totalFileSize, 
                    totalFilesLeft,
                    progression
                );
                totalFilesLeft--;
            }
            int progression2 = (int)(((double)(totalFiles - totalFilesLeft) / totalFiles) * 100);
            state.sendParamToLog(
                    name,
                    sourcePath,
                    targetPath,
                    StateEnumeration.in_progress,
                    totalFiles,
                    totalFileSize,
                    totalFilesLeft,
                    progression2
                );
        }
        public void DifferentialBackup(string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft)
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
                int progression = (int)(((double)(totalFiles - totalFilesLeft) / totalFiles) * 100);
                state.sendParamToLog(
                    name,
                    sourcePath,
                    targetPath,
                    StateEnumeration.in_progress,
                    totalFiles,
                    totalFileSize,
                    totalFilesLeft,
                    progression
                );
            }
        }
    }
}