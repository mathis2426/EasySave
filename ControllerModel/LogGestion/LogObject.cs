using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ControllerModel.Logs2
{
    public class LogObject
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public string DesPath { get; set; }
        public double FileSize { get; set; }
        public long FileTransferTime { get; set; }
        public DateTime Time { get; set; }


        /// <summary>
        /// Constructeur utilisé pour la désérialisation JSON.
        /// </summary>
        /// <param name="Name">Nom du job.</param>
        /// <param name="FileSource">Chemin source.</param>
        /// <param name="FileTarget">Chemin de destination.</param>
        /// <param name="DesPath">Chemin du fichier de log.</param>
        /// <param name="FileSize">Taille totale des fichiers transférés.</param>
        /// <param name="FileTransferTime">Temps de transfert en ms.</param>
        /// <param name="Time">Date et heure du transfert.</param>
        [JsonConstructor]
        public LogObject(string Name, string FileSource, string FileTarget, string DesPath, double FileSize, long FileTransferTime, DateTime Time)
        {
            this.Name = Name;
            this.FileSource = FileSource;
            this.FileTarget = FileTarget;
            this.DesPath = DesPath;
            this.FileSize = FileSize;
            this.FileTransferTime = FileTransferTime;
            this.Time = Time;
        }

        /// <summary>
        /// Retourne l'objet courant.
        /// </summary>
        /// <returns>Instance actuelle de <see cref="LogObject"/>.</returns>
        public LogObject getLog()
        {
            return this;
        }
    }
}