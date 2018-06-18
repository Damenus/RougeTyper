using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

public class PlayerInBattle : MonoBehaviour {

	public AudioClip attackSound1;						//First of two audio clips to play when attacking the player.
	public AudioClip attackSound2;	

	public AudioClip gameOverSound;				//Audio clip to play when player dies.
	public AudioClip playerHitSound;				//Audio clip to play when player dies.

	private Animator animator;					//Used to store a reference to the Player's animator component.

	public int health;                           //Used to store player food points total during level.

	private int damage = 50;

	// Use this for initialization
	void Start () {
		//Get a component reference to the Player's animator component
		animator = GetComponent<Animator>();

		health = GameManager.instance.playerHealthPoints;
	}
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable() {
		   // Prevent to init on very first start of the game
        if (GameManager.instance != null && GameManager.instance.isBattle) {
			Debug.Log("OnEnable playerInBattle");
			health = GameManager.instance.playerHealthPoints;		
			Debug.Log(health);
		}
	}

	public void attack(GameObject enemy) {
		animator.SetTrigger("playerChop");				
		SoundManager.instance.RandomizeSfx (attackSound1, attackSound2);
		enemy.GetComponent<EnemyInBattle>().hit(damage);

	}

    public void GetDamage(int damage)
    {
        health -= damage;
		SoundManager.instance.PlaySingle (playerHitSound);
		animator.SetTrigger("playerHit");
        GameManager.instance.SetPlayersHealth(health);
        CheckIfGameOver();
    }

    private void CheckIfGameOver ()
		{
			//Check if food point total is less than or equal to zero.
			if (health <= 0) 
			{
				// //Call the PlaySingle function of SoundManager and pass it the gameOverSound as the audio clip to play.
				// SoundManager.instance.PlaySingle (gameOverSound);
				
				// //Stop the background music.
				// SoundManager.instance.musicSource.Stop();
				
				//Call the GameOver function of GameManager.
                BattleManager.instance.ClearScreen();
				GameManager.instance.GameOver ();
			}
		}
}
