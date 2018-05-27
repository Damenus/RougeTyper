using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordInput : MonoBehaviour
{
    public BattleManager BattleManager;

	// Update is called once per frame
	void Update () {
	    foreach (var letter in Input.inputString)
	    {
	        BattleManager.TypeLetter(letter);
	    }
	}
}
