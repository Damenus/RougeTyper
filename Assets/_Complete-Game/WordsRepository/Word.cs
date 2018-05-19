using System.Xml.Serialization;

namespace Assets.WordsRepository
{
    public class Word
    {
        [XmlText]
        public string Value { get; set; }
        [XmlAttribute(AttributeName = "wordLevel")]
        public WordLevel WordLevel { get; set; }
    }
}