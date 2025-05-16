using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassLoggerDaily : ILoggerWriter
    {
        JsonHelperClassBasics jsonHelperClassBasicsForLogger = new();
        public void WriteLog<T> (string path,T obj)
        {
            List<T> ListObject = jsonHelperClassBasicsForLogger.ReadJsonList<T>(path);
            ListObject.Add (obj);
            jsonHelperClassBasicsForLogger.CreateJsonList<T>(path, ListObject);
        }   
    }
}
