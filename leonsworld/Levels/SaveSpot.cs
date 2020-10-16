using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LeonsWorld
{
    public class SaveSpot : MonoBehaviour
    {
        // Reference to the game manager
	    public GameManager gm;
        // Gets the scene name and stores it into a string
	    public string level;

        // If the player enters the save spot then go to the next scene and save
	    void OnTriggerEnter(Collider other)
	    {
		    if(other.tag == "Player")
		    {
                Time.timeScale = 1;
			    gm.SaveGame();
			    SceneManager.LoadScene(level);

                // Set the LevelUnlock integers depending on what scenes are loaded
                if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("level1"))
                {
                    PlayerPrefs.SetInt("LevelUnlock", 2);
                }
            }
	    }
    }
}