using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeonsWorld
{
    public class BackToGame : MonoBehaviour
    {
        public void BackToGameFunction()
        {
            // Play a sound
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            // Unpauses the game so the player can play again
            GameManager.pauseMenu = false;

            // Unpause the game when the button is pressed
            Time.timeScale = 1;

            // Disable the pause menu text
            GameObject.Find("PausedText").SetActive(false);

            // Allow the weapons to be used again and the CTRL, 1, 2 weapon switching
            GameManager.keysActive = true;
            GameManager.wepKeys = true;
        }
    }
}
