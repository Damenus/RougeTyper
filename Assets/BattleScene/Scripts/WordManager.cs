using System.Collections;
using System.Collections.Generic;
using Assets.WordsRepository;
using Completed;
using UnityEngine;

public class WordManager : MonoBehaviour
{
    public WordSpawner WordSpawner;
    public WordsRepository WordsRepository;
    public WordToType ActiveWord;

    // Use this for initialization
	void Start ()
	{
        Debug.Log("Start WordManager");
	    WordsRepository = XmlManager.Deserialize<WordsRepository>();
	    ActiveWord = new WordToType(WordsRepository.GetRandomWord(WordLevel.hard), WordSpawner.SpawnWord());
	}

    public void TypeLetter(char letter)
    {
        ActiveWord.TypeLetter(letter);

        if (ActiveWord.IsWordTyped())
        {
            Debug.Log("ale wyrabiscie");
            GameManager.instance.ExitBattle();
            // GameManager.instance.loadMainScene();
        }
    }
}