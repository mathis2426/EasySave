using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ControllerModel.JsonHelper
{

    public class JsonHelperClassJsonReadMultipleObj
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsReadMultipleObj = new();

        /// <summary>
        /// Reads a JSON file containing a list of objects and returns the deserialized list.
        /// </summary>
        /// <typeparam name="T">Type of objects to read.</typeparam>
        /// <param name="PathToFileToUpdate">Path of JSON file to read.</param>
        /// <returns>Deserialized list of objects from JSON file.</returns>
        public List<T> ReadMultipleObj<T> (string PathToFileToUpdate)
        {
            return _jsonHelperClassBasicsReadMultipleObj.ReadJsonList<T>(PathToFileToUpdate);
        }   
    }   
}
