using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKnight
{
    public class GoToGodMenuUnpause : MonoBehaviour
    {
        // Reference to the sound to play
        public AudioSource pauseMenuSound;

        // Reference to the pause script
        public PauseGame pauseGame;

        // Update is called once per frame
        void Update()
        {
            if (pauseGame.pausedTextOnOff)
            {
                // If "Space" is pressed unpause the game and the player will go straight
                // to the main menu
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Play the pause menu sound
                    pauseMenuSound.Play();

                    // Unpause the entire game
                    Time.timeScale = 1;

                    // Destroy both GameManager and SoundPrefab to prevent errors when going back to the god menu
                    Destroy(GameObject.Find("GameManager(Clone)").gameObject);
                    Destroy(GameObject.Find("SoundPrefab(Clone)").gameObject);
                }
            }
        }
    }
}