using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.WordsRepository
{
    [Serializable]
    [XmlRoot("wordsRepository")]
    public class WordsRepository
    {
        [XmlArray("words")]
        [XmlArrayItem("word")]
        public List<Word> Words;
    }
}