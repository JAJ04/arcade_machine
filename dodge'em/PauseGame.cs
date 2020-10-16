using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DodgeEm
{
    public class PauseGame : MonoBehaviour
    {
        // Reference to the text on the screen and the shadow
        public Text pausedText;

        // Reference to the text to display it when the game is paused
        public Text creditsText;

        // Reference to the button to return to the Main Menu
        public GameObject mainMenuButton;

        // Bool to turn on/off the paused text
        public bool pausedTextOnOff;

        // Bool to test whether the game has started or not
        public bool allowPause = true;

        // Reference to the speeding audio source
        public AudioSource speeding;

        // Update is called once per frame
        void Update()
        {
            if (allowPause == true)
            {
                // Check if "LeftAlt" is pressed and if it is
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    // Flip the pausedTextOnOff to enable the text
                    pausedTextOnOff = !pausedTextOnOff;

                    // Test the paused text on/off
                    if (pausedTextOnOff == false)
                    {
                        // Disable the paused text and shadow if it is showing
                        pausedText.GetComponent<Text>().enabled = false;

                        // Play the speeding sound
                        speeding.Play();

                        // Unpause the entire game
                        Time.timeScale = 1;

                        // Don't show the return to main menu button
                        mainMenuButton.SetActive(false);
                    }

                    if (pausedTextOnOff)
                    {
                        // Enable the paused text and shadow if it is not showing
                        pausedText.GetComponent<Text>().enabled = true;

                        // Enable the credits text always
                        creditsText.GetComponent<Text>().enabled = true;

                        // Select the main menu button
                        StartCoroutine(SelectButtonLater());

                        // Pause the speeding sound
                        speeding.Pause();

                        // Show the return to main menu button
                        mainMenuButton.SetActive(true);

                        // Pause the entire game
                        Time.timeScale = 0;
                    }
                }
            }
        }

        // Selects the button after waiting a frame
        IEnumerator SelectButtonLater()
        {
            yield return null;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuButton.gameObject);
        }
    }
}