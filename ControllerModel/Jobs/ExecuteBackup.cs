using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Diagnostics;
using ControllerModel.Logs2;
using ControllerModel.JsonHelper;
using System.Reflection;

namespace ControllerModel.Jobs
{
    public class ExecuteBackup
    {
        // Properties
        private readonly Daily _logDaily = new();
        private readonly State _state = new();
        private string[] _listExtensionFileCrypt;

        public JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();
        public SaveConfig _saveConfig;
        //public SaveConfig saveConfigObjJob;
        public string binPathGlobal;

        public ExecuteBackup()
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPath, "config.json"));
            this._saveConfig = SaveConfig;
            _listExtensionFileCrypt = this._saveConfig.ExtensionFileCrypt;
        }

        /// <summary>
        /// Exécute la sauvegarde pour tous les jobs présents dans la liste.
        /// </summary>
        /// <param name="JobList">Liste des jobs de sauvegarde à exécuter.</param>
        public void ExecuteJobAll(List<JobObj> JobList)
        {

            foreach (var job in JobList)
            {
                ExecuteJob(job);
            }

        }

        /// <summary>
        /// Exécute une sauvegarde pour un job donné.
        /// Vérifie l'existence des chemins source et cible, mesure le temps d'exécution,
        /// et loggue les informations liées au job.
        /// </summary>
        /// <param name="job">Le job de sauvegarde à exécuter.</param>
        /// <returns>0 si la sauvegarde a réussi, 1 sinon (ex : chemin non valide).</returns>
        public int ExecuteJob(JobObj job)
        {
            // Simulate file transfer
            string sourcePath = job.SourcePath;
            string targetPath = job.TargetPath;
            string name = job.Name;

            // Timer
            Stopwatch stopwatch = new();
            stopwatch.Start();
            if (!Directory.Exists(sourcePath)) { return 1; }
            if (!Directory.Exists(targetPath)) { return 1; }

            int totalFiles = Directory.GetFiles(sourcePath).Length;
            int totalFilesLeft = totalFiles;
            long totalFileSize = new DirectoryInfo(job.SourcePath).GetFiles().Sum(f => f.Length);

            Dictionary<string, long> fileEncryptionTimes = new();

            // transfer files
            switch (job.Type)
            {
                case JobType.Full:
                    FullBackup(name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft, fileEncryptionTimes);
                    break;
                case JobType.Differential:
                    DifferentialBackup(name, sourcePath, targetPath, totalFiles, totalFileSize, totalFilesLeft, fileEncryptionTimes);
                    break;
            }
            stopwatch.Stop();

            _logDaily.sendParamToLog(
                name,
                sourcePath,
                targetPath,
                totalFileSize,
                stopwatch.ElapsedMilliseconds,
                DateTime.Now,
                fileEncryptionTimes
            );
            return 0;
        }
        // Backup methods

        /// <summary>
        /// Effectue une sauvegarde complète : supprime tous les fichiers dans la cible
        /// et copie tous les fichiers du source vers la cible.
        /// Met à jour la progression dans l'état.
        /// </summary>
        /// <param name="name">Nom du job.</param>
        /// <param name="sourcePath">Chemin source des fichiers à sauvegarder.</param>
        /// <param name="targetPath">Chemin cible pour la sauvegarde.</param>
        /// <param name="totalFiles">Nombre total de fichiers à sauvegarder.</param>
        /// <param name="totalFileSize">Taille totale des fichiers à sauvegarder en octets.</param>
        /// <param name="totalFilesLeft">Nombre de fichiers restant à traiter.</param>
        public void FullBackup(string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes)
        {
            foreach (string file in Directory.GetFiles(targetPath))
            {
                File.Delete(file);
            }
            foreach (var file in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(file);
                string targetFile = Path.Combine(targetPath, fileName);
                long timeToEncrypt = 0;

                if (_listExtensionFileCrypt.Contains(Path.GetExtension(file)))
                {
                    Stopwatch encryptTimer = Stopwatch.StartNew();

                    string basePath = Path.Combine(Path.GetDirectoryName(AppContext.BaseDirectory), "win-x64/CryptoSoft.exe");

                    ProcessStartInfo processStartInfo = new ProcessStartInfo
                    {
                        FileName = basePath,
                        Arguments = $"\"{file}\" \"{targetFile}\"",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    Process processCryptoSoft = Process.Start(processStartInfo);

                    processCryptoSoft.WaitForExit();
                    encryptTimer.Stop();
                    timeToEncrypt = encryptTimer.ElapsedMilliseconds;

                    Console.WriteLine("Process terminé");
                } 
                else 
                { 
                    File.Copy(file, targetFile, true);
                }
                fileEncryptionTimes[fileName] = timeToEncrypt;
                totalFilesLeft--;

                int progression = (int)((double)(totalFiles - totalFilesLeft) / totalFiles * 100);

                _state.SendParamToLog(
                    name,
                    sourcePath,
                    targetPath,
                    StateEnumeration.In_progress,
                    totalFiles,
                    totalFileSize,
                    totalFilesLeft,
                    progression
                );
            }

        }

        /// <summary>
        /// Effectue une sauvegarde différentielle : copie uniquement les fichiers modifiés ou nouveaux.
        /// Met à jour la progression dans l'état.
        /// </summary>
        /// <param name="name">Nom du job.</param>
        /// <param name="sourcePath">Chemin source des fichiers à sauvegarder.</param>
        /// <param name="targetPath">Chemin cible pour la sauvegarde.</param>
        /// <param name="totalFiles">Nombre total de fichiers à analyser.</param>
        /// <param name="totalFileSize">Taille totale des fichiers à analyser en octets.</param>
        /// <param name="totalFilesLeft">Nombre de fichiers restant à traiter.</param>
        public void DifferentialBackup(string name, string sourcePath, string targetPath, int totalFiles, long totalFileSize, int totalFilesLeft, Dictionary<string, long> fileEncryptionTimes)
        {
            foreach (string sourceFilePath in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(sourceFilePath);
                string destFilePath = Path.Combine(targetPath, fileName);

                long timeToEncrypt = 0;

                if (!File.Exists(destFilePath) || File.GetLastWriteTime(sourceFilePath) > File.GetLastWriteTime(destFilePath))
                {
                    if (_listExtensionFileCrypt.Contains(Path.GetExtension(sourceFilePath)))
                    {
                        Stopwatch encryptTimer = Stopwatch.StartNew();
                        ProcessStartInfo processStartInfo = new ProcessStartInfo
                        {
                            FileName = "C:\\Users\\Mathis\\OneDrive\\Bureau\\cesi temporaire\\A3\\Bloc Génie logiciel\\Prosit-5\\Prosit5\\Prosit5\\bin\\Release\\net8.0\\Prosit5.exe",
                            Arguments = $"\"{sourceFilePath}\" \"{destFilePath}\"",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        Process processCryptoSoft = Process.Start(processStartInfo);
                        processCryptoSoft.WaitForExit();

                        encryptTimer.Stop();
                        timeToEncrypt = encryptTimer.ElapsedMilliseconds;

                        Console.WriteLine("Process terminé");
                    }
                    else
                    {
                        File.Copy(sourceFilePath, destFilePath, true);
                    }
                    fileEncryptionTimes[fileName] = timeToEncrypt;
                }
                
                totalFilesLeft--;
                int progression = (int)((double)(totalFiles - totalFilesLeft) / totalFiles * 100);
                
                _state.SendParamToLog(
                    name,
                    sourcePath,
                    targetPath,
                    StateEnumeration.In_progress,
                    totalFiles,
                    totalFileSize,
                    totalFilesLeft,
                    progression
                );
            }
        }
    }
}