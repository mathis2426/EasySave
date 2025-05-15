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

                Console.WriteLine("Fichier JSON créé !");
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public List<T> ReadJsonList<T>(string name )
        {
            string json = File.ReadAllText(name);
            try
            {
                List<T> obj = JsonSerializer.Deserialize<List<T>>(json);
                return obj;
            }
            catch (Exception e) { 
                throw e;
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
