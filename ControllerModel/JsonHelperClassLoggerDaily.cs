using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using ControllerModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassLoggerDaily : ILoggerWriter
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsForLogger = new();
        public void WriteLog<T> (string path,T obj)
        {
            List<T> ListObject = _jsonHelperClassBasicsForLogger.ReadJsonList<T>(path);
            ListObject.Add (obj);
            _jsonHelperClassBasicsForLogger.CreateJsonList<T>(path, ListObject);
        }

        public void WriteLogList<T>(string path, List<T> list)
        { 
        }
    }
   
}
