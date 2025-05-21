using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassJsonUpdate
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsForUpdate = new();
        public void Update<T> (string PathToFileToUpdate, List<T> ListObj)
        {
            _jsonHelperClassBasicsForUpdate.CreateJsonList<T>(PathToFileToUpdate, ListObj);
        }

        public void UpdateSingleObj<T>(string PathToFileToUpdate, T Obj)
        {
            _jsonHelperClassBasicsForUpdate.CreateJson<T>(PathToFileToUpdate, Obj);
        }

    }   
}
