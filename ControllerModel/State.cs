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

        public State() 
        {
            _pathToLog = "C:\\ProjectCSharp\\StateProjectCSharp\\StateLog.json";
            JsonHelperClassLoggerStatus jsonList = JsonHelperFactory.CreateLoggerStatus();
            _stateObjList = jsonList.ReadLogStatus<StateObject>(_pathToLog);
        }
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
            stateModification(stateObject);
        }

        public override void GenerateLog()
        {
            ILoggerWriter jsonState = JsonHelperFactory.CreateLoggerStatus();
            Console.WriteLine(JsonSerializer.Serialize(_stateObjList));
            jsonState.WriteLogList(_pathToLog, _stateObjList);
        }

        public void stateAddDelete(JobObj jobObj)
        {
            var existing = _stateObjList.FirstOrDefault(state => state._name == jobObj._name);

            if (existing != null)
            {
                _stateObjList.Remove(existing);
            }
            else
            {
                StateObject newState = new StateObject(
                    jobObj._name,
                    jobObj._sourcePath,
                    jobObj._targetPath,
                    StateEnumeration.inactive,
                    0,                        
                    0,                       
                    0,                        
                    0,                       
                    _pathToLog                
                );
                _stateObjList.Add(newState);
            }
            GenerateLog();
        }

        public void stateModification<T>(T stateObject) where T : StateObject
        {
            var stateToModify = _stateObjList.FirstOrDefault(item => item._name == stateObject._name);
            if (stateToModify != null)
            {
                _stateObjList.Remove(stateToModify);
                _stateObjList.Add(stateObject);
            }
            GenerateLog();
        }

        public bool verifyState(StateEnumeration state)
        {
            return Enum.IsDefined(typeof(StateEnumeration), state);
        }
    }
}