
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel.Logs2
{
    public abstract class AbstractLogger
    {
        /// <summary>
        /// Abstract method intended to generate a log.
        /// Must be implemented by derived classes to define the logging logic.
        /// </summary
        public abstract void GenerateLog();
    }
}
