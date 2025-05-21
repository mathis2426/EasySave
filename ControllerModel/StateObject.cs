using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public class StateObject
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public StateEnumeration State { get; set; }
        public int TotalFileToCopy { get; set; }
        public double TotalFileSize { get; set; }
        public int FilesLeftToDo { get; set; }
        public float Progression { get; set; }
        public string DesPath { get; set; }
    
        public StateObject(string Name, string FileSource, string FileTarget, StateEnumeration State, int TotalFileToCopy, double TotalFileSize, int FilesLeftToDo, float Progression, string DesPath)
        {
            this.Name = Name;
            this.FileSource = FileSource;
            this.FileTarget = FileTarget;
            this.State = State;
            this.TotalFileToCopy = TotalFileToCopy;
            this.TotalFileSize = TotalFileSize;
            this.FilesLeftToDo = FilesLeftToDo;
            this.Progression = Progression;
            this.DesPath = DesPath;
        }

        public StateObject GetLog()
        {
            return this;
        }
    }
}