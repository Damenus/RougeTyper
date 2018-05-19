using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Assets.WordsRepository
{
    public static class XmlManager
    {
        public static void Serialize<T>(T obj)
        {
            using (var filestream = new FileStream("words3.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(filestream, obj);
            }
        }

        public static T Deserialize<T>()
        {
            using (var reader = XmlReader.Create("words.xml"))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(reader);
            }
        }
    }
}