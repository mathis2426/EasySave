using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    class StateObject
    {
        private string _name { get; set; }
        private string _fileSource { get; set; }
        private string _fileTarget { get; set; }
        private StateEnumeration _state { get; set; }
        private int _totalFileToCopy { get; set; }
        private double _totalFileSize { get; set; }
        private int _filesLeftToDo { get; set; }
        private float _progression { get; set; }
        private string _desPath { get; set; }
    
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