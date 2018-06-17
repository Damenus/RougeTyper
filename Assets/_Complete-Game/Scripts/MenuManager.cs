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
		var rawHallOfFame = new List<Pair<string, int>>();
		using (StreamReader reader = new StreamReader(hofPath)) {
			string line;
			while((line = reader.ReadLine()) != null) {
				string[] words = line.Split('|');
				Pair<string, int> score = new Pair<string, int>(words[0], int.Parse(words[1]));
				rawHallOfFame.Add(score);
			}			
			var uniquePlayersScores = rawHallOfFame;
			uniquePlayersScores.Sort((a, b) => a.Score.CompareTo(b.Score));	
			
			int startIndex =  uniquePlayersScores.Count - 1;
			int endIndex = uniquePlayersScores.Count - maxSizeOfHof <= 0 ? 0 : uniquePlayersScores.Count - maxSizeOfHof;
			for (int i = startIndex ; i>=endIndex; i--) {
				hallOfFameTextList.Add(uniquePlayersScores[i].Name + ": " + uniquePlayersScores[i].Score.ToString());
			}
		}
	}

	private void showMainMenu() {
		mainPanel.SetActive(true);
		hofPanel.SetActive(false);
	}

	private class Pair<T1, T2>
	{
		public T1 Name { get; private set; }
		public T2 Score { get; private set; }
		internal Pair(T1 name, T2 score)
		{
			Name = name;
			Score = score;
		}
	}
     
     private static class Pair
     {
         public static Pair<T1, T2> New<T1, T2>(T1 name, T2 score)
         {
             var pair = new Pair<T1, T2>(name, score);
             return pair;
         }
     }

	
}
