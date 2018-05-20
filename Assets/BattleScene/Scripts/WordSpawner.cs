using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordSpawner : MonoBehaviour
{

    public GameObject WordPrefab;
    public Transform WordCanvas;

    public WordDisplay SpawnWord()
    {
        GameObject wordObject = Instantiate(WordPrefab, WordCanvas);

        return wordObject.GetComponent<WordDisplay>();
    }
}
