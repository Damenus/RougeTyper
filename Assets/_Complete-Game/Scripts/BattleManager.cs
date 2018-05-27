using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

	public WordManager wordManager;

	// Use this for initialization
	void Start () {
		Debug.Log("Starting the battle!");
		wordManager = GetComponent<WordManager>();	
		Debug.Log("Started Word Component");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
