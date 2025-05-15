using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LibrairieJsonHelper;
using Microsoft.Extensions.Configuration;

namespace ControllerModel
{
    class State : AbstractLogger
    {
        private string pathToLog;
        public void getParam(
            string name,
            string fileSource,
            string fileTarget,
            StateEnumeration state,
            int totalFileToCopy,
            double totalFileSize,
            int filesLeftToDo,
            float progression,
            string desPath)
        {
            verifyState(state);
            // Charger le fichier JSON
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Ensure the Microsoft.Extensions.Configuration.Json package is installed
                .Build();

            // Lire la valeur
            pathToLog = config["pathToLog"];

            StateObject stateObject = new StateObject(name, fileSource, fileTarget, state, totalFileToCopy, totalFileSize, filesLeftToDo, progression, desPath);
            GenerateLog(stateObject);
        }

        public override void GenerateLog<T>(T stateObject)
        {
            ILoggerWriter jsonState = jsonFactory.CreateLogger();
            jsonState.WriteLog(pathToLog, stateObject);
        }

        public bool verifyState(StateEnumeration state)
        {
            return Enum.IsDefined(typeof(StateEnumeration), state);
        }
    }
}