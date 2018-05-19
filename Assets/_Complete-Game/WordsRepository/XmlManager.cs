using System.IO;
using System.Xml.Serialization;

namespace Assets.WordsRepository
{
    public static class XmlManager
    {
        public static void Serialize<T>(T obj)
        {
            using (var filestream = new FileStream("repo.xml", FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(filestream, obj);
            }
        }

        public static T Deserialize<T>()
        {
            using (var filestream = new FileStream("repo.xml", FileMode.Open))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(filestream);
            }
        }
    }
}