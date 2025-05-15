using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassLogger : ILoggerWriter
    {
        JsonHelperClassBasics jsonHelperClassBasicsForLogger = new();
        public void WriteLog<T> (string path,T obj)
        {
            jsonHelperClassBasicsForLogger.CreateJson<T>(path, obj);
        }   
    }




   
}
