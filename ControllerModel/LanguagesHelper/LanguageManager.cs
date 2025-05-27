using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using ControllerModel.JsonHelper;

namespace ControllerModel.LanguagesHelper
{
    public enum SupportedLanguage
    {
        English,
        Français,
    }

    public class LanguageManager
    {
        public ResourceManager ResManager = new ResourceManager("ControllerModel.Resources.Lang", Assembly.GetExecutingAssembly());
        public JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();
        public SaveConfig saveConfigObj;
        public string binPathGlobal;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageManager"/> class and configures the language.
        /// </summary>
        public LanguageManager()
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            binPathGlobal = binPath;
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPath, "config.json"));
            saveConfigObj = SaveConfig;
            SetLanguage(saveConfigObj.Language);
        }

        /// <summary>
        /// Sets the application language and updates the configuration file.
        /// </summary>
        /// <param name="cultureCode">Culture code (e.g. "en-US", "fr-FR").</param>
        public void SetLanguage(string cultureCode)
        {
            if(cultureCode == null && cultureCode == "")
            {
                cultureCode = "en-US";
            }
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
            SaveConfig.Language = cultureCode;
            jsonHelperClassJsonUpdate.UpdateSingleObj(Path.Combine(binPathGlobal, "config.json"), SaveConfig);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
        }

        /// <summary>
        /// Retrieves a localized string from resources.
        /// </summary>
        /// <param name="key">Resource key.</param>
        /// <returns>Localized string corresponding to the key.</returns>
        public string Get(string key)
        {
            return ResManager.GetString(key);
        }
    }
}