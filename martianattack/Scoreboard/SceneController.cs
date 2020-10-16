using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MartianAttack
{
    public class SceneController : MonoBehaviour
    {
        // Text to display info
        public Text _timeRemainingText;

        // Text of the input field
        public Text _playerNameChild;

        // Reference to the NAUGHTY text
        public Text naughtyText;

        // Reference to the credits text
        public Text creditsText;

        // Input field to get the username
        public InputField _playerName;

        // Variable to hold the pause menu
        public static GameObject pauseMenuObject;

        // If the game is alive then allow
        // space presses
        private bool _gameInPlay = true;

        // Variable to decrease the time
        private int _timeRemaining = 30;

        // Bool for the pause menu enable/disable
        public static bool pauseMenuActive = false;

        // Allow the timer associated with the pause scene enabling to count down
        public bool pauseCountdown = false;

        // AudioClip to play credits sound
        [SerializeField]
        private AudioClip _coinClip;

        // AudioClip to play beep sound
        [SerializeField]
        private AudioClip _beepSound;

        // AudioClip to play beep text name confirmed sound
        [SerializeField]
        private AudioClip _beepNameConfirmedSound;

        // Variable to hold the animation for the pause menu
        private Animator _pauseAnimator;

        // Button for going back to the main menu
        [SerializeField]
        private GameObject _menuButton;

        // Reference to the LOGO image
        public Image logoImage;

        // Use this for initialization
        void Start()
        {
            // Set volume in this scene
            AudioListener.volume = 1f;

            // Set pauseMenuObject as the Pause_Menu_Panel in the scene
            pauseMenuObject = GameObject.Find("Pause_Menu_Panel");

            // Set the _pauseAnimator variable to the script on the "Pause_Menu_Panel" object
            _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();

            // Turn on Selected_Character text and Username text
            ScrollManager._spacePressedCounter = 0;

            // Make the _pauseAnimator disregard Time.timeScale freezing
            _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;

            // Reset the _timeRemaining
            _timeRemaining = 30;

            // Start decreasing counter
            StartCoroutine(OneSecond());

            // Set max size of input field
            _playerName.characterLimit = 3;
        }

        // Update is called once per frame
        void Update()
        {
            // If "ESC" key is pressed then enable pause menu/disable pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // Change position of it at the start so that it doesn't appear instantly on the screen
                pauseMenuObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, -1000f, 0f);

                // Enable the menu button
                _menuButton.SetActive(true);

                if (pauseMenuActive == false)
                {
                    // Displays the pause menu screen
                    pauseMenuObject.SetActive(true);

                    // Select the menu button
                    StartCoroutine(SelectButtonLater());

                    // Activate the pause menu animation
                    _pauseAnimator.SetBool("escPressed", true);

                    // Time.timeScale completely pauses the game
                    Time.timeScale = 0;

                    // Bool used to have a toggle between switching the 
                    // pause menu on and off
                    pauseMenuActive = true;

                    // Disable the logo for the logo on the pause splash to not overlap with the logo persistent on 
                    // the scoreboard
                    GameObject.Find("Logo_Transparent").GetComponent<Image>().enabled = false;
                }
                else
                {
                    // Enable the logo for the logo to show again when the pause splash has gone
                    GameObject.Find("Logo_Transparent").GetComponent<Image>().enabled = true;

                    // Disable the menu button
                    _menuButton.SetActive(false);

                    // Disables the pause menu screen
                    pauseMenuObject.SetActive(false);

                    // Below code unpauses the game
                    Time.timeScale = 1;

                    pauseMenuActive = false;
                }
            }

            // Allow the game to be unpaused to switch scenes if "Space" is pressed when the game is paused
            if (pauseMenuActive)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Time.timeScale = 1f;
                }
            }

            // Reset the state
            if (GameManager.resetScoreBoard)
            {
                _timeRemainingText.GetComponent<Text>().enabled = true;
                _timeRemaining = 30;
                _playerName.GetComponent<Image>().enabled = true;
                _playerNameChild.GetComponent<Text>().enabled = true;
                ScrollManager._spacePressedCounter = 0;
                GameManager.resetScoreBoard = false;
            }

            // Confirm the entry in the text box once all 3 have been entered
            if (ScrollManager._spacePressedCounter > 2)
            {
                InitialsEntered();
            }

            // If "F" is pressed end the game and add the initials into the leaderboard
            if(Input.GetKeyDown(KeyCode.LeftAlt) && pauseMenuActive == false)
            {
                InitialsEntered();
                EndGame();
            }

            // If the game is not in play
            if (!_gameInPlay)
            {
                // No longer call update
                return;
            }
        }

        public void InitialsEntered()
        {
            // Check the high score and change scene after 2 secs
            if (_playerName.text == " ")
            {
                // Update high score and proceed to the main game screen again, this is if nothing is
                // entered into the input
                GetComponent<LeaderBoard>().CheckForHighScore(UIController.score, "NO NAME", UIController.bossSpaceshipCounter);

                // Play a beep name confirmed sound
                AudioSource.PlayClipAtPoint(_beepNameConfirmedSound, Camera.main.transform.position, 1f);

                // Switch back to the main scene
                SceneManager.LoadScene("game");
            }
            
            if(_playerName.text != " ")
            {
                // Censor list
                if (_playerName.text == "SEX" || _playerName.text == "ASS" || _playerName.text == "BUM" ||
               _playerName.text == "ASS" || _playerName.text == "COK" || _playerName.text == "DIK" ||
               _playerName.text == "SHT" || _playerName.text == "CNT" || _playerName.text == "POO" ||
               _playerName.text == "PSS" || _playerName.text == "PEE" || _playerName.text == "DIE" ||
               _playerName.text == "FUK" || _playerName.text == "LIK" || _playerName.text == "PSY" ||
               _playerName.text == "BUT" || _playerName.text == "KOK" || _playerName.text == "TIT" ||
               _playerName.text == "JIZ" || _playerName.text == "BCH" || _playerName.text == "CUM" ||
               _playerName.text == "KNT" || _playerName.text == "KKK" || _playerName.text == "SUK"
               || _playerName.text == "SUC")
                {
                    // Show the naughty text, disable the logo and reload the scene
                    logoImage.enabled = false;
                    naughtyText.enabled = true;

                    // Play a beep sound
                    AudioSource.PlayClipAtPoint(_beepSound, Camera.main.transform.position, 1f);

                    // Load scoreboard
                    SceneManager.LoadScene("scoreboard");
                }
                else
                {
                    // Play a beep name confirmed sound
                    AudioSource.PlayClipAtPoint(_beepNameConfirmedSound, Camera.main.transform.position, 1f);

                    // Update high score and proceed to the main game screen again
                    GetComponent<LeaderBoard>().CheckForHighScore(UIController.score, _playerName.text, UIController.bossSpaceshipCounter);

                    // Switch back to the main scene
                    SceneManager.LoadScene("game");
                }
            }

            // Disable _timeRemainingText
            _timeRemainingText.GetComponent<Text>().enabled = false;
            // Disable input field
            _playerName.GetComponent<Image>().enabled = false;
            _playerNameChild.GetComponent<Text>().enabled = false;

            // Disable credits text
            creditsText.enabled = false;
        }

        // Decrease _timeRemaining
        IEnumerator OneSecond()
        {
            while (true)
            {
                // Keep on decreasing the time
                yield return new WaitForSeconds(1.0f);

                _timeRemaining--;
                _timeRemainingText.text = "Time to enter score: " + _timeRemaining;

                // Give some time to save the username
                if (_timeRemaining <= 0.5f)
                {
                    // Save the username
                    InitialsEntered();
                }

                // End the game and break out of this loop
                // to prevent decrementing further
                if (_timeRemaining <= 0)
                {
                    EndGame();
                    break;
                }
            }
        }

        void EndGame()
        {
            // Game is no longer alive
            _gameInPlay = false;
            // Turn off Selected_Character text and Username text
            GameObject.Find("Selected_Character").SetActive(false);
            GameObject.Find("Username").SetActive(false);
            // Switch back to the main scene
            SceneManager.LoadScene("game");
        }

        public void PlayCoin()
        {
            // Start Credit Sound Clip
            float lastTimeScale = Time.timeScale;
            Time.timeScale = 1f;
            AudioSource.PlayClipAtPoint(_coinClip, Camera.main.transform.position, 1f);
            Time.timeScale = lastTimeScale;
        }

        // Waits a frame and selects the button
        IEnumerator SelectButtonLater()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_menuButton.gameObject);
        }
    }
}