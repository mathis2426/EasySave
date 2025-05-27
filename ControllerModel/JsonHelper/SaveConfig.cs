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

        /// <summary>
        /// Initializes a new instance of the <see cref="SaveConfig"/> class.
        /// </summary>
        /// <param name="PathTologDaily">Daily log path.</param>
        /// <param name="PathTologStatus">Status log path.</param>
        /// <param name="Language">Language code (culture). </param>
        /// <param name="ExtensionFileCrypt">Extension of files to encrypt.</param>
        /// <param name="BlockingApp">Blocking application.</param>
        /// <param name="ExtensionPriorityFile">Extension of priority files.</param>
        public SaveConfig(string PathTologDaily, string PathTologStatus, string Language, string[] ExtensionFileCrypt, string[] ExtensionPriorityFile, string BlockingApp) 
        { 
            this.PathToLogDaily = PathTologDaily;
            this.PathTologStatus = PathTologStatus;
            this.Language = Language;
            this.ExtensionFileCrypt = ExtensionFileCrypt;
            this.ExtensionPriorityFile = ExtensionPriorityFile;
            this.BlockingApp = BlockingApp;
        }

    }
}
