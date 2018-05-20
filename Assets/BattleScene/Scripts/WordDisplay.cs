using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordDisplay : MonoBehaviour
{

    public Text Text;

    public void SetWord(string word)
    {
        Text.text = word;
    }

    public void RemoveLetter()
    {
        Text.text = Text.text.Remove(0, 1);
        Text.color = Color.red;
    }

    public void RemoveWord()
    {
        Destroy(gameObject);
    }
}
