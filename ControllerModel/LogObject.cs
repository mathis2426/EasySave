using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class LogObject
    {
        public string _name { get; set; }
        public string _fileSource { get; set; }
        public string _fileTarget { get; set; }
        public string _desPath { get; set; }
        public double _fileSize { get; set; }
        public long _fileTransferTime { get; set; }
        public DateTime _time { get; set; }

        [JsonConstructor]
        public LogObject(string _name, string _fileSource, string _fileTarget, string _desPath, double _fileSize, long _fileTransferTime, DateTime _time)
        {
            this._name = _name;
            this._fileSource = _fileSource;
            this._fileTarget = _fileTarget;
            this._desPath = _desPath;
            this._fileSize = _fileSize;
            this._fileTransferTime = _fileTransferTime;
            this._time = _time;
        }

        public LogObject getLog()
        {
            return this;
        }
    }
}