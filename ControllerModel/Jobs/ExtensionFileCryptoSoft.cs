using ControllerModel.JsonHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class ExtensionFileCryptoSoft
    {
        public JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();
        public string binPathGlobal;
        /// <summary>
        /// Définit la langue de l'application et met à jour le fichier de configuration.
        /// </summary>
        /// <param name="cultureCode">Code de la culture (ex. : "en-US", "fr-FR").</param>
        public void SetExtensionFileCryptoSoft(string[] ExtensionFileCryptoSoft)
        {
            binPathGlobal = Path.GetDirectoryName(AppContext.BaseDirectory);

            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
            SaveConfig.ExtensionFileCrypt = ExtensionFileCryptoSoft;
            jsonHelperClassJsonUpdate.UpdateSingleObj(Path.Combine(binPathGlobal, "config.json"), SaveConfig);

            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(ExtensionFileCryptoSoft);
        }
    }
}
