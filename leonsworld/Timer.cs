using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeonsWorld
{
    public class Timer : MonoBehaviour
    {
        // This script is used to give a timer as to
        // how long the player has to complete the scene
        // Game crashes without this (no idea why) so a gameplay element
        // with a timer was implemented

        // This declares a time (120 secs) to complete the scene
        private float timer = 120f;

        // GUI text will only display if this is true
        private bool enableTimeRemainingText = true;

        // Reference to the UI text on the screen to update
        public Text timeRemainingText;
        // Reference to the UI panel on the screen
        public Image panel;

        // Reference to the GameManager script
        public GameManager gameManager;

        // Update is called once per frame
        void Update ()
        {
            // Keep decreasing the timer every update
            timer -= Time.deltaTime;
        
            // Disable the time remaining button 
            // if the user is dead
            if(GameManager.curHealth <= 0)
            {
                enableTimeRemainingText = false;
            }

            if(timer < 0)
            {
                // Disable to time remaining display
                enableTimeRemainingText = false;
                // Kill the player
                GameManager.curHealth = 0;
                // Don't allow the player to change weapons with LCTRL and num keys
                GameManager.wepKeys = false;
            }

            // Keep updating the text if you are allowed to
            if(enableTimeRemainingText)
            {
                timeRemainingText.GetComponent<Text>().text = "" + timer.ToString("0") + "\n seconds remaining.";
            }
            else
            {
                // Disable both the time remaining text and the panel itself
                timeRemainingText.gameObject.SetActive(false);
                panel.gameObject.SetActive(false);
                gameManager.timerIsNotDone = false;
            }
	    }
    }
}