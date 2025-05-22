using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Text.Json;
using ControllerModel;
using ControllerModel.Logs2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;


namespace ControllerModel.JsonHelper
{

    public class JsonHelperClassLoggerDaily : ILoggerWriter
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsForLogger = new();

        /// <summary>
        /// Écrit une seule entrée de journal dans un fichier JSON.
        /// Si le fichier contient déjà des entrées, la nouvelle est ajoutée à la suite.
        /// </summary>
        /// <typeparam name="T">Type de l'objet journalisé.</typeparam>
        /// <param name="path">Chemin du fichier JSON.</param>
        /// <param name="obj">Objet à ajouter au journal.</param>
        public void WriteLog<T> (string path,T obj)
        {
            List<T> ListObject = _jsonHelperClassBasicsForLogger.ReadJsonList<T>(path);
            ListObject.Add (obj);
            _jsonHelperClassBasicsForLogger.CreateJsonList(path, ListObject);
        }

        /// <summary>
        /// Méthode non implémentée pour écrire une liste complète d'objets dans le journal.
        /// </summary>
        /// <typeparam name="T">Type des objets.</typeparam>
        /// <param name="path">Chemin du fichier JSON.</param>
        /// <param name="list">Liste d'objets à écrire.</param>
        public void WriteLogList<T>(string path, List<T> list)
        { 
        }
    }
   
}
