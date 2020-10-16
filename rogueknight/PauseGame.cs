using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RogueKnight
{
    public class PauseGame : MonoBehaviour
    {
        // Reference to the text on the screen and the shadow
        public Text pausedText;

        // Reference to the credits text to show it when the game is paused
        public Text creditsText;

        // Reference to the button to return to the Main Menu
        public GameObject mainMenuButton;

        // Bool to turn on/off the paused text
        public bool pausedTextOnOff;

        // Bool to test whether the game has started or not
        public bool allowPause = true;

        // This is a bool that will allow the player to pause the game after 2 seconds
        public bool allowPauseAfterImage = false;

        // Reference to the speeding audio source
        public AudioSource pauseMenuSound;

        private void Start()
        {
            // Allow the player to pause the game after 2 seconds (after the dungeon image)
            StartCoroutine(AllowPause());
        }

        // Update is called once per frame
        void Update()
        {
            if(allowPauseAfterImage)
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

                            // Play the pause menu sound
                            pauseMenuSound.Play();

                            // Unpause the entire game
                            Time.timeScale = 1;

                            // Don't show the return to main menu button
                            mainMenuButton.SetActive(false);
                        }

                        if (pausedTextOnOff)
                        {
                            // Enable the paused text and shadow if it is not showing
                            pausedText.GetComponent<Text>().enabled = true;

                            // Show the credits text
                            creditsText.GetComponent<Text>().enabled = true;

                            // Select the main menu button
                            StartCoroutine(SelectButtonLater());

                            // Pause the pause menu sound
                            pauseMenuSound.Pause();

                            // Show the return to main menu button
                            mainMenuButton.SetActive(true);

                            // Pause the entire game
                            Time.timeScale = 0;
                        }
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

        // Will set allowPauseAfterImage to true after 2 seconds
        IEnumerator AllowPause()
        {
            yield return new WaitForSeconds(2f);
            allowPauseAfterImage = true;
        }
    }
}
