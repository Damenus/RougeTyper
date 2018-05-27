using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour
{

    public Text Text;

    public void SetWord(string word)
    {
        Debug.Log("Set word:");
        Debug.Log(word);
        Text.text = word;
    }

    public void RemoveLetter()
    {
        // Debug.Log("RemoveLetter.");
        Text.text = Text.text.Remove(0, 1);
        Text.color = Color.red;
    }

    public void RemoveWord()
    {
        Debug.Log("RemoveWord.");
        Destroy(gameObject);
    }
}
