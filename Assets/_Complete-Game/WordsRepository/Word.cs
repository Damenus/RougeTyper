using System.Xml.Serialization;

namespace Assets.WordsRepository
{
    public class Word
    {
        [XmlText]
        public string Value { get; set; }
        [XmlAttribute(AttributeName = "wordlevel")]
        public WordLevel WordLevel { get; set; }
    }
}