using System.Reflection;
using System.Text.Json;
using ControllerModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{

    public class JsonHelperClassJsonReadSingleObj
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsReadSingleObj = new();

        public T ReadSingleObj<T> (string PathToFileToRead)
        {
            if (!File.Exists(PathToFileToRead))
            {
                string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

                SaveConfig saveConfig = new SaveConfig(Path.Combine(binPath, "daily.json"), Path.Combine(binPath, "state.json"), "en-US");
                string json = JsonSerializer.Serialize(saveConfig, new JsonSerializerOptions { WriteIndented = true });
                T save = JsonSerializer.Deserialize<T>(json);
                _jsonHelperClassBasicsReadSingleObj.CreateJson<T>(PathToFileToRead, save);
                return save;
            }
            return _jsonHelperClassBasicsReadSingleObj.ReadJson<T>(PathToFileToRead);
        }   
    }   
}
