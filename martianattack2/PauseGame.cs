using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UFO
{
    public class PauseGame : MonoBehaviour
    {
        // Reference to the button to return to the Main Menu
        public GameObject mainMenuButton;

        // Variable to hold the pause menu
        [SerializeField]
        private GameObject _pauseMenuObject;

        // Bool to turn on/off the paused text
        private bool pausedImageOnOff;

        // Bool to test whether the game has started or not
        public bool gameStarted = false;

        // Reference to the game manager
        public GameManager gameManager;

        // Variable to hold the animation for the pause menu
        private Animator _pauseAnimator;

        // Bool to allow the pause menu to pop up
        public bool allowPause = false;

        // Reference to the credit text to show it when the game is paused
        public Text creditText;

        // Reference to the countdown sound to pause it
        public AudioSource countdownSound;

        // Reference to the game over sound
        public AudioSource gameOverSound;

        private void Start()
        {
            // Set the _pauseAnimator variable to the script on the "Pause_Menu_Panel" object
            _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();

            // Make the _pauseAnimator disregard Time.timeScale freezing
            _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        // Update is called once per frame
        void Update()
        {
            // Allow the pausing of the game whenever allowPause is t
            if (allowPause == true)
            {
                // Check if "Left Alt" is pressed and if it is
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    // Flip the pausedTextOnOff to enable the text
                    pausedImageOnOff = !pausedImageOnOff;

                    // Test the paused text on/off
                    if (pausedImageOnOff == false)
                    {
                        // Unpause the countdown sound
                        countdownSound.UnPause();

                        // Unpause the game over sound
                        gameOverSound.UnPause();

                        // Disables the pause menu screen
                        _pauseMenuObject.SetActive(false);

                        // Unpause the entire game
                        Time.timeScale = 1;

                        // Enable the score text if it exists
                        if (GameObject.Find("Score Text") != null)
                        {
                            GameObject.Find("Score Text").gameObject.GetComponent<Text>().enabled = true;
                        }

                        // Don't show the return to main menu button
                        mainMenuButton.SetActive(false);
                    }

                    if (pausedImageOnOff)
                    {
                        // Select the button
                        StartCoroutine(SelectButtonLater());

                        // Pause the countdown sound
                        countdownSound.Pause();

                        // Pause the game over sound
                        gameOverSound.Pause();

                        // Pause the explosion sound on a prefab
                        if(GameObject.Find("Enemy_Explosion(Clone") != null)
                        {
                            GameObject.Find("Enemy_Explosion(Clone").GetComponent<AudioSource>().Pause();
                        }

                        // Remove the score text if it exists
                        if(GameObject.Find("Score Text") != null)
                        {
                            GameObject.Find("Score Text").gameObject.GetComponent<Text>().enabled = false;
                        }

                        // Enable the menu button
                        mainMenuButton.SetActive(true);

                        // Change position of the pause menu object at the start so that it doesn't appear instantly on the screen
                        _pauseMenuObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, -1000f, 0f);

                        // Enable the credits text
                        creditText.enabled = true;

                        // Displays the pause menu screen
                        _pauseMenuObject.SetActive(true);

                        // Activate the pause menu animation
                        _pauseAnimator.SetBool("escPressed", true);

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

        // Selects the main menu button after a frame
        IEnumerator SelectButtonLater()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(mainMenuButton.gameObject);
        }
    }
}