using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControllerModel.Logs2;

namespace ControllerModel.XmlHelper
{
    public class XmlHelperClassLoggerDaily : ILoggerWriter
    {
        private readonly XmlHelperClassBasics _xmlHelper = new();

        public void WriteLog<T>(string path, T obj)
        {
            var list = _xmlHelper.ReadXmlList<T>(path);
            list.Add(obj);
            _xmlHelper.CreateXmlList(path, list);
        }

        public void WriteLogList<T>(string path, List<T> list)
        {
            _xmlHelper.CreateXmlList(path, list);
        }
    }
}
