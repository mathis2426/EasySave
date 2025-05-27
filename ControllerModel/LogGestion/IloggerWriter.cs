using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Runtime.InteropServices.Marshalling;
using System.Reflection;
using ControllerModel.JsonHelper;



namespace ControllerModel.Logs2
{
    /// <summary>
    /// Interface defining methods for writing logs in JSON format.
    /// </summary>
    public interface ILoggerWriter
    {
        /// <summary>
        /// Writes a log object to a specified file.
        /// </summary>
        /// <typeparam name="T">Type of log object.</typeparam>
        /// <param name="path">Path to file where to write log.</param>
        /// <param name="obj">Log object to write.</param>
        void WriteLog<T>(string path, T obj);

        /// <summary>
        /// Writes a list of log objects to a specified file.
        /// </summary>
        /// <typeparam name="T">Type of log objects in the list.</typeparam>
        /// <param name="path">Path to the file where to write the logs.</param>
        /// <param name="list">List of log objects to write.</param>
        void WriteLogList<T>(string path, List<T> list);

    }
}
