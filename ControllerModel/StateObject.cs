using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class StateObject
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
    
        public StateObject(string _name, string _fileSource, string _fileTarget, StateEnumeration _state, int _totalFileToCopy, double _totalFileSize, int _filesLeftToDo, float _progression, string _desPath)
        {
            this._name = _name;
            this._fileSource = _fileSource;
            this._fileTarget = _fileTarget;
            this._state = _state;
            this._totalFileToCopy = _totalFileToCopy;
            this._totalFileSize = _totalFileSize;
            this._filesLeftToDo = _filesLeftToDo;
            this._progression = _progression;
            this._desPath = _desPath;
        }

        public StateObject getLog()
        {
            return this;
        }
    }
}