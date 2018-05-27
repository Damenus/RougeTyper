using System.Collections;
using System.Collections.Generic;
using Assets.WordsRepository;
using Completed;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public WordSpawner WordSpawner;
    public WordsRepository WordsRepository;
    public WordToType ActiveWord;

    public GameObject playerInBattle;
    public GameObject enemyInBattle;

    public float exitDelay = 1f;


        void Awake() {
			//Sets this to not be destroyed when reloading scene
			DontDestroyOnLoad(gameObject);
		}


    // Use this for initialization
	void Start ()
	{
        Debug.Log("Start BattleManager");
	    WordsRepository = XmlManager.Deserialize<WordsRepository>();
	    ActiveWord = new WordToType(WordsRepository.GetRandomWord(WordLevel.hard), WordSpawner.SpawnWord());
        
	}

    public void TypeLetter(char letter)
    {
        ActiveWord.TypeLetter(letter);

        if (ActiveWord.IsWordTyped())
        {
            Debug.Log("ale wyrabiscie");
            playerInBattle.GetComponent<PlayerInBattle>().attack(enemyInBattle);

            if(enemyInBattle.GetComponent<EnemyInBattle>().health <= 0) {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
				Invoke ("Exit", exitDelay);
                
            }   else {
                 ActiveWord = new WordToType(WordsRepository.GetRandomWord(WordLevel.hard), WordSpawner.SpawnWord());
            }         
            // GameManager.instance.loadMainScene();
        }
    }

    private void Exit() {
        GameManager.instance.ExitBattle();
    }
}