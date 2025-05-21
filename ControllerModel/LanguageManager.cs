using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using LibrairieJsonHelper;

namespace ControllerModel
{
    public class LanguageManager
    {
        public ResourceManager resManager = new ResourceManager("ControllerModel.Resources.Lang", Assembly.GetExecutingAssembly());
        public JsonHelperClassJsonReadSingleObj jsonHelperClassJsonReadSingleObj = JsonHelperFactory.CreateJsonReadSingleObj();
        public JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = JsonHelperFactory.CreateJsonUpdate();
        public SaveConfig saveConfigObj;
        public string binPathGlobal;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="LanguageManager"/> et configure la langue.
        /// </summary>
        public LanguageManager()
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            binPathGlobal = binPath;

            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPath, "config.json"));
            saveConfigObj = SaveConfig;
            SetLanguage(SaveConfig._language);
        }

        /// <summary>
        /// Définit la langue de l'application et met à jour le fichier de configuration.
        /// </summary>
        /// <param name="cultureCode">Code de la culture (ex. : "en-US", "fr-FR").</param>
        public void SetLanguage(string cultureCode)
        {

            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
            SaveConfig._language = cultureCode;
            jsonHelperClassJsonUpdate.UpdateSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"), SaveConfig);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
        }

        /// <summary>
        /// Récupère une chaîne localisée à partir des ressources.
        /// </summary>
        /// <param name="key">Clé de la ressource.</param>
        /// <returns>Chaîne localisée correspondant à la clé.</returns>
        public string Get(string key)
        {
            return resManager.GetString(key);
        }
    }
}