using ControllerModel.JsonHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class ExtensionFileParam
    {
        public JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();
        public string binPathGlobal;

        public ExtensionFileParam()
        {
            binPathGlobal = Path.GetDirectoryName(AppContext.BaseDirectory);
        }
        /// <summary>
        /// Définit la langue de l'application et met à jour le fichier de configuration.
        /// </summary>
        /// <param name="ExtensionFileCryptoSoft">Liste des extensions à modifier.</param>
        public void SetExtensionFileCryptoSoft(string[] ExtensionFileCryptoSoft)
        {
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
            SaveConfig.ExtensionFileCrypt = ExtensionFileCryptoSoft;
            jsonHelperClassJsonUpdate.UpdateSingleObj(Path.Combine(binPathGlobal, "config.json"), SaveConfig);
        }

        public string[] getListExtensionFilesCryptoSoft()
        {
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
            return SaveConfig.ExtensionFileCrypt;
        }

        public void SetExtensionPriorityFile(string[] ExtensionPriorityFile)
        {
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
            SaveConfig.ExtensionPriorityFile = ExtensionPriorityFile;
            jsonHelperClassJsonUpdate.UpdateSingleObj(Path.Combine(binPathGlobal, "config.json"), SaveConfig);
        }

        public string[] getListExtensionPriorityFiles()
        {
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
            return SaveConfig.ExtensionPriorityFile;
        }
    }
}
