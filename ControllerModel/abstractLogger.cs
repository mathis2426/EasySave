using LibrairieJsonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    public abstract class AbstractLogger
    {
        /// <summary>
        /// Méthode abstraite destinée à générer un log.
        /// Doit être implémentée par les classes dérivées pour définir la logique de journalisation.
        /// </summary
        public abstract void GenerateLog();
    }
}
