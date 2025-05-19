using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using LibrairieJsonHelper;
using Microsoft.Extensions.Configuration;

namespace ControllerModel
{
    public class State : AbstractLogger
    {
        public string _pathToLog;
        public List<StateObject> _stateObjList = new List<StateObject>();
        public void sendParamToLog(
            string name,
            string fileSource,
            string fileTarget,
            StateEnumeration state,
            int totalFileToCopy,
            long totalFileSize,
            int filesLeftToDo,
            float progression)
        {
            verifyState(state);
            // Charger le fichier JSON
            /*var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json") // Ensure the Microsoft.Extensions.Configuration.Json package is installed
                .Build();

            // Lire la valeur
            pathToLog = config["pathToLog"];*/

            StateObject stateObject = new StateObject(name, fileSource, fileTarget, state, totalFileToCopy, totalFileSize, filesLeftToDo, progression, _pathToLog);
            stateObject.getLog();
            GenerateLog(stateObject);
        }

        public override void GenerateLog<T>(T stateObject)
        {
            _pathToLog = "C:\\ProjectCSharp\\StateProjectCSharp\\StateLog.json";
            ILoggerWriter jsonState = JsonHelperFactory.CreateLoggerStatus();
            Console.WriteLine(JsonSerializer.Serialize(stateObject));
            jsonState.WriteLog(_pathToLog, stateObject);
        }

        public void stateAddDelete(JobObj jobObj)
        {
            // TODO : A implementer

            // recuperer la liste des objets (liste des states)
            // Prendre l'objet envoyer par bounty et ajouter à la liste
            // puis renvoyer la liste a theBlackShade
        }

        public void stateModification()
        {
            // TODO : A implementer

            // recuperer la liste des objets (liste des states)
            // Prendre l'objet envoyer par bounty et modifier la liste en fonction
            // puis renvoyer la liste a theBlackShade
        }

        public bool verifyState(StateEnumeration state)
        {
            return Enum.IsDefined(typeof(StateEnumeration), state);
        }
    }
}