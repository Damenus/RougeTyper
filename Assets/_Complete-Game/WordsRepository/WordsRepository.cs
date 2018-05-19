using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Assets.WordsRepository
{
    [Serializable]
    public class WordsRepository
    {
        [XmlArray]
        public List<Word> Words;
    }
}