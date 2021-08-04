using UnityEngine;
using System.Collections;

namespace RogueKnight 
{
    public class LoadGameSoundManager : MonoBehaviour 
    {
        // These are game objects that will hold the associated scripts
        public GameObject gameManager;
        public GameObject soundManager;

        void Awake() 
	{
            // Checks to see if a GameManager has been assigned to a static variable
            if (GameManager.gmInstance == null) 
	    {
                // If not then instantiate the gameManager prefab
                Instantiate(gameManager);
            }

            // Checks to see if a SoundManager has been assigned to a static variable
            if (SoundManager.instance == null) 
	    {
                // If not then instantiate the soundManager prefab
                Instantiate(soundManager);
            }
        }
    }
}
