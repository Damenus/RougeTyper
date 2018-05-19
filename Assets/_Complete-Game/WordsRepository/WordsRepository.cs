using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Assets.WordsRepository
{
    [Serializable]
    public class WordsRepository
    {
        [XmlArray]
        public IEnumerable<Word> Words;
    }
}