using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Runtime.InteropServices.Marshalling;
using System.Reflection;
using ControllerModel.JsonHelper;
using ControllerModel.XmlHelper;

namespace ControllerModel.Logs2
{
    public class Daily : AbstractLogger
    {
        private readonly string _pathToLog;
        private LogObject _logObject;

        /// <summary>
        /// Initialise une nouvelle instance de la classe Daily,
        /// et définit le chemin vers le fichier de log quotidien.
        /// </summary>
        public Daily()
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

            _pathToLog = Path.Combine(binPath,"logDaily.json");
        }

        /// <summary>
        /// Prépare un objet log avec les paramètres fournis,
        /// met à jour l'objet de log interne et génère le log.
        /// </summary>
        /// <param name="name">Nom du job ou de la tâche.</param>
        /// <param name="fileSource">Chemin source du fichier sauvegardé.</param>
        /// <param name="fileTarget">Chemin cible du fichier sauvegardé.</param>
        /// <param name="fileSize">Taille du fichier en octets.</param>
        /// <param name="fileTransferTime">Durée du transfert du fichier en millisecondes.</param>
        /// <param name="time">Horodatage de l'opération.</param>
        public void sendParamToLog(
            string name,
            string fileSource,
            string fileTarget,
            long fileSize,
            long fileTransferTime,
            DateTime time)
        {
            LogObject _logObject = new LogObject(name, fileSource, fileTarget, _pathToLog, fileSize, fileTransferTime, time);
            this._logObject = _logObject.getLog();
            GenerateLog();
        }

        /// <summary>
        /// Implémentation de la méthode abstraite GenerateLog.
        /// Utilise un logger JSON pour écrire les données de log dans le fichier défini.
        /// </summary>
        public override void GenerateLog()
        {
            ILoggerWriter jsonLog = JsonHelperFactory.CreateLoggerDaily();
            ILoggerWriter xmlLog = XmlHelperFactory.CreateLoggerDaily();
            jsonLog.WriteLog(_pathToLog, _logObject);
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            xmlLog.WriteLog(Path.Combine(binPath,"logDaily.xml"), _logObject);
        }
    }
}