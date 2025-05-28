using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using ControllerModel.Jobs;
using ControllerModel.JsonHelper;
using Microsoft.Extensions.Configuration;

namespace ControllerModel.Logs2
{
    public class State : AbstractLogger
    {
        private readonly string _pathToLog;
        private static List<StateObject> _stateObjList = new List<StateObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="State"/> class.
        /// Reads existing state objects from the JSON file.
        /// </summary>
        public State() 
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            _pathToLog = Path.Combine(binPath,"logState.json");
            JsonHelperClassLoggerStatus jsonList = JsonHelperFactory.CreateLoggerStatus();
            _stateObjList = jsonList.ReadLogStatus<StateObject>(_pathToLog);
            JsonHelperClassJsonReadMultipleObj jsonHelperClassJsonReadMultipleObj = new JsonHelperClassJsonReadMultipleObj();
            List<JobObj> _JobList = jsonHelperClassJsonReadMultipleObj.ReadMultipleObj<JobObj>(Path.Combine(binPath, "job.json"));
            List<StateObject> _newStateObjList = new List<StateObject>();

            if (_JobList != null && _JobList.Count > 0)
            {
                foreach (var jobObj in _JobList)
                {
                    StateObject newState = new StateObject(
                        jobObj.Name,
                        jobObj.SourcePath,
                        jobObj.TargetPath,
                        0,
                        0,
                        0,
                        0,
                        0,
                        _pathToLog
                    );

                    _newStateObjList.Add(newState);
                }
            }

            if (_newStateObjList != _stateObjList) 
            { 
                _stateObjList = _newStateObjList;

                JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = new JsonHelperClassJsonUpdate();
                jsonHelperClassJsonUpdate.Update<StateObject>(_pathToLog, _stateObjList);
            }

        }

        /// <summary>
        /// Adds a new entry or modifies an existing state with the specified parameters.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="fileSource">File source path.</param>
        /// <param name="fileTarget">File target path.</param>
        /// <param name="state">Task current state. </param>
        /// <param name="totalFileToCopy">Total number of files to copy.</param>
        /// <param name="totalFileSize">Total size of files.</param>
        /// <param name="filesLeftToDo">Number of remaining files to copy.</param>
        /// <param name="progress">Progress in percentage.</param>
        public void SendParamToLog(
            string name,
            string fileSource,
            string fileTarget,
            StateEnumeration state,
            int totalFileToCopy,
            long totalFileSize,
            int filesLeftToDo,
            float progression)
        {
            VerifyState(state);
            StateObject stateObject = new StateObject(name, fileSource, fileTarget, state, totalFileToCopy, totalFileSize, filesLeftToDo, progression, _pathToLog);
            stateObject.GetLog();
            StateModification(stateObject);
        }

        /// <summary>
        /// Writes the current status list to the JSON log file.
        /// </summary>
        public override void GenerateLog()
        {
            ILoggerWriter jsonState = JsonHelperFactory.CreateLoggerStatus();
            jsonState.WriteLogList(_pathToLog, _stateObjList);
        }

        /// <summary>
        /// Adds a new state or deletes it if it already exists.
        /// Mainly used to activate/deactivate tasks.
        /// </summary>
        /// <param name="jobObj">Object representing the task to be added or deleted.</param>
        public void StateAddDelete(JobObj jobObj)
        {
            var existing = _stateObjList.FirstOrDefault(state => state.Name == jobObj.Name);

            if (existing != null)
            {
                _stateObjList.Remove(existing);
            }
            else
            {
                StateObject newState = new StateObject(
                    jobObj.Name,
                    jobObj.SourcePath,
                    jobObj.TargetPath,
                    0,
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

        /// <summary>
        /// Modifies the state of an existing object in the list.
        /// </summary>
        /// <typeparam name="T">Type inheriting from <see cref="StateObject"/>.</typeparam>
        /// <param name="stateObject">New state to apply.</param>
        public void StateModification<T>(T stateObject) where T : StateObject
        {
            try {
                var stateToModify = _stateObjList.FirstOrDefault(item => item.Name == stateObject.Name);
                if (stateToModify != null)
                {
                   _stateObjList.Remove(stateToModify);
                   _stateObjList.Add(stateObject);
                }
            }
            catch (Exception ex){
                string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
                JsonHelperClassJsonReadMultipleObj jsonHelperClassJsonReadMultipleObj = new JsonHelperClassJsonReadMultipleObj();
                List<JobObj> _JobList = jsonHelperClassJsonReadMultipleObj.ReadMultipleObj<JobObj>(Path.Combine(binPath, "job.json"));

                List<StateObject> _newStateObjList = new List<StateObject>();

                if (_JobList != null && _JobList.Count > 0)
                {
                    foreach (var jobObj in _JobList)
                    {
                        StateObject newState = new StateObject(
                            jobObj.Name,
                            jobObj.SourcePath,
                            jobObj.TargetPath,
                            0,
                            0,
                            0,
                            0,
                            0,
                            _pathToLog
                        );

                        _newStateObjList.Add(newState);
                    }
                }

                _stateObjList = _newStateObjList;
                JsonHelperClassJsonUpdate jsonHelperClassJsonUpdate = new JsonHelperClassJsonUpdate();
                jsonHelperClassJsonUpdate.Update<StateObject>(_pathToLog, _stateObjList);

            }
            GenerateLog();
        }

        /// <summary>
        /// Checks whether the specified state value is valid.
        /// </summary>
        /// <param name="state">Enumeration value <see cref="StateEnumeration"/>.</param>
        /// <returns>True if valid, false otherwise.</returns>
        public bool VerifyState(StateEnumeration state)
        {
            return Enum.IsDefined(typeof(StateEnumeration), state);
        }
    }
}