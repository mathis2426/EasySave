using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    class LogObject
    {
        public string _name { get; set; }
        public string _fileSource { get; set; }
        public string _fileTarget { get; set; }
        public string _desPath { get; set; }
        public double _fileSize { get; set; }
        public Stopwatch _fileTransferTime { get; set; }
        public DateTime _time { get; set; }

        public LogObject(string name, string fileSource, string fileTarget, string desPath, double fileSize, Stopwatch fileTransferTime, DateTime time)
        {
            _name = name;
            _fileSource = fileSource;
            _fileTarget = fileTarget;
            _desPath = desPath;
            _fileSize = fileSize;
            _fileTransferTime = fileTransferTime;
            _time = time;
        }

        public LogObject getLog()
        {
            return this;
        }
    }
}