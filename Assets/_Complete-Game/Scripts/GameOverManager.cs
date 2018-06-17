using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;		//Allows us to use Lists. 
using UnityEngine.UI;					//Allows us to use UI.
using System.IO;

public class GameOverManager : MonoBehaviour {

	private Text gameOverText;									//Text to display current level number.
	private Text nameInput;

	public string hofPath = "Assets/_Complete-Game/Resources/hof.txt";

	private string survivedLevels = "0";


	private bool firstType = true;
	// Use this for initialization
	void Start () {
		Debug.Log("Start()");

		gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
		nameInput = GameObject.Find("InputName").GetComponent<Text>();
		//Set levelText to display number of levels passed and game over message
		if (!GameManager.instance) {
			survivedLevels =  (GameManager.instance.level - 2).ToString();
		}		
		gameOverText.text = "You survived " + survivedLevels  + " level(s).";		
	}
	
	// Update is called once per frame
	void Update () {
		// enter players name
		foreach (var letter in Input.inputString) {
			if (firstType) {
				nameInput.text = "";
				firstType = false;
			}
			//check if 'enter' was pressed 
			if((int)(Input.GetAxisRaw("Submit")) == 1) {
					signToHoF(nameInput.text, survivedLevels, GameManager.instance.meanKPM.ToString().Split('.')[0]);
					SceneManager.LoadScene("Menu");
			} else if(Input.inputString == "\b") {
				//backspace
				if (nameInput.text.Length == 1) {
					nameInput.text = "___";	
					firstType = true;
				} else {
					nameInput.text = nameInput.text.Remove(nameInput.text.Length - 1);
				}

			} else {
				nameInput.text += letter;
			}			
		}			
	}

	void signToHoF(string name, string level, string kpm) { 
		Debug.Log("Sign to HoF.");
		Debug.Log("Name: " + name + "; levle: " + level + "; kpm: " + kpm);
        StreamWriter writer = new StreamWriter(hofPath, true);
		string line = name + "|" + level + "|" + kpm;
        writer.WriteLine(line);
        writer.Close();
	}
}
