using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassJsonUpdate
    {
        JsonHelperClassBasics jsonHelperClassBasicsForUpdate = new();
        public void Update<T> (string PathToFileToUpdate, List<T> ListObj)
        {
            jsonHelperClassBasicsForUpdate.CreateJsonList<T>(PathToFileToUpdate, ListObj);
        }
        
    }   
}
