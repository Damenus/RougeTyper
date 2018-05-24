using UnityEngine;
using System;
using System.Collections.Generic;
using Assets.WordsRepository;
using Random = UnityEngine.Random;

namespace Completed
{
	public class BoardManager : MonoBehaviour
	{
	    public WordsRepository WordsRepository;

		// Using Serializable allows us to embed a class with sub properties in the inspector.
		[Serializable]
		public class Count
		{
			public int minimum;
			public int maximum;
		}
		
		public int columns = 8; 										//Number of columns in our game board.
		public int rows = 8;											//Number of rows in our game board.
		public Count wallCount = new Count {minimum = 5, maximum = 9};  //Lower and upper limit for our random number of wall items per level.
        public Count foodCount = new Count {minimum = 1, maximum = 5};  //Lower and upper limit for our random number of food items per level.
        public GameObject exitWoods;											//Prefab to spawn for exit.
		public GameObject exitRuins;											//Prefab to spawn for exit.
		public GameObject[] floorTilesWoods;									//Array of floor prefabs.
		public GameObject[] floorTilesRuins;									//Array of floor prefabs.
		public GameObject[] wallTilesWoods;									//Array of wall prefabs.
		public GameObject[] wallTilesRuins;									//Array of wall prefabs.
		public GameObject[] foodTiles;									//Array of food prefabs.
		public GameObject[] enemyTiles;									//Array of enemy prefabs.
		public GameObject[] outerWallTilesWoods;								//Array of outer tile prefabs.
		public GameObject[] outerWallTilesRuins;								//Array of outer tile prefabs.

	
		
		private enum LevelType {Woods, Ruins};
		private LevelType currentLevelType = LevelType.Woods;
		
		private Transform boardHolder;									//A variable to store a reference to the transform of our Board object.
		private List <Vector3> gridPositions = new List <Vector3> ();	//A list of possible locations to place tiles.
		

		private GameObject getExit() {
			if(currentLevelType.Equals(LevelType.Woods)) {
				return exitWoods;
			} else if(currentLevelType.Equals(LevelType.Ruins)) {
				return exitRuins;
			} else {
				return exitWoods;
			}

		}

		private GameObject[] getFloorTiles() {
			if(currentLevelType.Equals(LevelType.Woods)) {
				return floorTilesWoods;
			} else if(currentLevelType.Equals(LevelType.Ruins)) {
				return floorTilesRuins;
			} else {
				return floorTilesWoods;
			}

		}

		private GameObject[] getWallTiles() {
			if(currentLevelType.Equals(LevelType.Woods)) {
				return wallTilesWoods;
			} else if(currentLevelType.Equals(LevelType.Ruins)) {
				return wallTilesRuins;
			} else {
				return wallTilesWoods;
			}

		}
		private GameObject[] getOuterWallTiles() {
			if(currentLevelType.Equals(LevelType.Woods)) {
				return outerWallTilesWoods;
			} else if(currentLevelType.Equals(LevelType.Ruins)) {
				return outerWallTilesRuins;
			} else {
				return outerWallTilesWoods;
			}

		}

	
		

		
		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}
		
		//Clears our list gridPositions and prepares it to generate a new board.

	    //Sets up the outer walls and floor (background) of the game board.

	    public void SetupScene (int level)
		{

			Array levelValues = Enum.GetValues(typeof(LevelType));
			currentLevelType = (LevelType)levelValues.GetValue(Random.Range(0, levelValues.Length));
		
			//Creates the outer walls and floor.
		    BoardSetup();
		    InitialiseList();

		    //Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
		    LayoutObjectAtRandom(getWallTiles(), wallCount.minimum, wallCount.maximum);

		    //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
		    LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);

		    //Determine number of enemies based on current level number, based on a logarithmic progression
		    int enemyCount = (int)Mathf.Log(level, 2f);

		    //Instantiate a random number of enemies based on minimum and maximum, at randomized positions.
		    LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);

		    //Instantiate the exit tile in the upper right hand corner of our game board
		    Instantiate(getExit(), new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
		}

	    void BoardSetup ()
	    {
//	        WordsRepository = XmlManager.Deserialize<WordsRepository>();
			

	        //Instantiate Board and set boardHolder to its transform.
	        boardHolder = new GameObject ("Board").transform;
			
	        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
	        for(int x = -1; x < columns + 1; x++)
	        {
	            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
	            for(int y = -1; y < rows + 1; y++)
	            {
	                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
	                GameObject toInstantiate = getFloorTiles()[Random.Range (0,getFloorTiles().Length)];
					
	                //Check if we current position is at board edge, if so choose a random outer wall prefab from our array of outer wall tiles.
	                if(x == -1 || x == columns || y == -1 || y == rows)
	                    toInstantiate = getOuterWallTiles() [Random.Range (0, getOuterWallTiles().Length)];
					
	                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
	                GameObject instance =
	                    Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
					
	                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
	                instance.transform.SetParent (boardHolder);
	            }
	        }
	    }

	    void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
	    {
	        //Choose a random number of objects to instantiate within the minimum and maximum limits
	        int objectCount = Random.Range (minimum, maximum+1);
			
	        //Instantiate objects until the randomly chosen limit objectCount is reached
	        for(int i = 0; i < objectCount; i++)
	        {
	            //Choose a position for randomPosition by getting a random position from our list of available Vector3s stored in gridPosition
	            Vector3 randomPosition = RandomPosition();
				
	            //Choose a random tile from tileArray and assign it to tileChoice
	            GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];
				
	            //Instantiate tileChoice at the position returned by RandomPosition with no change in rotation
	            Instantiate(tileChoice, randomPosition, Quaternion.identity);
	        }
	    }

	    Vector3 RandomPosition ()
	    {
	        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
	        int randomIndex = Random.Range (0, gridPositions.Count);
			
	        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
	        Vector3 randomPosition = gridPositions[randomIndex];
			
	        //Remove the entry at randomIndex from the list so that it can't be re-used.
	        gridPositions.RemoveAt (randomIndex);
			
	        //Return the randomly selected Vector3 position.
	        return randomPosition;
	    }

	    void InitialiseList ()
	    {
	        //Clear our list gridPositions.
	        gridPositions.Clear ();
			
	        //Loop through x axis (columns).
	        for(int x = 1; x < columns-1; x++)
	        {
	            //Within each column, loop through y axis (rows).
	            for(int y = 1; y < rows-1; y++)
	            {
	                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
	                gridPositions.Add (new Vector3(x, y, 0f));
	            }
	        }
	    }
	}
}
