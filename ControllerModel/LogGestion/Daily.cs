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
    public class Daily : AbstractLogger
    {
        private readonly string _pathToLog;
        private LogObject _logObject;

        /// <summary>
        /// Initializes a new instance of the Daily class,
        /// and defines the path to the daily log file.
        /// </summary>
        public Daily()
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

            _pathToLog = Path.Combine(binPath,"logDaily.json");
        }

        /// <summary>
        /// Prepares a log object with the parameters supplied,
        /// updates the internal log object and generates the log.
        /// </summary>
        /// <param name="name">Name of the job or task.</param>
        /// <param name="fileSource">Source path of the backed-up file.</param>
        /// <param name="fileTarget">Target path of the backed-up file. </param>
        /// <param name="fileSize">File size in bytes.</param>
        /// <param name="fileTransferTime">File transfer time in milliseconds.</param>
        /// <param name="time">Operation timestamp.</param>
        /// <param name="encryptionTimes">File encryption time.</param>
        public void sendParamToLog(
            string name,
            string fileSource,
            string fileTarget,
            long fileSize,
            long fileTransferTime,
            DateTime time,
            Dictionary<string, long> encryptionTimes
            )
        {
            LogObject _logObject = new LogObject(name, fileSource, fileTarget, _pathToLog, fileSize, fileTransferTime, time, encryptionTimes);
            this._logObject = _logObject.getLog();
            GenerateLog();
        }

        /// <summary>
        /// Implementation of the GenerateLog abstract method.
        /// Uses a JSON logger to write log data to the defined file.
        /// </summary>
        public override void GenerateLog()
        {
            ILoggerWriter jsonLog = JsonHelperFactory.CreateLoggerDaily();
            jsonLog.WriteLog(_pathToLog, _logObject);
        }
    }
}