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
        /// Writes an object to a JSON file, replacing its contents.
        /// </summary>
        /// <typeparam name="T">Type of object to write.</typeparam>
        /// <param name="path">Path of JSON file.</param>
        /// <param name="obj">Object to write.</param>
        public void WriteLog<T> (string path,T obj)
        {
            jsonHelperClassBasicsForLogger.CreateJson(path, obj);
        }


        /// <summary>
        /// Writes a list of objects to a JSON file, replacing its contents.
        /// </summary>
        /// <typeparam name="T">Type of objects in the list.</typeparam>
        /// <param name="path">Path to JSON file.</param>
        /// <param name="obj">List of objects to write.</param>
        public void WriteLogList<T>(string path, List<T> obj)
        {
            jsonHelperClassBasicsForLogger.CreateJsonList(path, obj);
        }

        /// <summary>
        /// Reads a list of objects from a JSON file.
        /// Returns an empty list if the file does not exist or is empty.
        /// </summary>
        /// <typeparam name="T">Type of objects to read.</typeparam>
        /// <param name="path">Path of JSON file to read.</param>
        /// <returns>List of objects read from JSON file.</returns>
        public List<T> ReadLogStatus<T>(string path)
        {
            List<T> ListObject = jsonHelperClassBasicsForLogger.ReadJsonList<T>(path);
            return ListObject;
        }
    }
}
