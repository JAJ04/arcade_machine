using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DodgeEm
{
    public class UIManager : MonoBehaviour
    {
        // Reference to the game over text
        public GameObject gameOverTextBTTF;

        // Reference to the replay text
        public GameObject replayText;
        // Reference to the main menu text
        public GameObject mainMenuText;
        // Reference to the exit text
        public GameObject exitText;

        // Reference to the BTTF buttons and normal buttons in the scene
        public Button replayButton;
        public Button menuButton;
        public Button exitButton;

        public Button replayButtonBTTF;
        public Button menuButtonBTTF;
        public Button exitButtonBTTF;

        // Reference to the text in the scene and the BTTF text
        public Text scoreText;
        public Text scoreBTTFText;

        // Reference to the game over text in the scene
        public Text gameOverText;

        // Variable that holds the value of the score
        private int scoreValue;

        // Variable to see when the game is over
        private bool gameOver;
        // Bool that allows the player to go back to the god menu if the game is over 
        private bool goBackToGodMenuBool;
        // Bool to only allow the back sound to play once
        private bool soundPlayOnce = true;

        // Reference to the pause game to not allow you to pause it
        // when the game is over
        public PauseGame pausedGame;

        // Reference to the player script
        public PlayerController player;

        // Reference to the back sound to play
        public AudioSource backSound;

        private void Start()
        {
            // Initialize the boolean variable
            gameOver = false;
            // Initialize the score
            scoreValue = 0;
            // Keep incrementing the score once in a while
            InvokeRepeating("IncrementScore", 1.0f, 0.5f);
        }

        private void Update()
        {
            if(gameOver == false)
            {
                // Keep on updating the score text to what the score value is and convert
                // it to a string
                scoreText.text = "Score: " + scoreValue.ToString();
                scoreBTTFText.text = "SCORE: " + scoreValue.ToString();
            }

            // If this bool is true then you can go back to the god menu when "LeftAlt" is pressed
            if (goBackToGodMenuBool)
            {
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    StartCoroutine(GoBackToGodMenu());
                }
            }
        }

        public void GameOverConfirmed()
        {
            if(player.activateBTTFGUI == false)
            {
                // Make the game over true when this function is called
                gameOver = true;

                // Show the game over text
                gameOverText.gameObject.SetActive(true);

                // Don't allow the game to be paused
                pausedGame.allowPause = false;

                // Show the buttons
                replayButton.gameObject.SetActive(true);
                menuButton.gameObject.SetActive(true);
                exitButton.gameObject.SetActive(true);

                // Allow the player to go to the god menu
                goBackToGodMenuBool = true;

                // Select the replay button
                replayButton.Select();
            }
            else
            {
                // Show the BTTF score text
                scoreBTTFText.gameObject.SetActive(true);

                // Disable the normal score text
                scoreText.gameObject.SetActive(false);

                // Disable the normal buttons and show the BTTF buttons

                // Make the game over true when this function is called
                gameOver = true;

                // Show the game over text
                gameOverTextBTTF.SetActive(true);

                // Don't allow the game to be paused
                pausedGame.allowPause = false;

                // Show the buttons but disable the text associated with the buttons
                replayButton.gameObject.SetActive(true);
                replayText.SetActive(false);

                // Show the BTTF button
                replayButtonBTTF.gameObject.SetActive(true);

                menuButton.gameObject.SetActive(true);
                mainMenuText.SetActive(false);

                // Show the main menu BTTF button
                menuButtonBTTF.gameObject.SetActive(true);

                exitButton.gameObject.SetActive(true);
                exitText.SetActive(false);

                // Show the exit game BTTF button
                exitButtonBTTF.gameObject.SetActive(true);

                // Allow the player to go to the god menu
                goBackToGodMenuBool = true;

                // Select the replay button
                StartCoroutine(SelectButtonLater());
            }
        }

        // Increases the score by 1
        void IncrementScore()
        {
            scoreValue += 1;
        }

        // Reload the level again
        public void ReplayGame()
        {
            StartCoroutine(LoadReplay());
        }

        // Loads the menu again
        public void ReturnToMenu()
        {
            StartCoroutine(LoadReturnToMenu());
        }

        // Loads the god menu
        public void ExitGame()
        {
            StartCoroutine(LoadExitGame());
        }

        public void ReplayGameBTTF()
        {
            StartCoroutine(LoadReplayBTTF());
        }

        public void ReturnToMenuBTTF()
        {
            StartCoroutine(LoadReturnToMenuBTTF());
        }

        public void ExitGameBTTF()
        {
            StartCoroutine(LoadExitGameBTTF());
        }

        IEnumerator LoadReplay()
        {
            yield return new WaitForSeconds(0.35f);
            SceneManager.LoadScene("dodge-em");
        }

        IEnumerator LoadReturnToMenu()
        {
            yield return new WaitForSeconds(0.35f);
            SceneManager.LoadScene("dodge-em-menu");
        }

        IEnumerator LoadExitGame()
        {
            yield return new WaitForSeconds(0.35f);
            SceneManager.LoadScene("godMenu");
        }

        IEnumerator LoadReplayBTTF()
        {
            yield return new WaitForSeconds(0.8f);
            SceneManager.LoadScene("dodge-em");
        }

        IEnumerator LoadReturnToMenuBTTF()
        {
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene("dodge-em-menu");
        }

        IEnumerator LoadExitGameBTTF()
        {
            yield return new WaitForSeconds(0.35f);
            SceneManager.LoadScene("godMenu");
        }

        // Selects the button after waiting a frame
        IEnumerator SelectButtonLater()
        {
            yield return null;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(replayButtonBTTF.gameObject);
        }

        IEnumerator GoBackToGodMenu()
        {
            // Play a sound (only once) and switch scenes after 0.25f seconds
            if (soundPlayOnce)
            {
                backSound.Play();
                soundPlayOnce = false;
            }

            yield return new WaitForSeconds(0.25f);

            SceneManager.LoadScene("godMenu");
        }
    }
}