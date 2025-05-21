using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassLoggerStatus : ILoggerWriter
    {
        private JsonHelperClassBasics jsonHelperClassBasicsForLogger = new();
        public void WriteLog<T> (string path,T obj)
        {
            jsonHelperClassBasicsForLogger.CreateJson<T>(path, obj);
        }
        public void WriteLogList<T>(string path, List<T> obj)
        {
            jsonHelperClassBasicsForLogger.CreateJsonList<T>(path, obj);
        }

        public List<T> ReadLogStatus<T>(string path)
        {
            List<T> ListObject = jsonHelperClassBasicsForLogger.ReadJsonList<T>(path);
            return ListObject;
        }
    }
}
