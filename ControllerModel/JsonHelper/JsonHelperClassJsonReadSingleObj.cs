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
        /// Lit un objet depuis un fichier JSON.
        /// Si le fichier n'existe pas, crée un fichier avec une instance par défaut de <see cref="SaveConfig"/>.
        /// </summary>
        /// <typeparam name="T">Type de l'objet à lire.</typeparam>
        /// <param name="PathToFileToRead">Chemin du fichier JSON à lire.</param>
        /// <returns>Objet désérialisé de type <typeparamref name="T"/>.</returns>
        public T ReadSingleObj<T> (string PathToFileToRead)
        {
            if (!File.Exists(PathToFileToRead))
            {
                string binPath = Path.GetDirectoryName(AppContext.BaseDirectory);

                SaveConfig saveConfig = new SaveConfig(Path.Combine(binPath, "daily.json"), Path.Combine(binPath, "state.json"), "en-US", [".txt"]);
                string json = JsonSerializer.Serialize(saveConfig, new JsonSerializerOptions { WriteIndented = true });
                T save = JsonSerializer.Deserialize<T>(json);
                _jsonHelperClassBasicsReadSingleObj.CreateJson(PathToFileToRead, save);
                return save;
            }
            return _jsonHelperClassBasicsReadSingleObj.ReadJson<T>(PathToFileToRead);
        }   
    }   
}
