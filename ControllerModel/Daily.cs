using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using System;
using LibrairieJsonHelper;
using System.Diagnostics;
using System.Text.Json;
using System.Runtime.InteropServices.Marshalling;

namespace ControllerModel
{
    public class Daily : AbstractLogger
    {
        public string _pathToLog;
        LogObject _logObject;
        public Daily()
        {
            _pathToLog = "C:/ProjectCSharp/LogProjectCSharp/LogDaily.json";  
        }
        public void sendParamToLog(
            string name,
            string fileSource,
            string fileTarget,
            long fileSize,
            long fileTransferTime,
            DateTime time)
        {
            // Charger le fichier JSON
            /*var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Ensure the Microsoft.Extensions.Configuration.Json package is installed
                .Build();

            // Lire la valeur
            pathToLog = config["pathToLog"];
*/
            LogObject _logObject = new LogObject(name, fileSource, fileTarget, _pathToLog, fileSize, fileTransferTime, time);
            _logObject.getLog();
            GenerateLog();
        }

        public override void GenerateLog()
        {
            ILoggerWriter jsonLog = JsonHelperFactory.CreateLoggerDaily();
            Console.WriteLine(JsonSerializer.Serialize(_logObject));
            jsonLog.WriteLog(_pathToLog, _logObject);
        }
    }
}