using System.Text.Json;
using ControllerModel.Logs2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ControllerModel.JsonHelper
{

    public class JsonHelperClassLoggerStatus : ILoggerWriter
    {
        private JsonHelperClassBasics jsonHelperClassBasicsForLogger = new();

        /// <summary>
        /// Écrit un objet dans un fichier JSON, remplaçant son contenu.
        /// </summary>
        /// <typeparam name="T">Type de l'objet à écrire.</typeparam>
        /// <param name="path">Chemin du fichier JSON.</param>
        /// <param name="obj">Objet à écrire.</param>
        public void WriteLog<T> (string path,T obj)
        {
            jsonHelperClassBasicsForLogger.CreateJson(path, obj);
        }

        /// <summary>
        /// Écrit une liste d'objets dans un fichier JSON, remplaçant son contenu.
        /// </summary>
        /// <typeparam name="T">Type des objets dans la liste.</typeparam>
        /// <param name="path">Chemin du fichier JSON.</param>
        /// <param name="obj">Liste d'objets à écrire.</param>
        public void WriteLogList<T>(string path, List<T> obj)
        {
            jsonHelperClassBasicsForLogger.CreateJsonList(path, obj);
        }

        /// <summary>
        /// Lit une liste d'objets depuis un fichier JSON.
        /// Retourne une liste vide si le fichier n'existe pas ou est vide.
        /// </summary>
        /// <typeparam name="T">Type des objets à lire.</typeparam>
        /// <param name="path">Chemin du fichier JSON à lire.</param>
        /// <returns>Liste d'objets lue à partir du fichier JSON.</returns>
        public List<T> ReadLogStatus<T>(string path)
        {
            List<T> ListObject = jsonHelperClassBasicsForLogger.ReadJsonList<T>(path);
            return ListObject;
        }
    }
}
