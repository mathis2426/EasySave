using LibrairieJsonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel
{
    abstract class AbstractLogger
    {
        protected JsonHelperFactory jsonFactory = new JsonHelperFactory();
        public abstract void GenerateLog<T>(T obj);
    }
}
