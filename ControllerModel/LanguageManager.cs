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
        public LanguageManager()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string binPath = Path.GetDirectoryName(asm.Location);
            binPathGlobal = binPath;
            
            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPath, "config.json"));
            saveConfigObj = SaveConfig;
            SetLanguage(SaveConfig._language);
        }
        public void SetLanguage(string cultureCode)
        {

            SaveConfig SaveConfig = jsonHelperClassJsonReadSingleObj.ReadSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"));
            SaveConfig._language = cultureCode;
            jsonHelperClassJsonUpdate.UpdateSingleObj<SaveConfig>(Path.Combine(binPathGlobal, "config.json"), SaveConfig);

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);


        }

        public string Get(string key)
        {
            return resManager.GetString(key);
        }
    }
}