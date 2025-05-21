using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ControllerModel.JsonHelper
{

    public class JsonHelperClassJsonUpdate
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsForUpdate = new();

        /// <summary>
        /// Met à jour un fichier JSON avec une liste d'objets.
        /// Si le fichier existe, son contenu est remplacé.
        /// </summary>
        /// <typeparam name="T">Type des objets de la liste.</typeparam>
        /// <param name="PathToFileToUpdate">Chemin du fichier JSON à mettre à jour.</param>
        /// <param name="ListObj">Liste d'objets à écrire dans le fichier.</param>
        public void Update<T> (string PathToFileToUpdate, List<T> ListObj)
        {
            _jsonHelperClassBasicsForUpdate.CreateJsonList(PathToFileToUpdate, ListObj);
        }

        /// <summary>
        /// Met à jour un fichier JSON avec un seul objet.
        /// Si le fichier existe, son contenu est remplacé.
        /// </summary>
        /// <typeparam name="T">Type de l'objet.</typeparam>
        /// <param name="PathToFileToUpdate">Chemin du fichier JSON à mettre à jour.</param>
        /// <param name="Obj">Objet à écrire dans le fichier.</param>
        public void UpdateSingleObj<T>(string PathToFileToUpdate, T Obj)
        {
            _jsonHelperClassBasicsForUpdate.CreateJson(PathToFileToUpdate, Obj);
        }

    }   
}
