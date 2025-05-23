using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace ControllerModel.XmlHelper
{
    public class XmlHelperClassBasics
    {
        public void CreateXml<T>(string path, T obj)
        {
            try
            {
                using var writer = new StreamWriter(path);
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, obj);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la sérialisation XML : " + ex.Message, ex);
            }
        }
        public void CreateXmlList<T>(string path, List<T> list)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

                string? dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(fs, list);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la création du fichier XML : {ex.Message}");
            }
        }


        public T ReadXml<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                    throw new FileNotFoundException("Fichier XML introuvable.");

                using var reader = new StreamReader(path);
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new Exception("Erreur lors de la désérialisation XML : " + ex.Message, ex);
            }
        }
        public List<T> ReadXmlList<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                    return new List<T>();

                XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    return (List<T>)serializer.Deserialize(fs);
                }
            }
            catch (Exception)
            {
                return new List<T>();
            }
        }
    }
}
