using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassJsonReadMultipleObj
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsReadMultipleObj = new();

        /// <summary>
        /// Lit un fichier JSON contenant une liste d'objets et retourne la liste désérialisée.
        /// </summary>
        /// <typeparam name="T">Type des objets à lire.</typeparam>
        /// <param name="PathToFileToUpdate">Chemin du fichier JSON à lire.</param>
        /// <returns>Liste d'objets désérialisés du fichier JSON.</returns>
        public List<T> ReadMultipleObj<T> (string PathToFileToUpdate)
        {
            return _jsonHelperClassBasicsReadMultipleObj.ReadJsonList<T>(PathToFileToUpdate);
        }   
    }   
}
