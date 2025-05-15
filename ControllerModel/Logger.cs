using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using System;

namespace ControllerModel
{
    class Logger : AbstractLogger
    {
        private string pathToLog;
        public void getParam(
            string name,
            string fileSource,
            string fileTarget,
            string desPath,
            double fileSize,
            float fileTransfer,
            DateTime time)
        {
            // Charger le fichier JSON
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Ensure the Microsoft.Extensions.Configuration.Json package is installed
                .Build();

            // Lire la valeur
            pathToLog = config["pathToLog"];

            LogObject logObject = new LogObject(name, fileSource, fileTarget, desPath, fileSize, fileTransfer, time);
            GenerateLog(logObject);
        }

        public override void GenerateLog<T>(T logObject)
        {
            JSONHelper jsonHelper = new JSONHelper();
            jsonHelper.write(pathToLog, logObject);
        }
    }
}