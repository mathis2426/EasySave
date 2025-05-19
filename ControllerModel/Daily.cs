using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Configuration;
using System;
using LibrairieJsonHelper;
using System.Diagnostics;
using System.Text.Json;
using System.Runtime.InteropServices.Marshalling;
using System.Reflection;

namespace ControllerModel
{
    public class Daily : AbstractLogger
    {
        public string _pathToLog;
        LogObject _logObject;
        public Daily()
        {
            string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

            this._pathToLog = Path.Combine(binPath,"logDaily.json");
        }
        public void sendParamToLog(
            string name,
            string fileSource,
            string fileTarget,
            long fileSize,
            long fileTransferTime,
            DateTime time)
        {
            LogObject _logObject = new LogObject(name, fileSource, fileTarget, _pathToLog, fileSize, fileTransferTime, time);
            this._logObject = _logObject.getLog();
            GenerateLog();
        }

        public override void GenerateLog()
        {
            ILoggerWriter jsonLog = JsonHelperFactory.CreateLoggerDaily();
            jsonLog.WriteLog(_pathToLog, this._logObject);
        }
    }
}