using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel.Logs2
{
    /// <summary>
    /// Représente les différents états possibles d'une tâche (Job).
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
