using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerModel.XmlHelper
{
    public class XmlHelperFactory
    {
        public static XmlHelperClassBasics CreateBasic() => new();
        public static XmlHelperReadList CreateReaderList() => new();
        public static XmlHelperClassLoggerDaily CreateLoggerDaily() => new();
    }
}
