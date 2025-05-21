using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassJsonReadMultipleObj
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsReadMultipleObj = new();

        public List<T> ReadMultipleObj<T> (string PathToFileToUpdate)
        {
            return _jsonHelperClassBasicsReadMultipleObj.ReadJsonList<T>(PathToFileToUpdate);
        }   
    }   
}
