using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassJsonReadMultipleObj
    {
        JsonHelperClassBasics jsonHelperClassBasicsReadMultipleObj = new();

        public List<T> ReadMultipleObj<T> (string PathToFileToUpdate)
        {
            return jsonHelperClassBasicsReadMultipleObj.ReadJsonList<T>(PathToFileToUpdate);
        }   
    }   
}
