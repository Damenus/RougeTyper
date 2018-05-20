using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInput : MonoBehaviour
{
    public WordManager WordManager;

	// Update is called once per frame
	void Update () {
	    foreach (var letter in Input.inputString)
	    {
	        WordManager.TypeLetter(letter);
	    }
	}
}
