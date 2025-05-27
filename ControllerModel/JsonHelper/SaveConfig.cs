using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel.JsonHelper
{
    public class SaveConfig
    {
        public string PathToLogDaily { get; set; }
        public string Language { get; set; }
        public string PathTologStatus { get; set; }
        public string[] ExtensionFileCrypt { get; set; }
        public string BlockingApp { get; set; }
        public string[] ExtensionPriorityFile { get; set; }
        public int LargeFileThreshold { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveConfig"/> class.
        /// </summary>
        /// <param name="PathTologDaily">Chemin du log journalier.</param>
        /// <param name="PathTologStatus">Chemin du log de statut.</param>
        /// <param name="Language">Code de langue (culture).</param>
        /// <param name="ExtensionFileCrypt">Extension des fichiers à crypter.</param>
        /// <param name="BlockingApp">Application de blocage.</param>
        /// <param name="ExtensionPriorityFile">Extension des fichiers prioritaires.</param>
        /// <param name="largeFileThreshold">Size threshold for files considered large in KB.</param>
        public SaveConfig(string PathTologDaily, string PathTologStatus, string Language, string[] ExtensionFileCrypt, string[] ExtensionPriorityFile, string BlockingApp, int largeFileThreshold) 
        { 
            this.PathToLogDaily = PathTologDaily;
            this.PathTologStatus = PathTologStatus;
            this.Language = Language;
            this.ExtensionFileCrypt = ExtensionFileCrypt;
            this.ExtensionPriorityFile = ExtensionPriorityFile;
            this.BlockingApp = BlockingApp;
            this.LargeFileThreshold = largeFileThreshold;
        }

    }
}
