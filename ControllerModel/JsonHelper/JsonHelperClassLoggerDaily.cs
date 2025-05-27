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
        /// Writes a single log entry to a JSON file.
        /// If the file already contains entries, the new one is added next.
        /// </summary>
        /// <typeparam name="T">Type of the object being logged.</typeparam>
        /// <param name="path">Path of the JSON file.</param>
        /// <param name="obj">Object to be added to the log.</param>
        public void WriteLog<T> (string path,T obj)
        {
            List<T> ListObject = _jsonHelperClassBasicsForLogger.ReadJsonList<T>(path);
            ListObject.Add (obj);
            _jsonHelperClassBasicsForLogger.CreateJsonList(path, ListObject);
        }

        /// <summary>
        /// Unimplemented method for writing a complete list of objects to the log.
        /// </summary>
        /// <typeparam name="T">Type of objects.</typeparam>
        /// <param name="path">Path of JSON file.</param>
        /// <param name="list">List of objects to write.</param>
        public void WriteLogList<T>(string path, List<T> list)
        { 
        }
    }
   
}
