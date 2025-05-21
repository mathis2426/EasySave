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
        public string BlockingProcess { get; set; }


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SaveConfig"/>.
        /// </summary>
        /// <param name="PathTologDaily">Chemin du log journalier.</param>
        /// <param name="PathTologStatus">Chemin du log de statut.</param>
        /// <param name="Language">Code de langue (culture).</param>
        public SaveConfig(string PathTologDaily, string PathTologStatus, string Language,string BlockingProcess) 
        { 
            this.PathToLogDaily = PathTologDaily;
            this.PathTologStatus = PathTologStatus;
            this.Language = Language;
            this.BlockingProcess = BlockingProcess;
        }

    }
}
