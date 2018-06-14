using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	private string hofPath = "Assets/_Complete-Game/Resources/hof.txt";
	private MenuState state = MenuState.MAIN;
	
	private List<string> hallOfFameTextList = new List<string>();
	private int maxSizeOfHof = 5;
	private GameObject mainPanel;
	private GameObject hofPanel;

	private Text scoreList;

	
	enum MenuState
	{
		MAIN, HOF
	}
	void Start() {
		mainPanel = GameObject.Find("Panel");
		hofPanel = GameObject.Find("HallOfFame");
		scoreList = GameObject.Find("ScoreList").GetComponent<Text>();

		mainPanel.SetActive(true);
		hofPanel.SetActive(false);
	}
	void Update() {
		switch	(state) {
			case MenuState.HOF:
				if((int)(Input.GetAxisRaw("Submit")) == 1) {
					showMainMenu();
				}				
				break;
			default:            
              break;
		}
	}
	public void StartGame() {
		if(GameManager.instance)
		{
			GameManager.instance.isGameOver = false;
		}
		SceneManager.LoadScene("_Complete-Game");
	}	

	public void ShowHallOfFame() {
		hallOfFameTextList = new List<string>();
		loadHOF();
		string scoreListText = "";
        foreach(string score in hallOfFameTextList) {
             scoreListText = scoreListText.ToString () + score.ToString() + "\n";
        }       

		scoreList.text = scoreListText;

		state = MenuState.HOF;
		mainPanel.SetActive(false);
		hofPanel.SetActive(true);
		
	}

	public void ExitGame() {
		Application.Quit();
	}

	private void loadHOF() {
		Dictionary<string, int> rawHallOfFame = new Dictionary<string,int>();
		using (StreamReader reader = new StreamReader(hofPath)) {
			string line;
			while((line = reader.ReadLine()) != null) {
				string[] words = line.Split('|');
				rawHallOfFame[words[0]] = int.Parse(words[1]);
			}			
			var uniquePlayersScores = rawHallOfFame.ToList();
			uniquePlayersScores.Sort((a, b) => a.Value.CompareTo(b.Value));	
			
			int startIndex =  uniquePlayersScores.Count - 1;
			int endIndex = uniquePlayersScores.Count - maxSizeOfHof <= 0 ? 0 : uniquePlayersScores.Count - maxSizeOfHof;
			Debug.Log("start: " + startIndex.ToString());
			Debug.Log("end: " + endIndex.ToString());
			for (int i = startIndex ; i>=endIndex; i--) {
				hallOfFameTextList.Add(uniquePlayersScores[i].Key + ": " + uniquePlayersScores[i].Value.ToString());
			}
		}
	}

	private void showMainMenu() {
		mainPanel.SetActive(true);
		hofPanel.SetActive(false);
	}

	
}
