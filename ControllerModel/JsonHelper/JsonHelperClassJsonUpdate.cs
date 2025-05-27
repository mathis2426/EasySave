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
        /// Updates a JSON file with a list of objects.
        /// If the file exists, its contents are replaced.
        /// </summary>
        /// <typeparam name="T">Type of objects in the list.</typeparam>
        /// <param name="PathToFileToUpdate">Path of the JSON file to update.</param>
        /// <param name="ListObj">List of objects to write to the file.</param>
        public void Update<T> (string PathToFileToUpdate, List<T> ListObj)
        {
            _jsonHelperClassBasicsForUpdate.CreateJsonList(PathToFileToUpdate, ListObj);
        }

        /// <summary>
        /// Updates a JSON file with a single object.
        /// If the file exists, its contents are replaced.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="PathToFileToUpdate">Path of JSON file to update.</param>
        /// <param name="Obj">Object to write to file.</param>
        public void UpdateSingleObj<T>(string PathToFileToUpdate, T Obj)
        {
            _jsonHelperClassBasicsForUpdate.CreateJson(PathToFileToUpdate, Obj);
        }

    }   
}
