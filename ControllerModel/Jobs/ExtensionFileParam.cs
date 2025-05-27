using ControllerModel.Jobs;
using ControllerModel.JsonHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class FileParam
    {
        public JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();
        public SaveConfig saveConfig;
        public string binPathGlobal;

        public FileParam()
        {
            binPathGlobal = Path.GetDirectoryName(AppContext.BaseDirectory);
            saveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
        }
        /// <summary>
        /// Sets the application language and updates the configuration file.
        /// </summary>
        /// <param name="ExtensionFileCryptoSoft">List of extensions to modify.</param>
        public void SetExtensionFileCryptoSoft(string[] ExtensionFileCryptoSoft)
        {
            saveConfig.ExtensionFileCrypt = ExtensionFileCryptoSoft;
            jsonHelperClassJsonUpdate.UpdateSingleObj(Path.Combine(binPathGlobal, "config.json"), saveConfig);
        }

        public string[] getListExtensionFilesCryptoSoft()
        {
            return saveConfig.ExtensionFileCrypt;
        }

        public void SetExtensionPriorityFile(string[] ExtensionPriorityFile)
        {
            saveConfig.ExtensionPriorityFile = ExtensionPriorityFile;
            jsonHelperClassJsonUpdate.UpdateSingleObj(Path.Combine(binPathGlobal, "config.json"), saveConfig);
        }

        public string[] getListExtensionPriorityFiles()
        {
            return saveConfig.ExtensionPriorityFile;
        }

        public string GetBlockingApp()
        {
            return saveConfig.BlockingApp;
        }
        public void SetBlockingApp(string app)
        {
            saveConfig.BlockingApp = app;
            jsonHelperClassJsonUpdate.UpdateSingleObj(Path.Combine(binPathGlobal, "config.json"), saveConfig);
        }

        public int GetLargeFileThreshold()
        {
            return saveConfig.LargeFileThreshold;
        }

        public void SetLargeFileThreshold(int largeFileThreshold)
        {
            saveConfig.LargeFileThreshold = largeFileThreshold;
            jsonHelperClassJsonUpdate.UpdateSingleObj(Path.Combine(binPathGlobal, "config.json"), saveConfig);
        }
    }
}
