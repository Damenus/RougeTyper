using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Xml.Serialization;
using UnityEngine;
using Random = System.Random;

namespace Assets.WordsRepository
{
    [Serializable]
    [XmlRoot("wordsRepository")]
    public class WordsRepository
    {
        [XmlArray("words")]
        [XmlArrayItem("word")]
        public List<Word> Words;

        public string GetRandomWord(WordLevel level = WordLevel.easy)
        {
            var words = Words.Where(word => word.WordLevel == level).ToList();
            int index = UnityEngine.Random.Range(0, words.Count);

            return words[index].Value;
        }
    }
}