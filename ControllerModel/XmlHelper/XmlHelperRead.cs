using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel.XmlHelper
{
    public class XmlHelperReadList
    {
        private readonly XmlHelperClassBasics _xmlHelper = new();

        public List<T> ReadList<T>(string path)
        {
            return _xmlHelper.ReadXmlList<T>(path);
        }
    }
}
