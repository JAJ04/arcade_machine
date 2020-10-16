using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MartianAttack;

// REFERENCE: https://www.youtube.com/watch?v=A-GkNM8M5p8&t=4905s

namespace UFO
{
    public class GameManager : MonoBehaviour
    {
        // Delegate is used to notify other scripts of what is happening in this class (events)
        public delegate void GameDelegate();

        // A delegate is an event, these will be used to notify other scripts and are
        // named appropriately

        // Delegates can also be imagined as "function pointers"
        public static event GameDelegate OnGameStart;
        public static event GameDelegate OnGameOver;

        // Creates a static variable that allows access to the properties of this class
        public static GameManager Instance;

        // Reference to the score text on the screen
        public Text scoreText;

        // References to game objects in the scene
        public GameObject startGroup;
        public GameObject countdownGroup;
        public GameObject gameOverGroup;

        // Reference to the audio for the theme song
        public AudioSource themeAudio;
        // Reference to the audio for the back sound
        public AudioSource backSound;

        // Reference to the audio for the start sound
        public AudioSource startAudio;

        // Reference to the player game object
        public GameObject player;

        // Reference to the pause game script
        public PauseGame pauseGame;

        // Stores a variety of game states
        enum GameStates
        {
            None,
            Start,
            GameOver,
            Countdown
        };

        // Sets the score to 0 at the start
        private int score = 0;

        // Used to get the score variable
        public int Score { get { return score; } }

        // Game is not playing (or is false) at the start
        private bool gameOver = true;
        // Only allow the sound to play once to go back to the main menu
        private bool soundPlayOnce = true;

        // Other scripts can get the gameOver variable but cannot set anything
        public bool GameOver { get { return gameOver; } }
        // Bool to start the game with the "Space" key
        public bool gameStartedGM = false;
        // Bool to start the game with the "Space" key
        public bool gameRestartedStartedGM = false;

        // Reference to the pause game script
        public PauseGame pauseGameScript;

        // Assign the variable "Instance" to "THIS" class
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            // Go back to the main menu if the game has not been started
            if(gameStartedGM == false)
            {
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    StartCoroutine(GoBackToGodMenu());
                }
            }

            // Start the game if gameStartedGM is false

            // This is false at the very start of the script beginning
            if(gameStartedGM == false)
            {
                // If the credits is greater than 0 then you can start a new game
                if(CheatCodes.credits > 0)
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        // Play the start sound
                        startAudio.Play();

                        // Decrease a credit
                        CheatCodes.credits--;

                        // Prevent spamming of the "Space" key that confuses the game
                        gameStartedGM = true;
                        StartGame();
                    }
                }
            }

            // Restart the game if you are allowed to
            if(gameRestartedStartedGM == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    // Prevent spamming of the "Space" key that confuses the game
                    gameRestartedStartedGM = false;
                    // Allow the game to be started again
                    gameStartedGM = false;
                    // Confirm that the game is over
                    GameOverConfirmed();
                }
            }
        }

        // Subscribe/Unsubscribe delegates

        // When this game object is enabled then...
        void OnEnable()
        {
            // These will be ran when TapController fires events "OnPlayerScored"
            // and "OnPlayerDeath"
            PlayerController.OnPlayerPoint += OnPlayerPoint;
            PlayerController.OnPlayerDeath += OnPlayerDeath;

            // Call the OnCountdownFinish in this class if the event in
            // CountdownTextScript is activated
            CountdownTextScript.OnCountdownFinish += OnCountdownFinish;
        }

        // When this game object is disabled then...
        private void OnDisable()
        {
            // These will be unsubscribed when the object is disabled
            // so they will never be ran
            PlayerController.OnPlayerPoint -= OnPlayerPoint;
            PlayerController.OnPlayerDeath -= OnPlayerDeath;

            // Don't call the OnCountdownFinish in this class if the event in
            // CountdownTextScript is activated
            CountdownTextScript.OnCountdownFinish -= OnCountdownFinish;
        }

        // This will be accessed from the player class
        public void ExitGame()
        {
            StartCoroutine(GoBackToGodMenu());
        }

        // Set the states of the game with a parameter passed in
        void SetGroupState(GameStates state)
        {
            // Switch state is the same as an "IF" statement
            switch (state)
            {
                // If GameStates is none then do the following etc
                case GameStates.None:
                    startGroup.SetActive(false);
                    gameOverGroup.SetActive(false);
                    countdownGroup.SetActive(false);
                    break;
                case GameStates.Countdown:
                    startGroup.SetActive(false);
                    gameOverGroup.SetActive(false);
                    countdownGroup.SetActive(true);
                    break;
                case GameStates.Start:
                    startGroup.SetActive(true);
                    gameOverGroup.SetActive(false);
                    countdownGroup.SetActive(false);

                    // Make the sprite visible
                    player.gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                    // Stop the game over sound
                    player.GetComponent<PlayerController>().gameOverAudio.Stop();
                    break;
                case GameStates.GameOver:
                    startGroup.SetActive(false);
                    gameOverGroup.SetActive(true);
                    countdownGroup.SetActive(false);

                    // Stop the theme song
                    themeAudio.Stop();
                    break;
            }
        }

        void OnPlayerPoint()
        {
            // Increment the score by 1
            score++;
            // Update the score text on the screen
            scoreText.text = score.ToString();
        }

        void OnPlayerDeath()
        {
            // If the player is dead the game is now over
            gameOver = true;

            // Retrieve the hiscore value from the playerprefs
            int hiscore = PlayerPrefs.GetInt("UFO Hiscore");

            // Check for a new hiscore
            if (score > hiscore)
            {
                // If score > hiscore then save it to PlayerPrefs
                PlayerPrefs.SetInt("UFO Hiscore", score);
            }

            // The game is now over (state form)
            SetGroupState(GameStates.GameOver);
        }

        void OnCountdownFinish()
        {
            // The state of the game is nothing
            SetGroupState(GameStates.None);

            // Throws events to other scripts

            // Event sent to PlayerController
            OnGameStart();

            // Reset game and set the game over to false
            score = 0;
            gameOver = false;

            // Play the theme song when the countdown is over
            themeAudio.Play();
        }

        public void StartGame()
        {
            // Activated when play button is hit

            // Countdown page will show up
            SetGroupState(GameStates.Countdown);

            // Pause the countdown audio

            // Allow the game to be paused 
            pauseGame.allowPause = true;
        }

        public void GameOverConfirmed()
        {
            // Activated when replay button is hit

            // Resets the objects

            // This event is sent to the PlayerController
            OnGameOver();
            // Reset the score
            scoreText.text = "0";
            // You may now start the game
            SetGroupState(GameStates.Start);

            if(GameObject.Find("Enemy_Explosion(Clone)") != null)
            {
                // Don't show any instances of an explosion
                GameObject.Find("Enemy_Explosion(Clone)").SetActive(false);
            }

            // Don't allow the game to be paused
            pauseGameScript.gameStarted = false;
        }

        // Play a sound and go back to the god menu
        IEnumerator GoBackToGodMenu()
        {
            // Only allow the sound to play once
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