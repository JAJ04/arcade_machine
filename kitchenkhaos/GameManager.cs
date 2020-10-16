using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Catch
{
    // REFERENCE TUTORIAL: https://unity3d.com/learn/tutorials/topics/2d-game-creation/2d-catch-game-pt-1

    public class GameManager : MonoBehaviour
    {
        // Reference to the theme sound
        public AudioSource themeSound;

        // Reference to the sound to play when the time is up
        public AudioSource timesUp;

        // Reference to the back sound to play
        public AudioSource backSound;

        // Reference to the sound to play when the game is over
        public AudioSource gameOverSound;

        // Reference to the ScoreTracker script
        public ScoreTracker scoreTracker;

        // This is a GameObject array that stores all of the catch objects which
        // can be assigned in the inspector
        public GameObject[] catchObjects;

        // Float that stores the time that the game has left
        public float timer;

        // Allows the timer to be decreased
        private bool _timerDecrease;
        // This stores the max width of the screen
        private float _maxWidthScreen;

        // Reference to the UI text that stores the
        // time in the text with the shadow
        public Text timerText;
        public Text timerTextShadow;

        // Reference to the player properties
        public PlayerController playerController;

        // Reference to the PauseGame script
        public PauseGame pauseGameScript;

        // GameObject variables that store the game over text, start button etc
        public GameObject startBtn;
        public GameObject gameOverText;
        public GameObject gameOverTextShadow;
        public GameObject restartBtn;
        public GameObject splashScrn;

        // This will allow the player to go back to the god menu when the game is over
        private bool goToGodMenu = false;
        // This will allow the back sound to only play once 
        private bool soundPlayOnce = true;
        // Play the time's up sound only once
        private bool timesUpOnce = true;

        // Use this for initialization
        void Start()
        {
            // Get the upper corner of the game screen using Screen.width and Screen.height
            Vector3 topLeftCorner = new Vector3(Screen.width, Screen.height, 0.0f);
            // Gets the width of the screen despite resolutions
            Vector3 screenWidth = Camera.main.ScreenToWorldPoint(topLeftCorner);

            // Gets the width of each catch object
            float catchObjectWidth = catchObjects[0].GetComponent<Renderer>().bounds.extents.x;

            // Gets the max width of the screen for the objects
            _maxWidthScreen = screenWidth.x - catchObjectWidth;
            // Changes the timer text as it is in the beginning and the shadow
            timerText.text = Mathf.RoundToInt(timer).ToString();
            timerTextShadow.text = Mathf.RoundToInt(timer).ToString();
        }

        private void Update()
        {
            if(goToGodMenu)
            {
                // Go back to main menu if the game is over and left alt is pressed
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    StartCoroutine(GoToGodMenu());
                }
            }
        }

        void FixedUpdate()
        {
            // If counting is true then
            if (_timerDecrease)
            {
                // Decrease the timer variable by Time.deltaTime
                timer -= Time.deltaTime;

                // If timer ever becomes < 0 then set it to 0
                if (timer < 0)
                {
                    timer = 0;

                    if(timesUpOnce)
                    {
                        // Play the sound for when the time is up
                        timesUp.Play();
                        // Don't play it again
                        timesUpOnce = false;
                    }

                    // Set it so you cannot move
                    playerController.PlayerMove(false);
                }

                // Update the timer text and the shadow
                timerText.text = Mathf.RoundToInt(timer).ToString();
                timerTextShadow.text = Mathf.RoundToInt(timer).ToString();
            }
        }

        // This disables the appropriate game objects and allows the catch objects to be spawned
        public void StartGame()
        {
            splashScrn.SetActive(false);
            startBtn.SetActive(false);
            playerController.PlayerMove(true);
            StartCoroutine(SpawnRoutine());
            pauseGameScript.gameStarted = true;
        }

        public IEnumerator SpawnRoutine()
        {
            // Starts the rest of the code after 2 seconds
            yield return new WaitForSeconds(1.0f);

            // The timer can now decrease
            _timerDecrease = true;

            // If the timer is > 0 then spawn catch objects from the catch objects array
            while (timer > 0)
            {
                // Get a random catch object from the catchObjects array and store it in "catchObject"
                GameObject catchObject = catchObjects[Random.Range(0, catchObjects.Length)];

                // Creates a spawn position which is inside the range of the maxWidth and at the top
                Vector3 spawnPosition = new Vector3(
                    transform.position.x + Random.Range(-_maxWidthScreen, _maxWidthScreen),
                    transform.position.y,
                    0.0f
                );

                // No rotation on the game object you're spawning
                Quaternion spawnRotation = Quaternion.identity;

                // Instantiate the catch object at the spawn position with no rotation
                Instantiate(catchObject, spawnPosition, spawnRotation);

                // Spawn a new object between 1 and 2 seconds
                yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
            }

            // When the timer is < 0 then show the game over text and the restart button
            // after 2 seconds
            yield return new WaitForSeconds(2.0f);
            gameOverText.SetActive(true);
            gameOverTextShadow.SetActive(true);

            // Play the game over sound
            gameOverSound.Play();

            // Turn off the theme song
            themeSound.Stop();

            // Allow the player to go to the god menu
            goToGodMenu = true;

            // Game is no longer in play
            pauseGameScript.gameStarted = false;

            yield return new WaitForSeconds(2.0f);
            restartBtn.SetActive(true);
        }

        IEnumerator GoToGodMenu()
        {
            // Play a sound (only once) and go back to the god menu after 0.25f seconds
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