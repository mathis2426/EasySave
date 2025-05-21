using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{
    /// <summary>
    /// Classe utilitaire pour la gestion basique de la sérialisation et désérialisation JSON.
    /// </summary>
    public class JsonHelperClassBasics
    {
        /// <summary>
        /// Crée un fichier JSON à partir d'un objet unique.
        /// </summary>
        /// <typeparam name="T">Type de l'objet à sérialiser.</typeparam>
        /// <param name="name">Nom (chemin) du fichier JSON à créer.</param>
        /// <param name="obj">Objet à sérialiser dans le fichier.</param>
        public void CreateJson<T>(string name, T obj) 
        {
            try
            {
                string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(name, json);

            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// Crée un fichier JSON à partir d'une liste d'objets.
        /// </summary>
        /// <typeparam name="T">Type des objets dans la liste.</typeparam>
        /// <param name="name">Nom (chemin) du fichier JSON à créer.</param>
        /// <param name="obj">Liste des objets à sérialiser dans le fichier.</param>
        public void CreateJsonList<T>(string name, List<T> obj)
        {
            try
            {
                string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(name, json);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lit un fichier JSON contenant une liste d'objets et retourne la liste désérialisée.
        /// Si le fichier est vide ou inexistant, retourne une liste vide.
        /// </summary>
        /// <typeparam name="T">Type des objets à désérialiser.</typeparam>
        /// <param name="path">Chemin du fichier JSON à lire.</param>
        /// <returns>Liste d'objets désérialisés.</returns>
        public List<T> ReadJsonList<T>(string path)
        {
            if (!File.Exists(path))
                return new List<T>();

            string json = File.ReadAllText(path);

            if (string.IsNullOrWhiteSpace(json))
                return new List<T>();
            json = json.Trim();

            var options = new JsonSerializerOptions
            {
                IncludeFields = true
            };

            if (json.StartsWith("["))
            {
                
                var list = JsonSerializer.Deserialize<List<T>>(json, options);
                return list ?? new List<T>();
            }
            else
            {
                T single = JsonSerializer.Deserialize<T>(json, options)!;
                return new List<T> { single };
            }
        }

        /// <summary>
        /// Lit un fichier JSON contenant un seul objet et retourne cet objet désérialisé.
        /// </summary>
        /// <typeparam name="T">Type de l'objet à désérialiser.</typeparam>
        /// <param name="name">Chemin du fichier JSON à lire.</param>
        /// <returns>Objet désérialisé.</returns>
        public T ReadJson<T>(string name)
        {
            string json = File.ReadAllText(name);
            try
            {
                T obj = JsonSerializer.Deserialize<T>(json);
                return obj;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }   
}
