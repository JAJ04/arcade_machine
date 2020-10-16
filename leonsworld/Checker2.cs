using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LeonsWorld
{
    public class Checker2 : MonoBehaviour
    {
        // This script checks to see if the player is dead and 
        // resets the boss scene if the gameManger is null
        // i.e. player is dead

        // Property to hold the boss object
	    public GameObject boss;
        
        // Property to hold the win face
	    public GameObject winFace;

        // GameObject array to hold the boss bullets
        GameObject[] gameObjects;

        // Property to hold the GameManager
        public GameManager gameManager;
	
        // Reference for properties to the palyer
	    public PlayerControl playerControl;
	
        // This is the win sound
	    public AudioSource win;
	
        // This is used to show the particle effects when the player defeats the boss
	    public ParticleSystem winner;
	    public ParticleSystem winner2;
	    public ParticleSystem winner3;
	    public ParticleSystem old;
	
        // This is a bool that checks whether the player has beaten the boss or not
	    public bool winBool = false;

        // Timer used to reset states
	    public float timer = 0;
	
	    // Use this for initialization
	    void Start () {
		    winFace.GetComponent<SpriteRenderer>().enabled = false;
	    }
	
	    // Update is called once per frame
	    void Update ()
	    {
            // If the boss is null
		    if(boss == null)
		    {
                // Don't play the boss MJ Moonwalker sound
			    gameManager.MJ.Stop();

                // Play the win sound if the player has defeated the boss
                // with the specified conditions
			    if(winBool == false)
			    {
				    win.Play();
                    winBool = true;
                }
			
                // This enables the winFace and disables the bossFace to indicate that the player has won
			    gameManager.bossFace.GetComponent<SpriteRenderer>().enabled = false;
                winFace.GetComponent<SpriteRenderer>().enabled = true;

                // This plays the particle effect sounds
                winner.Play();
			    winner2.Play();
			    winner3.Play();
			    Destroy(old);
			
                // Used to reset states further
			    timer += Time.deltaTime;
			
			    winner.GetComponent<ParticleSystem>().Play();
			    winner2.GetComponent<ParticleSystem>().Play();
			    winner3.GetComponent<ParticleSystem>().Play();
			
			    DestroyAllObjects();
			
                // This is used to stop repetitions of the win sound
			    if(timer >= 2f)
			    {
				    winBool = false;
			    }
			
                // Reset the scene after 6 seconds
			    if(timer >= 6f)
			    {
                    Time.timeScale = 1;
				    gameManager.MJ.Stop();
				    SceneManager.LoadScene("cutscene2FadeIn");
			    }

                // Make it so that the player is no longer able to control their character
			    gameManager.controller2D.controllable = false;
			    gameManager.controller2D.horizontal = 0;

                // Save the game
                gameManager.SaveGame();
		    }
	    }
	
         // This destroys the SnowballBoss bullets
	     void DestroyAllObjects()
         {
              gameObjects = GameObject.FindGameObjectsWithTag ("SnowballBoss");
         
             for(var i = 0 ; i < gameObjects.Length ; i ++)
             {
                 Destroy(gameObjects[i]);
             }
         }
    }
}