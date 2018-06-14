using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour {

	public void StartGame() {
		if(GameManager.instance)
		{
			GameManager.instance.isGameOver = false;
		}
		SceneManager.LoadScene("_Complete-Game");
	}	

	public void EnterHallOfFame() {

	}

	public void ExitGame() {
		Application.Quit();
	}
}
