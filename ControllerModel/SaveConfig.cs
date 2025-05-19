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

        public SaveConfig(string _pathTologDaily, string _pathTologStatus, string _language) 
        { 
            this._pathToLogDaily = _pathTologDaily;
            this._pathTologStatus = _pathTologStatus;
            this._language = _language;
        }

    }
}
