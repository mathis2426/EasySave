using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace LibrairieJsonHelper
{
    public class JsonHelperClassBasics
    {
        public void CreateJson<T>(string name, T obj) 
        {
            try
            {
                string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(name, json);

            }
            catch (DirectoryNotFoundException ex)
            {
                // Le dossier spécifié dans le chemin n'existe pas ou n'a pas pu être trouvé
                throw new Exception("Le chemin vers ce dossier n'existe pas", ex);
            }
            catch (JsonException ex)
            {
                // Le contenu JSON est invalide ou ne correspond pas à la structure attendue
                throw new Exception("Erreur dans la sérialisation ou la désérialisation des données", ex);
            }
            catch (IOException ex)
            {
                // Le fichier est inaccessible : peut être utilisé par un autre processus, verrouillé, ou problème d'I/O (disque plein)
                throw new Exception("Erreur, le fichier n'est pas accessible pour le moment", ex);
            }
            catch (Exception ex)
            {
                // Toute autre erreur
                throw new Exception("Une erreur innatendue est survenue :", ex);
            }
        }

        public void CreateJsonList<T>(string name, List<T> obj)
        {
            try
            {
                string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });

                File.WriteAllText(name, json);

            }
            catch (DirectoryNotFoundException ex)
            {
                // Le dossier spécifié dans le chemin n'existe pas ou n'a pas pu être trouvé
                throw new Exception("Le chemin vers ce dossier n'existe pas", ex);
            }
            catch (JsonException ex)
            {
                // Le contenu JSON est invalide ou ne correspond pas à la structure attendue
                throw new Exception("Erreur dans la sérialisation ou la désérialisation des données", ex);
            }
            catch (IOException ex)
            {
                // Le fichier est inaccessible : peut être utilisé par un autre processus, verrouillé, ou problème d'I/O (disque plein)
                throw new Exception("Erreur, le fichier n'est pas accessible pour le moment", ex);
            }
            catch (Exception ex)
            {
                // Toute autre erreur
                throw new Exception("Une erreur innatendue est survenue :", ex);
            }
        }

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
        public T ReadJson<T>(string name)
        {
            string json = File.ReadAllText(name);
            try
            {
                T obj = JsonSerializer.Deserialize<T>(json);
                return obj;
            }
            catch (JsonException ex)
            {
                // Format JSON incorrect
                throw new Exception($"Erreur JSON : {ex.Message}");
            }
            catch (NotSupportedException ex)
            {
                // Type T non supporté
                throw new Exception($"Type non supporté : {ex.Message}");
            }
            catch (Exception ex)
            {
                // Toute autre erreur
                throw new Exception($"Erreur inattendue : {ex.Message}");
            }
        }

    }   
}
