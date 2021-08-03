using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DodgeEm
{
    public class GoToGodMenuUnpause : MonoBehaviour
    {
        // Reference to the sound to play
        public AudioSource speeding;

        // Reference to the pause script
        public PauseGame pauseGame;

	// Update is called once per frame
	void Update ()
        {
            if(pauseGame.pausedTextOnOff)
            {
                // If "Space" is pressed unpause the game and the player will go straight
                // to the main menu
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Play the speeding sound
                    speeding.Play();

                    // Unpause the entire game
                    Time.timeScale = 1;
                }
            }
        }
    }
}
