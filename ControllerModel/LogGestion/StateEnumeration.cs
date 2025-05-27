using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel.Logs2
{
    /// <summary>
    /// Represents the different possible states of a task (Job).
    /// </summary>
    public enum StateEnumeration : byte
    {
        Inactive,
        Active,
        Error,
        In_progress,
        End,
    }
}
