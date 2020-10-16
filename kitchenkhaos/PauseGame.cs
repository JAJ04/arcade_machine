using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Catch
{
    public class PauseGame : MonoBehaviour
    {
        // Reference to the text on the screen and the shadow
        public Text pausedText;
        public Text pausedTextShadow;

        // Reference to the credits text to show the credits when the game is paused
        public Text creditsText;

        // Reference to the button to return to the Main Menu
        public GameObject mainMenuButton;

        // Bool to turn on/off the paused text
        private bool pausedTextOnOff;

        // Bool to test whether the game has started or not
        public bool gameStarted = false;

        // Update is called once per frame
        void Update()
        {
            if(gameStarted == true)
            {
                // Check if "Left Alt" is pressed and if it is
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    // Flip the pausedTextOnOff to enable the text
                    pausedTextOnOff = !pausedTextOnOff;

                    // Test the paused text on/off
                    if (pausedTextOnOff == false)
                    {
                        // Disable the paused text and shadow if it is showing
                        pausedText.GetComponent<Text>().enabled = false;
                        pausedTextShadow.GetComponent<Text>().enabled = false;

                        // Unpause the entire game
                        Time.timeScale = 1;

                        // Don't show the return to main menu button
                        mainMenuButton.SetActive(false);
                    }

                    if (pausedTextOnOff)
                    {
                        // Enable the paused text and shadow if it is not showing
                        pausedText.GetComponent<Text>().enabled = true;
                        pausedTextShadow.GetComponent<Text>().enabled = true;

                        // Show the return to main menu button
                        mainMenuButton.SetActive(true);

                        // Show the credits text 
                        creditsText.GetComponent<Text>().enabled = true;

                        // Select the menu button
                        StartCoroutine(SelectContinueButtonLater());

                        // Pause the entire game
                        Time.timeScale = 0;
                    }
                }

                // If "Space" is pressed unpause the game and the player will go straight
                // to the main menu
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Unpause the entire game
                    Time.timeScale = 1;
                }
            }
        }

        // Selects the button after waiting a frame
        IEnumerator SelectContinueButtonLater()
        {
            yield return null;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuButton.gameObject);
        }
    }
}