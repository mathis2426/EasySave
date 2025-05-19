using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassJsonUpdate
    {
        JsonHelperClassBasics jsonHelperClassBasicsForUpdate = new();
        public void Update<T> (string PathToFileToUpdate, T obj)
        {
            List<T> ListObject = jsonHelperClassBasicsForUpdate.ReadJsonList<T>(PathToFileToUpdate);
            ListObject.Add(obj);
            jsonHelperClassBasicsForUpdate.CreateJsonList<T>(PathToFileToUpdate, ListObject);
        }   
    }   
}
