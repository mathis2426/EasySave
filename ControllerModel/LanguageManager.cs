using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace LanguageManagers
{
    public static class LanguageManager
    {
        public static ResourceManager resManager = new ResourceManager("View.Resources.Lang", Assembly.GetExecutingAssembly());

        public static void SetLanguage(string cultureCode)
        {
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
        }

        public static string Get(string key)
        {
            return resManager.GetString(key);
        }
    }
}
