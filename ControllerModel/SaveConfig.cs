using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class SaveConfig
    {
        public string _pathToLogDaily { get; set; }
        public string _language { get; set; }
        public string _pathTologStatus { get; set; }


        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="SaveConfig"/>.
        /// </summary>
        /// <param name="_pathTologDaily">Chemin du log journalier.</param>
        /// <param name="_pathTologStatus">Chemin du log de statut.</param>
        /// <param name="_language">Code de langue (culture).</param>
        public SaveConfig(string _pathTologDaily, string _pathTologStatus, string _language) 
        { 
            this._pathToLogDaily = _pathTologDaily;
            this._pathTologStatus = _pathTologStatus;
            this._language = _language;
        }

    }
}
