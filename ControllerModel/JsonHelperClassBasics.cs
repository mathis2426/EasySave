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
            catch (Exception ex) 
            {
                throw ex;
            }
        }

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
            catch (Exception e)
            {
                throw e;
            }
        }

    }   
}
