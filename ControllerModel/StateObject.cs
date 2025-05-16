using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    class StateObject
    {
        public string _name { get; set; }
        public string _fileSource { get; set; }
        public string _fileTarget { get; set; }
        public StateEnumeration _state { get; set; }
        public int _totalFileToCopy { get; set; }
        public double _totalFileSize { get; set; }
        public int _filesLeftToDo { get; set; }
        public float _progression { get; set; }
        public string _desPath { get; set; }
    
        public StateObject(string name, string fileSource, string fileTarget, StateEnumeration state, int totalFileToCopy, double totalFileSize, int filesLeftToDo, float progression, string desPath)
        {
            _name = name;
            _fileSource = fileSource;
            _fileTarget = fileTarget;
            _state = state;
            _totalFileToCopy = totalFileToCopy;
            _totalFileSize = totalFileSize;
            _filesLeftToDo = filesLeftToDo;
            _progression = progression;
            _desPath = desPath;
        }

        public StateObject getLog()
        {
            return this;
        }
    }
}