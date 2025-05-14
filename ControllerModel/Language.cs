using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Enumerations;

namespace Languages
{
    public class Language
    {
        public EnumLanguage language
        {
            get;
            set;
        }

        public Language()
        {
            language = EnumLanguage.en;
        }
        public void changeLanguage(EnumLanguage newLang)
        {
            language = newLang;

        }
    }

}
