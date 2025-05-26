using System.Collections.Concurrent;
using System.Text.Json;

namespace ControllerModel.JsonHelper
{
    public class JsonHelperClassBasics
    {
        private static readonly ConcurrentDictionary<string, object> _fileLocks = new();

        public void CreateJson<T>(string name, T obj)
        {
            var fileLock = _fileLocks.GetOrAdd(name, _ => new object()); // Allow multiple threads to access different files concurrently

            lock (fileLock)
            {
                try
                {
                    using (var fs = new FileStream(name, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan)) // allow only one thread to write to the file at a time
                    using (var writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true })) // use Utf8JsonWriter for better performance
                    {
                        /*string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(name, json);*/
                        JsonSerializer.Serialize(writer, obj);
                    }
                }
                catch (DirectoryNotFoundException ex)
                {
                    throw new Exception("Le chemin vers ce dossier n'existe pas", ex);
                }
                catch (JsonException ex)
                {
                    throw new Exception("Erreur dans la sérialisation ou la désérialisation des données", ex);
                }
                catch (IOException ex)
                {
                    throw new Exception("Erreur, le fichier n'est pas accessible pour le moment", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Une erreur inattendue est survenue :", ex);
                }
            }
        }

        public void CreateJsonList<T>(string name, List<T> obj)
        {
            var fileLock = _fileLocks.GetOrAdd(name, _ => new object()); // Allow multiple threads to access different files concurrently

            lock (fileLock)
            {
                try
                {
                    using (var fs = new FileStream(name, FileMode.Create, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan)) // allow only one thread to write to the file at a time
                    using (var writer = new Utf8JsonWriter(fs, new JsonWriterOptions { Indented = true })) // use Utf8JsonWriter for better performance
                    {
                        /*string json = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true });
                        File.WriteAllText(name, json);*/
                        JsonSerializer.Serialize(writer, obj);
                    }
                }
                catch (DirectoryNotFoundException ex)
                {
                    throw new Exception("Le chemin vers ce dossier n'existe pas", ex);
                }
                catch (JsonException ex)
                {
                    throw new Exception("Erreur dans la sérialisation ou la désérialisation des données", ex);
                }
                catch (IOException ex)
                {
                    throw new Exception("Erreur, le fichier n'est pas accessible pour le moment", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Une erreur inattendue est survenue :", ex);
                }
            }
        }

        public List<T> ReadJsonList<T>(string path)
        {
            if (!File.Exists(path))
                return new List<T>();

            var fileLock = _fileLocks.GetOrAdd(path, _ => new object()); // Allow multiple threads to access different files concurrently

            lock (fileLock)
            {
                try
                {
                    using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan); // allow multiple threads to read the file concurrently
                    var options = new JsonSerializerOptions { IncludeFields = true }; // Include fields in serialization/deserialization

                    using var jsonDoc = JsonDocument.Parse(fs); // Parse the JSON document from the file stream
                    var root = jsonDoc.RootElement; // Get the root element of the JSON document

                    if (root.ValueKind == JsonValueKind.Array)
                        return JsonSerializer.Deserialize<List<T>>(root.GetRawText(), options) ?? new List<T>(); // Deserialize the JSON array into a list of T
                    else
                        return new List<T> { JsonSerializer.Deserialize<T>(root.GetRawText(), options)! }; // Deserialize a single JSON object into a list containing that object
                }
                catch (JsonException ex)
                {
                    throw new Exception("Erreur dans la lecture JSON : " + ex.Message, ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erreur inattendue lors de la lecture : " + ex.Message, ex);
                }
            }
        }


        public T ReadJson<T>(string name)
        {
            var fileLock = _fileLocks.GetOrAdd(name, _ => new object()); // Allow multiple threads to access different files concurrently

            lock (fileLock)
            {
                try
                {
                    using var fs = new FileStream(name, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, FileOptions.SequentialScan); // allow multiple threads to read the file concurrently
                    return JsonSerializer.Deserialize<T>(fs)!; // Deserialize the JSON content into the specified type
                }
                catch (JsonException ex)
                {
                    throw new Exception($"Erreur JSON : {ex.Message}", ex);
                }
                catch (NotSupportedException ex)
                {
                    throw new Exception($"Type non supporté : {ex.Message}", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Erreur inattendue : {ex.Message}", ex);
                }
            }
        }
    }
}
