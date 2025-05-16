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

namespace ControllerModel
{
    public class Daily : AbstractLogger
    {
        public string _pathToLog;
        public void sendParamToLog(
            string name,
            string fileSource,
            string fileTarget,
            long fileSize,
            Stopwatch fileTransferTime,
            DateTime time)
        {
            // Charger le fichier JSON
            /*var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Ensure the Microsoft.Extensions.Configuration.Json package is installed
                .Build();

            // Lire la valeur
            pathToLog = config["pathToLog"];
*/
            LogObject logObject = new LogObject(name, fileSource, fileTarget, _pathToLog, fileSize, fileTransferTime, time);
            logObject.getLog();
            GenerateLog(logObject);
        }

        public override void GenerateLog<T>(T logObject)
        {
            _pathToLog = "C:/ProjectCSharp/LogProjectCSharp/LogDaily.json";
            ILoggerWriter jsonLog = JsonHelperFactory.CreateLoggerDaily();
            Console.WriteLine(JsonSerializer.Serialize(logObject));
            jsonLog.WriteLog(_pathToLog, logObject);
        }
    }
}