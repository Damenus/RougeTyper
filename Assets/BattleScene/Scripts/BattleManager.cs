using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.WordsRepository;
using Random = UnityEngine.Random;
using System.Collections;
using UnityEngine.UI;
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance = null;	
    public WordSpawner WordSpawner;
    public WordsRepository WordsRepository;
    public WordToType ActiveWord;

    public GameObject playerInBattle;
    public GameObject enemy;

    public Transform enemyPosition;

    private enum EnemyType {Wolf, Zombie};

    public Text playerHealth;
    public Text enemyHealth;

    private int maxEnemyHealth;
	
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

    // Use this for initialization when manager is enabled with "setActive"
	void OnEnable ()
	{
        Debug.Log("OnEnabled BattleManager");
        
        // Prevent to init on very first start of the game
        if (GameManager.instance != null && GameManager.instance.isBattle) {
	        WordsRepository = XmlManager.Deserialize<WordsRepository>();
            WordLevel wordLevel = EmotionMenager.GetInstance().LevelDifficulty();
            ActiveWord = new WordToType(WordsRepository.GetRandomWord(WordLevel.hard), WordSpawner.SpawnWord());

            // get random enemy from prefabs
            Debug.Log("Init enemy:");
            EnemyType enemyType = getEnemyType();
            Debug.Log(enemyType);
            if(enemyType.Equals(EnemyType.Wolf)) {
                Debug.Log("Loading Wolf.");
                enemy = Instantiate(Resources.Load("Prefabs/WolfBattle", typeof(GameObject)), enemyPosition.position,  Quaternion.identity) as GameObject;
            } else if (enemyType.Equals(EnemyType.Zombie)) {
                  Debug.Log("Loading Zombie.");
                enemy = Instantiate(Resources.Load("Prefabs/Zombie1Battle",typeof(GameObject)), enemyPosition.position,  Quaternion.identity) as GameObject;
            }
            Debug.Log(enemy);
		    // enemy.transform.parent = enemyPosition;
            //set up player&enemy health text
            Debug.Log("Player health: ");
            Debug.Log(playerInBattle.GetComponent<PlayerInBattle>().health);
            Debug.Log("Enemy health: ");
            Debug.Log(enemy.GetComponent<EnemyInBattle>().health);
            maxEnemyHealth = enemy.GetComponent<EnemyInBattle>().health;
            playerHealth.text = playerInBattle.GetComponent<PlayerInBattle>().health + "/" + Player.maxHealth;
            enemyHealth.text = enemy.GetComponent<EnemyInBattle>().health + "/" + enemy.GetComponent<EnemyInBattle>().health;
        }
	}

    public void UpdatePlayerHealth()
    {
        playerHealth.text = playerInBattle.GetComponent<PlayerInBattle>().health + "/" + Player.maxHealth;
    }

    public void HitPlayer(int damage)
    {
        playerInBattle.GetComponent<PlayerInBattle>().GetDamage(damage);
    }

    public void TypeLetter(char letter)
    {
        ActiveWord.TypeLetter(letter);

        if (ActiveWord.IsWordTyped())
        {
            Debug.Log("Player attack");

            playerInBattle.GetComponent<PlayerInBattle>().attack(enemy);

            enemyHealth.text = enemy.GetComponent<EnemyInBattle>().health + "/" + maxEnemyHealth;

            if(enemy.GetComponent<EnemyInBattle>().health <= 0) {
                //Invoke the Restart function to start the next level with a delay of restartLevelDelay (default 1 second).
                Text foodText = GameObject.FindGameObjectWithTag("FoodText").GetComponent<Text>();
                foodText.text = "Victory!";                
				Invoke ("Exit", exitDelay);
                
            }   else {
                WordLevel wordLevel = EmotionMenager.GetInstance().LevelDifficulty();
                ActiveWord = new WordToType(WordsRepository.GetRandomWord(wordLevel), WordSpawner.SpawnWord());
            }         
            // GameManager.instance.loadMainScene();
        }
    }

    private void Exit() {
        enemy.SetActive(false);
        GameManager.instance.SetPlayersHealth(playerInBattle.GetComponent<PlayerInBattle>().health);
        GameManager.instance.ExitBattle();
    }

    private EnemyType getEnemyType() {
        Array enemyValues = Enum.GetValues(typeof(EnemyType));
		return (EnemyType)enemyValues.GetValue(Random.Range(0, enemyValues.Length));
    }
}