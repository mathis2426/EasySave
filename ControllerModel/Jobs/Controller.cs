using System.Reflection;
using System.Runtime.ConstrainedExecution;
using ControllerModel.JsonHelper;

namespace ControllerModel.Jobs
{
    public class JobManager
    {
        /// <summary>
        /// Liste des jobs de sauvegarde actuellement chargés.
        /// </summary>
        public List<JobObj> JobList = new();

        private readonly BackupJob _backupJob = new();
        private readonly ExecuteBackup _executeBackup = new();

        public JsonHelperFactory JsonHelperFactory = new();
        public JsonHelperClassJsonUpdate JsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();

        public SaveConfig SaveConfigObj;
        
        private readonly string _pathToJob = "";
        private readonly string _pathToConfig = "";

        /// <summary>
        /// Initialise un nouveau gestionnaire de jobs,
        /// charge les jobs existants depuis le fichier JSON.
        /// </summary>
        public JobManager() 
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

            _pathToJob = Path.Combine(binPath, "job.json");
            _pathToConfig = Path.Combine(binPath, "config.json");
            JsonHelperClassJsonReadMultipleObj jsonHelperClassJsonReadMultipleObj = new JsonHelperClassJsonReadMultipleObj();
            JobList = jsonHelperClassJsonReadMultipleObj.ReadMultipleObj<JobObj>(_pathToJob);
            
        }

        /// <summary>
        /// Crée un nouveau job avec les paramètres fournis, l'ajoute à la liste,
        /// puis met à jour le fichier JSON des jobs.
        /// </summary>
        /// <param name="name">Nom du job.</param>
        /// <param name="sourcePath">Chemin source pour la sauvegarde.</param>
        /// <param name="targetPath">Chemin cible pour la sauvegarde.</param>
        /// <param name="type">Type de job.</param>
        public void JobCreation(string name, string sourcePath, string targetPath, JobType type)
        {
            JobList.Add(_backupJob.CreateJob(name, sourcePath, targetPath, type));
            JsonHelperClassJsonUpdate.Update(_pathToJob, JobList);
        }

        /// <summary>
        /// Supprime un job identifié par son index dans la liste,
        /// met à jour la liste et le fichier JSON correspondant.
        /// </summary>
        /// <param name="jobNum">Index du job à supprimer.</param>
        public void JobDeletion(int jobNum)
        {
            _backupJob.DeleteJob(JobList[jobNum]);
            JobList.RemoveAt(jobNum);
            JsonHelperClassJsonUpdate.Update(_pathToJob, JobList);
            

        }

        /// <summary>
        /// Lance la sauvegarde d'un job spécifique ou de tous les jobs.
        /// Si jobNum vaut 0, exécute tous les jobs.
        /// </summary>
        /// <param name="jobNum">Index du job à exécuter (1-based), ou 0 pour tous les jobs.</param>
        /// <returns>Retourne 0 si la sauvegarde s'est bien déroulée, sinon 1.</returns>
        public int LaunchBackup(int jobNum)
        {
            if( jobNum == 0)
            {
                _executeBackup.ExecuteJobAll(JobList);
                return 0;
            }
            int jobexit = _executeBackup.ExecuteJob(JobList[jobNum-1]);
            if(jobexit == 0) { return 0; }
            else { return 1; }


        }

        /// <summary>
        /// Lance la sauvegarde d'un job par son nom depuis la ligne de commande.
        /// Affiche un message si le job n'est pas trouvé.
        /// </summary>
        /// <param name="job">Nom du job à exécuter.</param>
        public void LaunchBackupCommandLine(string job)
        {
            int indexJob = JobList.FindIndex(x => x.Name == job);
            if (indexJob == -1)
            {
                Console.WriteLine("Job not found");
                return;
            }
            else
            {
                _executeBackup.ExecuteJob(JobList[indexJob]);
            }
        }
        public void SetBlockingProcess(string process)
        {
            _executeBackup.SetBlockingProcess(process);
        }
        public void SetBlockingApp(string app)
        {
            _executeBackup.SetBlockingApp(app);
        }
    }
}
