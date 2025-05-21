using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private readonly string _pathToLog;
        private static List<StateObject> _stateObjList = new List<StateObject>();

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="State"/>.
        /// Lit les objets d'état existants depuis le fichier JSON.
        /// </summary>
        public State() 
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);
            this._pathToLog = Path.Combine(binPath,"logState.json");
            JsonHelperClassLoggerStatus jsonList = JsonHelperFactory.CreateLoggerStatus();
            _stateObjList = jsonList.ReadLogStatus<StateObject>(this._pathToLog);
            JsonHelperClassJsonReadMultipleObj jsonHelperClassJsonReadMultipleObj = new JsonHelperClassJsonReadMultipleObj();
            List<JobObj> _jobList = jsonHelperClassJsonReadMultipleObj.ReadMultipleObj<JobObj>(Path.Combine(binPath, "job.json"));

        }

        /// <summary>
        /// Ajoute une nouvelle entrée ou modifie un état existant avec les paramètres spécifiés.
        /// </summary>
        /// <param name="name">Nom de la tâche.</param>
        /// <param name="fileSource">Chemin source du fichier.</param>
        /// <param name="fileTarget">Chemin cible du fichier.</param>
        /// <param name="state">État actuel de la tâche.</param>
        /// <param name="totalFileToCopy">Nombre total de fichiers à copier.</param>
        /// <param name="totalFileSize">Taille totale des fichiers.</param>
        /// <param name="filesLeftToDo">Nombre de fichiers restants à copier.</param>
        /// <param name="progression">Progression en pourcentage.</param>
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
            StateObject stateObject = new StateObject(name, fileSource, fileTarget, state, totalFileToCopy, totalFileSize, filesLeftToDo, progression, this._pathToLog);
            stateObject.GetLog();
            StateModification(stateObject);
        }

        /// <summary>
        /// Écrit la liste d'état actuelle dans le fichier de log JSON.
        /// </summary>
        public override void GenerateLog()
        {
            ILoggerWriter jsonState = JsonHelperFactory.CreateLoggerStatus();
            jsonState.WriteLogList(this._pathToLog, _stateObjList);
        }

        /// <summary>
        /// Ajoute un nouvel état ou le supprime s’il existe déjà.
        /// Utilisé principalement pour activer/désactiver des tâches.
        /// </summary>
        /// <param name="jobObj">Objet représentant la tâche à ajouter ou supprimer.</param>
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
                    StateEnumeration.Inactive,
                    0,                        
                    0,                       
                    0,                        
                    0,                       
                    this._pathToLog                
                );
                _stateObjList.Add(newState);
            }
            GenerateLog();
        }

        /// <summary>
        /// Modifie l’état d’un objet existant dans la liste.
        /// </summary>
        /// <typeparam name="T">Type héritant de <see cref="StateObject"/>.</typeparam>
        /// <param name="stateObject">Nouvel état à appliquer.</param>
        public void StateModification<T>(T stateObject) where T : StateObject
        {
            var stateToModify = _stateObjList.FirstOrDefault(item => item.Name == stateObject.Name);
            if (stateToModify != null)
            {
               _stateObjList.Remove(stateToModify);
               _stateObjList.Add(stateObject);
            }
            GenerateLog();
        }

        /// <summary>
        /// Vérifie si la valeur d’état spécifiée est valide.
        /// </summary>
        /// <param name="state">Valeur de l'énumération <see cref="StateEnumeration"/>.</param>
        /// <returns>True si valide, false sinon.</returns>
        public bool VerifyState(StateEnumeration state)
        {
            return Enum.IsDefined(typeof(StateEnumeration), state);
        }
    }
}