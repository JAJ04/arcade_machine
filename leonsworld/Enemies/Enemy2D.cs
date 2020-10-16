using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class Enemy2D : MonoBehaviour
    {
        // Used to get properties from the player
	    public PlayerControl controller2D;
	
	    // Enemy start and end position
	    float startingPos;
	    float endPos;

	    // Units that the enemy will move right
	    public int unitsToMove = 5;
	    // Enemies movement speed
	    public int moveSpeed = 2;

	    // Enemy will move to the right first but it will alternate between right/left
	    bool moveRight = true;
	
	    //Enemy health
	    public int enemyHealth = 1;

        // Types of Enemies
        public bool basicEnemy;
	    public bool advancedEnemy;

        // Variable to only allow the coroutine for TakenDamage to be ran once
        bool takenDamage = true;

        // GameObject variables that hold the exclamation mark and bullet properties
	    public GameObject exclamation;
	    public GameObject bullet;
	
        // Variable used to get GameManager properties
	    public GameManager gameManager;

        // Used to play sounds
	    public AudioSource evilYell;
	    public AudioSource enemyHit;
	    public AudioSource enemyHurt;

	    void Awake()
	    {
            // Gets the start and end position of the enemy
		    startingPos = transform.position.x;
		    endPos = startingPos + unitsToMove;
		
            // enemyHealth is 20 if it is a basic enemy
		    if(basicEnemy)
		    {
			    enemyHealth = 20;
		    }
		
            // enemyHealth is 30 if it is an advanced enemy
		    if(advancedEnemy)
		    {
			    enemyHealth = 30;
		    }
	    }	
	
	    void Update()
	    {
            // If moveRight is true then move the game object to the right according to a speed
		    if(moveRight)
		    {
			    GetComponent<Rigidbody>().position += Vector3.right * moveSpeed * Time.deltaTime;
		    }
		
            // If it is greater than the end position then moveRight is now false
		    if(GetComponent<Rigidbody>().position.x >= endPos)
		    {
			    moveRight = false;
		    }

            // If moveRight is false then move to the left
		    if(!moveRight)
		    {
			    GetComponent<Rigidbody>().position -= Vector3.right * moveSpeed * Time.deltaTime;
		    }

            // If it is less than the startingPos then move right again
		    if(GetComponent<Rigidbody>().position.x <= startingPos)
		    {
			    moveRight = true;
		    }
	    }
	
	    void OnTriggerEnter(Collider col)
	    {
            // If the collider is a Player then
		    if(col.gameObject.tag == "Player")
		    {
                // The player will take some damage
			    gameManager.controller2D.SendMessage("TakenDamage", SendMessageOptions.DontRequireReceiver);

                // Sound the "playerHurt" clip to indicate that a hit has occured
                if (!enemyHurt.isPlaying && gameObject != null)
                {
                    enemyHurt.Play();
                }
            }
	    }
	
	    // Enemy Taking Damage
	    void EnemyDamaged(int damage)
	    {
            // If the enemyHealth > 0 then you can decrease the health by 1
            // and play a sound
		    if(enemyHealth > 0)
		    {
			    enemyHealth -= damage;

                // Sound the "playerHurt" clip to indicate that a hit has occured
                if (!enemyHurt.isPlaying && gameObject != null)
                {
                    enemyHurt.Play();
                }
            }
		
            // If the enemy is dead then give the player some coins, destroy the enemy
            // and play some sounds
		    if(enemyHealth <= 0)
		    {
			    enemyHealth = 0;
			    Destroy(gameObject);
			    if(basicEnemy)
			    {
				    GameManager.curEXP += 15;
				    GameManager.curCoins += 50;
				    evilYell.Play();
				    Destroy (bullet);

                    // Start decreasing curEXP and maxEXP if you are at the highest level
				    if(GameManager.level == 10)
				    {
                        GameManager.curEXP -= 5;
					    gameManager.maxEXP -= 500;
					    evilYell.Play();
				    }
			    }

                // If the player kills an advancedEnemy
                // 
			    if(advancedEnemy)
			    {
                    GameManager.curEXP += 35;
				    GameManager.curCoins += 80;
				    Destroy (bullet);

                    // Start decreasing curEXP and maxEXP if you are at the highest level
                    if (GameManager.level == 10)
				    {
                        GameManager.curEXP -= 5;
					    gameManager.maxEXP -= 500;
					    evilYell.Play();
				    }
			    }
		    }
	    }
	
        // If the enemy takes damage flash the enemy sprite 
        // Coroutine will only run when takenDamage is true which means that
        // the coroutine will not be allowed to restart until TakenDamage is true again
        // at the end of the coroutine
	    public IEnumerator TakenDamage()
	    {
            if(takenDamage)
            {
                takenDamage = false;

                GetComponent<Renderer>().enabled = false;
                exclamation.GetComponent<Renderer>().enabled = false;
                yield return new WaitForSeconds(1);
                GetComponent<Renderer>().enabled = true;
                exclamation.GetComponent<Renderer>().enabled = true;
                yield return new WaitForSeconds(1);
                GetComponent<Renderer>().enabled = false;
                exclamation.GetComponent<Renderer>().enabled = false;
                yield return new WaitForSeconds(1);
                GetComponent<Renderer>().enabled = true;
                exclamation.GetComponent<Renderer>().enabled = true;

                takenDamage = true;
            }
	    }
    }

}
