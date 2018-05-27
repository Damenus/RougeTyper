﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Completed
{
	using System.Collections.Generic;		//Allows us to use Lists. 
	using UnityEngine.UI;					//Allows us to use UI.
	
	public class GameManager : MonoBehaviour
	{

		public GameObject boardCamera;
		public GameObject battleCamera;
		public float levelStartDelay = 2f;						//Time to wait before starting level, in seconds.
		public float turnDelay = 0.1f;							//Delay between each Player turn.
		public int playerHealthPoints = Player.maxHealth;						//Starting value for Player food points.
		public static GameManager instance = null;				//Static instance of GameManager which allows it to be accessed by any other script.
		[HideInInspector] public bool playersTurn = true;		//Boolean to check if it's players turn, hidden in inspector but public.
		
		
		private Text levelText;									//Text to display current level number.
		public Text foodText;
		private GameObject levelImage;							//Image to block out level as levels are being set up, background for levelText.
		private BoardManager boardScript;						//Store a reference to our BoardManager which will set up the level.
		private int level = 1;									//Current level number, expressed in game as "Day 1".
		private List<Enemy> enemies;							//List of all Enemy units, used to issue them move commands.
		private bool enemiesMoving;								//Boolean to check if enemies are moving.
		private bool doingSetup = true;							//Boolean to check if we're setting up board, prevent Player from moving during setup.
		
		public bool gameBattle;

		public GameObject player;

		public GameObject enemyInBattle;
		public GameObject playerInBattle;

		private GameObject currentlyFightingEnemy = null;
		
		//Awake is always called before any Start functions
		void Awake()
		{
			Debug.Log("Awake GameManager");	

			boardCamera.SetActive(true);
			battleCamera.SetActive(false);

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

			Debug.Log(boardCamera);
			Debug.Log(battleCamera);
			
			boardCamera.SetActive(true);
			battleCamera.SetActive(false);
		
		
			//Assign enemies to a new List of Enemy objects.
			enemies = new List<Enemy>();
			
			//Get a component reference to the attached BoardManager script
			boardScript = GetComponent<BoardManager>();			

			//Call the InitGame function to initialize the first level 
			InitGame();		
		}

        //this is called only once, and the paramter tell it to be called only after the scene was loaded
        //(otherwise, our Scene Load callback would be called the very first load, and we don't want that)
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization()
        {
			//register the callback to be called everytime the scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        //This is called each time a scene is loaded.
        static private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
             instance.level++;
             instance.InitGame();
        }
		
		
		//Initializes the game for each level.
		void InitGame()
		{
			//While doingSetup is true the player can't move, prevent player from moving while title card is up.
			doingSetup = true;
			
			//Get a reference to our image LevelImage by finding it by name.
			levelImage = GameObject.Find("LevelImage");
			
			//Get a reference to our text LevelText's text component by finding it by name and calling GetComponent.
			levelText = GameObject.Find("LevelText").GetComponent<Text>();
			
			//Set the text of levelText to the string "Day" and append the current level number.
			levelText.text = "Level " + level;
			
			//Set levelImage to active blocking player's view of the game board during setup.
			levelImage.SetActive(true);
			
			//Call the HideLevelImage function with a delay in seconds of levelStartDelay.
			Invoke("HideLevelImage", levelStartDelay);
			
			//Clear any Enemy objects in our List to prepare for next level.
			enemies.Clear();
			
			//Call the SetupScene function of the BoardManager script, pass it current level number.
			Debug.Log("Call setup scene");
			boardScript.SetupScene(level);
		}
		
		//Hides black image used between levels
		void HideLevelImage()
		{
			//Disable the levelImage gameObject.
			levelImage.SetActive(false);
			
			//Set doingSetup to false allowing player to move again.
			doingSetup = false;
		}
		
		//Update is called every frame.
		void Update()
		{			
//			if(!gameBattle) {
//			    if ((int)(Input.GetAxisRaw("Fire1")) == 1)
//			    {
//			        loadScene2();
//			    }
                //Check that playersTurn or enemiesMoving or doingSetup are not currently true.
                if (playersTurn || enemiesMoving || doingSetup)					
					//If any of these are true, return and do not start MoveEnemies.
					return;
				
				StartCoroutine(MoveEnemies());
//			} else {				
//
//				//Debug.Log(isBattle);
//				if((int) (Input.GetAxisRaw("Fire1"))==1) {				
//					loadMainScene();
//				}
//			}
		}
		
		public void loadScene2(){											
				SceneManager.LoadScene("scene2");
		}

		public void loadMainScene(){										
				SceneManager.LoadScene("_Complete-Game");
		}

		//Call this to add the passed in Enemy to the List of Enemy objects.
		public void AddEnemyToList(Enemy script)
		{
			//Add Enemy to List enemies.
			enemies.Add(script);
		}
		
		
		//GameOver is called when the player reaches 0 food points
		public void GameOver()
		{
			//Set levelText to display number of levels passed and game over message
			levelText.text = "You survived " + level + " levels.";
			
			//Enable black background image gameObject.
			levelImage.SetActive(true);
			
			//Disable this GameManager.
			enabled = false;
		}
		
		//Coroutine to move enemies in sequence.
		IEnumerator MoveEnemies()
		{
			//While enemiesMoving is true player is unable to move.
			enemiesMoving = true;
			
			//Wait for turnDelay seconds, defaults to .1 (100 ms).
			yield return new WaitForSeconds(turnDelay);
			
			//If there are no enemies spawned (IE in first level):
			if (enemies.Count == 0) 
			{
				//Wait for turnDelay seconds between moves, replaces delay caused by enemies moving when there are none.
				yield return new WaitForSeconds(turnDelay);
			}
			
			//Loop through List of Enemy objects.
			for (int i = 0; i < enemies.Count; i++)
			{
				//Call the MoveEnemy function of Enemy at index i in the enemies List.
				enemies[i].MoveEnemy ();
				
				//Wait for Enemy's moveTime before moving next Enemy, 
				yield return new WaitForSeconds(enemies[i].moveTime);
			}
			//Once Enemies are done moving, set playersTurn to true so player can move.
			playersTurn = true;
			
			//Enemies are done moving, set enemiesMoving to false.
			enemiesMoving = false;
		}

		public void EnterBattle(GameObject enemyObject) {
			Debug.Log("Entering battle:");

			currentlyFightingEnemy = enemyObject;

			Debug.Log(boardCamera);
			Debug.Log(battleCamera);

			boardCamera.SetActive(false);
			battleCamera.SetActive(true);

			//enemybattle = get random from list

			player.GetComponent<Player>().canMove = false;

			// GameObject enemy = Instantiate(enemyInBattle, enemyPodium.position, Quaternion.identity) as GameObject;

			// enemy.transform.parent = enemyPodium;

			// GameObject fightingPlayer = Instantiate(playerInBattle, playerPodium.position, Quaternion.identity) as GameObject;

			// fightingPlayer.transform.parent = playerPodium;

		}

		public void ExitBattle() {
			Debug.Log("Entering battle:");

			foodText.text = "Health " + playerHealthPoints + "/" + Player.maxHealth;

			boardCamera.SetActive(true);
			battleCamera.SetActive(false);

			player.GetComponent<Player>().canMove = true;

			if (currentlyFightingEnemy != null ) {
				currentlyFightingEnemy.SetActive(false);
				currentlyFightingEnemy = null;
			}
			

		}
	}
}

