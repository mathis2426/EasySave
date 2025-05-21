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
        private readonly State _state = new();
        /// <summary>
        /// Crée un nouveau job de sauvegarde avec les paramètres spécifiés,
        /// l'ajoute à l'état courant et retourne le job créé.
        /// </summary>
        /// <param name="name">Le nom du job de sauvegarde.</param>
        /// <param name="sourcePath">Le chemin source des fichiers à sauvegarder.</param>
        /// <param name="targetPath">Le chemin cible où la sauvegarde sera stockée.</param>
        /// <param name="type">Le type de job (jobType) à créer.</param>
        /// <returns>Le nouvel objet JobObj représentant le job créé.</returns>
        public JobObj CreateJob(string name, string sourcePath, string targetPath, JobType type)
        {
           JobObj job = new (name, sourcePath, targetPath, type);
           _state.StateAddDelete(job);
           return job;
        }
        /// <summary>
        /// Supprime un job existant de l'état courant.
        /// </summary>
        /// <param name="jobs">L'objet JobObj représentant le job à supprimer.</param>
        public void DeleteJob(JobObj jobs)
        {
            _state.StateAddDelete(jobs);
        }
    }
}
