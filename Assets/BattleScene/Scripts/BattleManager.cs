using System.Collections;
using System.Collections.Generic;
using Assets.WordsRepository;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{

    public static BattleManager instance = null;	

    public WordSpawner WordSpawner;
    public WordsRepository WordsRepository;
    public WordToType ActiveWord;

    public GameObject playerInBattle;
    public GameObject enemyInBattle;

	
    public float exitDelay = 1f;


        void Awake() {

            Debug.Log("Awake BattleManager");	
			//Sets this to not be destroyed when reloading scene
			//Check if instance already exists
            if (instance == null)
                //if not, set instance to this
                instance = this;

            //If instance already exists and it's not this:
            else if (instance != this)

                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);	
            
            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);
		}

    void Start() {
     
    }

    // Use this for initialization
	void OnEnable ()
	{
        Debug.Log("OnEnabled BattleManager");
        if (GameManager.instance != null && GameManager.instance.isBattle) {
            
	          WordsRepository = XmlManager.Deserialize<WordsRepository>();   
	        ActiveWord = new WordToType(WordsRepository.GetRandomWord(WordLevel.hard), WordSpawner.SpawnWord());
        }
        
        
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
                Text foodText = GameObject.FindGameObjectWithTag("FoodText").GetComponent<Text>();
                foodText.text = "Victory!";
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