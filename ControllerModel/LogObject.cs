using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    class LogObject
    {
        private string _name { get; set; }
        private string _fileSource { get; set; }
        private string _fileTarget { get; set; }
        private string _desPath { get; set; }
        private double _fileSize { get; set; }
        private float _fileTransferTime { get; set; }
        private DateTime _time { get; set; }

        public LogObject(string name, string fileSource, string fileTarget, string desPath, double fileSize, float fileTransfer, DateTime time)
        {
            _name = name;
            _fileSource = fileSource;
            _fileTarget = fileTarget;
            _desPath = desPath;
            _fileSize = fileSize;
            _fileTransferTime = fileTransfer;
            _time = time;
        }

        public LogObject getLog()
        {
            return this;
        }
    }
}