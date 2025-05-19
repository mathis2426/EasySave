using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class LanguageManager
    {
        public static ResourceManager resManager = new ResourceManager("View.Resources.Lang", Assembly.GetExecutingAssembly());

        public void SetLanguage(string cultureCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
        }

        public string Get(string key)
        {
            return resManager.GetString(key);
        }
    }
}