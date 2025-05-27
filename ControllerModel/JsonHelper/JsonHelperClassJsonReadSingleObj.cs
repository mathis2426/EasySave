using System.Reflection;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace ControllerModel.JsonHelper
{

    public class JsonHelperClassJsonReadSingleObj
    {
        private readonly JsonHelperClassBasics _jsonHelperClassBasicsReadSingleObj = new();

        /// <summary>
        /// Reads an object from a JSON file.
        /// If the file does not exist, creates a file with a default instance of <see cref="SaveConfig"/>.
        /// </summary>
        /// <typeparam name="T">Type of object to read. </typeparam>
        /// <param name="PathToFileToRead">Path to JSON file to read.</param>
        /// <returns>Deserialized object of type <typeparamref name="T"/>.</returns>
        public T ReadSingleObj<T> (string PathToFileToRead)
        {
            if (!File.Exists(PathToFileToRead))
            {
                string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

                SaveConfig saveConfig = new SaveConfig(Path.Combine(binPath, "daily.json"), Path.Combine(binPath, "state.json"), "en-US", new string[] { ".txt" }, new string[] { ".txt" },"");
                string json = JsonSerializer.Serialize(saveConfig, new JsonSerializerOptions { WriteIndented = true });
                T save = JsonSerializer.Deserialize<T>(json);
                _jsonHelperClassBasicsReadSingleObj.CreateJson(PathToFileToRead, save);
                return save;
            }
            return _jsonHelperClassBasicsReadSingleObj.ReadJson<T>(PathToFileToRead);
        }   
    }   
}
