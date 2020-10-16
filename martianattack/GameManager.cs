using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MartianAttack
{
    public class GameManager : MonoBehaviour
    {
        // Variable to hold the player
        [SerializeField]
        private GameObject _playerObject;
        // Prefab to instantiate the boss enemy
        [SerializeField]
        private GameObject _bossEnemy;
        // Variable to hold the pause menu
        [SerializeField]
        private GameObject _pauseMenuObject;
        // Button for going back to the main menu
        [SerializeField]
        private GameObject _menuButton;

        // Array of game objects to delete them when game is over
        private GameObject[] _enemyGameObjects;

        // Variable for deleting the alien spaceships when game is over
        GameObject _bossSpaceship;

        // Variable to hold the player in the scene
        private Player _playerObjectInScene;

        // Variable for the initialization of the boss
        public static int _bossSpawner = 600;
        // Variable for the range clamping
        public static int _bossSpawner2 = 800;
        // Variable for the _timeRemaining in the scoreboard scene
        public static int _timeRemaining = 30;

        // Bool to reset the scoreboard screen
        public static bool resetScoreBoard = true;
        // Bool variable to check if gameOver is true
        public static bool gameOver = true;

        // Bool variable to allow the OneShot to play only once
        public bool audioPlayOnce = true;
        // Bool variable to allow the OneShot to play only once, Game Over
        public bool audioPlayOnceGameOver = true;
        // Is it the first game?
        public bool firstGame = false;
        // Bool for the pause menu enable/disable
        public bool pauseMenuActive = false;
        // Bool to disable pausing during countdown screen
        public bool countdownScreen = true;
        // Allow the timer associated with the pause scene enabling to count down
        public bool pauseCountdown = false;
        // Game started variable to allow the user to start a new game
        public bool gameStarted = false;

        // Variable for only generating a random number once
        private bool _generateOnce = false;
        // Variable for only generating a random number once in the if statement after the game is started
        private bool _generateOnceGameStarted = false;
        // Variable to only allow the sound to play once
        private bool soundPlayOnce = true;
        // Reference to allow the player to go back to the god menu when they die
        private bool goBackToGodMenuBool;

        // Variable for the timer of the space cool down to prevent spamming a new game, 5 seconds
        public float timeLeft = 7f;

        // Variable for the timer of the pause menu enabler to prevent pause whilst startup sound is played
        public float timeLeftPause = 2f;

        // Variable to hold the random number for spawning a boss
        private float _randomBossNumber;

        // Variable for UIController script communication
        private UIController _uiController;

        // Background music source
        private AudioSource _audioSource;
        // The audio clip to play
        [SerializeField]
        private AudioClip _countDownClip;
        // Game over clip
        [SerializeField]
        private AudioClip _gameOverClip;
        // Start game clip
        [SerializeField]
        private AudioClip _startGameClip;

        // Audio Source reference for the back sound
        public AudioSource backSound;

        // Variable to hold the animation for the pause menu
        private Animator _pauseAnimator;

        // Get the _spawnController script to set the bool for
        // pausing the _enemyMovement
        private SpawnController _spawnController;

        // Timer for when to switch the scene
        private float _timer;

        private void Start()
        {
            //Set Cursor to not be visible
            Cursor.visible = false;

            // Initialize the _uiController variable
            _uiController = GameObject.Find("Canvas").GetComponent<UIController>();

            // Initialize the _spawnController variable
            _spawnController = GameObject.Find("Spawn_Controller").GetComponent<SpawnController>();

            // Get audio source on this object
            _audioSource = GetComponent<AudioSource>();

            // Change global audio setting
            AudioListener.volume = 1f;

            // Set the _pauseAnimator variable to the script on the "Pause_Menu_Panel" object
            _pauseAnimator = GameObject.Find("Pause_Menu_Panel").GetComponent<Animator>();

            // Make the _pauseAnimator disregard Time.timeScale freezing
            _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }

        private void Update()
        {
            // Allow the game to be unpaused to switch scenes if "Space" is pressed when the game is paused
            if(pauseMenuActive)
            {
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    Time.timeScale = 1f;
                }
            }

            // If this bool is true then you can go back to the god menu when "LeftAlt" is pressed
            if(goBackToGodMenuBool)
            {
                if(Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    StartCoroutine(GoBackToGodMenu());
                }
            }

            // Start increasing the timer
            if (firstGame == true)
            {
                _timer += Time.deltaTime;
            }

            // Only generate random number once when the game has started
            if(_generateOnceGameStarted)
            {
                // This is to only allow one random number to be generated
                if (_generateOnce == false)
                {
                    _randomBossNumber = Random.Range(_bossSpawner, _bossSpawner2);
                    _generateOnce = true;
                }
            }

            // Spawn an enemy and generate a new range when score is greater than random number
            if (UIController.score > _randomBossNumber)
            {
                // Generate new values for _bossSpawner and _bossSpawner2 to make it more random
                // and for there to be a range
                _bossSpawner = _bossSpawner * 2;
                _bossSpawner2 = _bossSpawner2 * 2;
                _generateOnce = false;

                // Code below is used to spawn pickups between specified ranges 
                // despite resolutions

                // Get the screen space
                Vector3 xClamper = Camera.main.WorldToViewportPoint(transform.position);
                // Clamp the x position of this game object between the values below according to the
                // screen space (this will spawn the boss outside of the screen so it flies in)
                xClamper.x = Mathf.Clamp(xClamper.x, -0.17f, -0.17f);
                // Set this game object to the clamped position always in the world
                Vector3 viewportToWorld = Camera.main.ViewportToWorldPoint(xClamper);

                // Make a copy of the prefab
                Instantiate(_bossEnemy, new Vector3(viewportToWorld.x, 2.35f, 0f), Quaternion.identity);
            }

            // If the game has ended
            if (gameOver == true)
            {
                // If the countdown screen is showing
                if(GameObject.Find("Main_Screen_5") || GameObject.Find("Main_Screen_4")
                    || GameObject.Find("Main_Screen_3") || GameObject.Find("Main_Screen_2")
                    || GameObject.Find("Main_Screen_1") || GameObject.Find("Main_Screen_0"))
                {
                    // Go back to god menu
                    if (Input.GetKeyDown(KeyCode.LeftAlt))
                    {
                        StartCoroutine(GoBackToGodMenu());
                    }
                }

                // Don't generate any new numbers for boss spawning when the game is over
                _generateOnceGameStarted = false;

                // Reset the scoreboard
                resetScoreBoard = true;

                // Delete all instances of the boss
                _bossSpaceship = GameObject.Find("Special_Spaceship(Clone)");
                Destroy(_bossSpaceship);

                // Reset values
                _bossSpawner = 600;
                _bossSpawner2 = 800;

                // After first game has ended
                if (firstGame == true)
                {
                    // Turn off the countdown animation with the martian
                    // Do the opposite of the "ShowMainScreen()" essentially
                    _uiController.mainScreen.gameObject.SetActive(false);
                    _uiController.lastAlien.gameObject.SetActive(false);

                    // Bool to set countdown screen to false 
                    UIController.countdownScreen = false;

                    // Show Game Over text
                    GameObject.Find("Game_Over_Text").GetComponent<Text>().enabled = true;

                    // Allow exit
                    goBackToGodMenuBool = true;

                    // Switch the scene after a second
                    if (_timer >= 4.0f)
                    {
                        // Change to scoreboard
                        SceneManager.LoadScene("scoreboard");
                    }
                }           

                // Stop background music
                _audioSource.Stop();

                // Initialize enemyGameObjects array
                _enemyGameObjects = FindObjectsOfType<GameObject>();

                // Find all enemy instantiations and delete them
                if (_enemyGameObjects != null)
                {
                    for (int i = 0; i < _enemyGameObjects.Length; i++)
                    {
                        if (_enemyGameObjects[i].name == "Enemy(Clone)")
                        {
                            Destroy(_enemyGameObjects[i]);
                        }
                    }
                }

                // Play the audio once
                if (audioPlayOnceGameOver == true && firstGame == true)
                {
                    // Play game over clip
                    AudioSource.PlayClipAtPoint(_gameOverClip, Camera.main.transform.position);
                    audioPlayOnceGameOver = false;
                }

                // Play the audio once
                if (audioPlayOnce == true && firstGame == false)
                {
                    AudioSource.PlayClipAtPoint(_countDownClip, Camera.main.transform.position, 1f);
                    audioPlayOnce = false;
                }

                // Decreases every time update is called
                timeLeft -= Time.deltaTime;

                // If space key is pressed and timeLeft < 0
                if (Input.GetKeyDown(KeyCode.Space) && timeLeft < 0 && CheatCodes.credits > 0)
                {
                    // Decrease credit by one
                    CheatCodes.credits--;

                    // Reset boss spaceship counter
                    if(UIController.bossSpaceshipCounter > 0)
                    {
                        UIController.bossSpaceshipCounter = 0;
                    }

                    pauseCountdown = true;

                    gameOver = false;
                    // Instantiate (make a copy of) the player object in the center
                    Instantiate(_playerObject, Vector3.zero, Quaternion.identity);
                    // Game has restarted so hide the title screen
                    _uiController.HideMainScreen();

                    // Start Game Sound Clip
                    AudioSource.PlayClipAtPoint(_startGameClip, Camera.main.transform.position, 1f);

                    // Play background music
                    _audioSource.Play();
                }
            }
            else
            {
                // Allow only one new random number to be generated when the game has started again
                _generateOnceGameStarted = true;
            }

            // This gets reset to true every time the game is over
            if (pauseCountdown == true)
            {
                // Decreases every time update is called
                timeLeftPause -= Time.deltaTime;
            }

            // If "ESC" key is pressed then enable pause menu/disable pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                // If the pause menu screen is not enabled and the time to first 
                // enable the pause menu has passed then
                if (UIController.countdownScreen == false && timeLeftPause < 0)
                {
                    // Change position of it at the start so that it doesn't appear instantly on the screen
                    _pauseMenuObject.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0f, -1000f, 0f);

                    // Enable the menu button
                    _menuButton.SetActive(true);

                    // Selects the main menu button
                    StartCoroutine(SelectButtonLater());

                    if (pauseMenuActive == false)
                    {
                        // Show the credits text
                        GameObject.Find("Credits_Text").GetComponent<Text>().enabled = true;

                        // Displays the pause menu screen
                        _pauseMenuObject.SetActive(true);

                        // Set _playerObjectInScene to the object in the scene
                        _playerObjectInScene = GameObject.Find("Player(Clone)").GetComponent<Player>();

                        // Disable the player script so you can't move or do anything
                        _playerObjectInScene.enabled = false;

                        // Activate the pause menu animation
                        _pauseAnimator.SetBool("escPressed", true);

                        // Time.timeScale completely pauses the game
                        Time.timeScale = 0;

                        // Bool used to have a toggle between switching the 
                        // pause menu on and off
                        pauseMenuActive = true;

                        // Pause the movement increase
                        _spawnController.gameIsPaused = true;
                    }
                    else
                    {
                        // Disable the menu button
                        _menuButton.SetActive(false);

                        // Enable the player script so you can move and do stuff
                        _playerObjectInScene.enabled = true;

                        // Disables the pause menu screen
                        _pauseMenuObject.SetActive(false);

                        // Below code unpauses the game
                        Time.timeScale = 1;

                        // Unpause the movement increase
                        _spawnController.gameIsPaused = false;

                        pauseMenuActive = false;
                    }
                }
            }         

            /* OLD CODE FOR STARTING A NEW GAME WHILST PLAYING IS TRUE 
            // If "Space" key is pressed then restart game/reload scene and ask the user
            // when game is running
            // Use timeLeftPause (the variable to allow when to unpause the game) to prevent
            // the game from restarting when they press space immediately to start the game
            if (Input.GetKeyDown(KeyCode.Space) && gameOver == false && timeLeftPause < 0f)
            {
                // Ask the user if they are sure about restarting a game
                // If "YES" then
                if()
                {
                    // Reload scene
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.name);
                    // Take away a credit
                    credits--;
                }
                // Remove the prompt and do nothing else
                else
                {

                }
            } */

            // If "M" key is pressed then mute/unmute game
            /*if (Input.GetKeyDown(KeyCode.M))
            {
                if (muteToggle == false)
                {
                    // Mute game
                    AudioListener.volume = 0;
                    muteToggle = true;

                    // Show mute icon
                    _muteIcon.SetActive(true);
                }
                else
                {
                    // Unmute game
                    AudioListener.volume = 0.5f;
                    muteToggle = false;

                    // Disable mute icon
                    _muteIcon.SetActive(false);
                }
            } */
        }

        // Selects the main menu button after a frame
        IEnumerator SelectButtonLater()
        {
            yield return null;
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_menuButton.gameObject);
        }

        IEnumerator GoBackToGodMenu()
        {
            // Play a sound (only once) and switch scenes after 0.25f seconds
            if(soundPlayOnce)
            {
                backSound.Play();
                soundPlayOnce = false;
            }

            yield return new WaitForSeconds(0.25f);

            SceneManager.LoadScene("godMenu");
        }
    }
}