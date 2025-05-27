using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ControllerModel.Logs2
{
    public class LogObject
    {
        public string Name { get; set; }
        public string FileSource { get; set; }
        public string FileTarget { get; set; }
        public string DesPath { get; set; }
        public double FileSize { get; set; }
        public long FileTransferTime { get; set; }
        public DateTime Time { get; set; }
        public Dictionary<string, long> EncryptionTimes { get; set; } = new Dictionary<string, long>();


        /// <summary>
        /// Constructor used for JSON deserialization.
        /// </summary>
        /// <param name="Name">Job name.</param>
        /// <param name="FileSource">Source path.</param>
        /// <param name="FileTarget">Destination path.</param>
        /// <param name="DesPath">Log file path. </param>
        /// <param name="FileSize">Total size of files transferred.</param>
        /// <param name="FileTransferTime">Transfer time in ms.</param>
        /// <param name="Time">Transfer date and time.</param>
        /// <param name="EncryptionTimes">Dictionary of encryption times.</param>"
        [JsonConstructor]
        public LogObject(string Name, string FileSource, string FileTarget, string DesPath, double FileSize, long FileTransferTime, DateTime Time, Dictionary<string, long> encryptionTimes)
        {
            this.Name = Name;
            this.FileSource = FileSource;
            this.FileTarget = FileTarget;
            this.DesPath = DesPath;
            this.FileSize = FileSize;
            this.FileTransferTime = FileTransferTime;
            this.Time = Time;
            this.EncryptionTimes = encryptionTimes;
        }

        /// <summary>
        /// Returns the current object.
        /// </summary>
        /// <returns>Current instance of <see cref="LogObject"/>.</returns>
        public LogObject getLog()
        {
            return this;
        }
    }
}