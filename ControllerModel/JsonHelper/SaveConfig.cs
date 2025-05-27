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
        /// <param name="PathTologDaily">Daily log path. </param>
        /// <param name="PathTologStatus">Status log path. </param>
        /// <param name="Language">Language code (culture). </param>
        /// <param name="ExtensionFileCrypt">File extension to encrypt. </param>
        /// <param name="BlockingApp">Application blocking. </param>
        /// <param name="ExtensionPriorityFile">Priority file extension. </param>
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
