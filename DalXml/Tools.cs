using System.Xml.Linq;
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

    public static int getNextID(string element)
    {
        string filePath = $"{s_dir}config.xml";

        if (!File.Exists(filePath))
            throw new Exception($"fail to load xml file: {filePath}");

        XElement rootElem = XElement.Load(filePath);
        XElement elem = rootElem.Element(element) ?? throw new Exception($"could not find {element} element on {filePath}");

        int id;

        if (!int.TryParse(elem.Value, out id))
            throw new Exception($"{element} value was invalid");

        elem.Value = (id + 1).ToString();

        try{rootElem.Save(filePath);}
        catch (Exception ex){throw new Exception($"fail to save xml file: {filePath}", ex);}

        return id;
    }


    public static int? ToIntNullable(this XElement element, string name) =>
       int.TryParse((string?)element.Element(name), out var result) ? (int?)result : null;


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

    public static XElement LoadListFromXMLElement(string entity)
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            if (File.Exists(filePath))
                return XElement.Load(filePath);
            XElement rootelement = new(entity);
            rootelement.Save(filePath);
            return rootelement;
        }
        catch { throw new Exception("fail load file"); }
    }

    public static void SaveListFromXMLElement(XElement rootElem, string entity)
    {
        string filePath = $"{s_dir + entity}.xml";
        try
        {
            rootElem.Save(filePath);
        }
        catch { throw new Exception("fail to creat this file"); }
    }
}
