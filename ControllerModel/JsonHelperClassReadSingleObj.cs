using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassJsonReadSingleObj
    {
        JsonHelperClassBasics jsonHelperClassBasicsReadSingleObj = new();

        public T ReadSingleObj<T> (string PathToFileToRead)
        {
            return jsonHelperClassBasicsReadSingleObj.ReadJson<T>(PathToFileToRead);
        }   
    }   
}
