using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Dal;

static class Tools
{
    const string s_dir = @"..\xml\";
    static Tools()
    {
        if (!Directory.Exists(s_dir))
            Directory.CreateDirectory(s_dir);
    }

    public static void SaveListToXMLSerializer<T>(List<T?> list, string entity) where T : struct
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            using FileStream file = new(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            XmlSerializer xmlSerializer = new(typeof(List<T?>));
            xmlSerializer.Serialize(file, list);
        }
        catch (Exception)
        {
            throw new Exception("fail to create zml file");
        }
    }


    public static List<T?> LoadListFromXMLSerializer<T>(string entity) where T : struct
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            if (!File.Exists(filePath))
                return new();
            using FileStream file = new(filePath, FileMode.Open);
            XmlSerializer xmlSerializer = new(typeof(List<T?>));
            return xmlSerializer.Deserialize(file) as List<T?> ?? new();
        }
        catch (Exception)
        {
            throw new Exception("fail to load xml file");
        }
    }
}
