using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFO
{
    // This script will add a Rigidbody2D on an object
    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerController : MonoBehaviour
    {
        // Delegate and events that alert other scripts when the player
        // scores and dies
        public delegate void PlayerDelegate();
        public static event PlayerDelegate OnPlayerPoint;
        public static event PlayerDelegate OnPlayerDeath;

        // Variables to play sounds
        public AudioSource jumpAudio;
        public AudioSource deathAudio;
        public AudioSource pointAudio;
        public AudioSource gameOverAudio;

        // Prefab for the explosion
        public GameObject explosion;

        // Variables for the movement of the player such as rotation downwards
        // Force and smoothing which can be adjusted
        public float tiltSmoothing;
        public float tapForce;
        public Vector3 startPosition;

        // Both of these are used to do rotations on the game object
        // Quaternion is X, Y, Z, W (values from 0 to 1)
        private Quaternion forwardRotation;
        private Quaternion downRotation;

        // Reference to the GameManager 
        private GameManager gameManager;

        // Reference to the PauseGame script
        public PauseGame pauseGame;

        // This tells whether the player is dead or not
        private bool playerDead = false;

        void Start()
        {
            // Take a Vector3 and turn it into a quaternion

            // Face upwards a bit
            forwardRotation = Quaternion.Euler(0, 0, 35);
            // Face downwards
            downRotation = Quaternion.Euler(0, 0, -90);
            // Assign "gameManager"
            gameManager = GameManager.Instance;
            // Physics won't act on it
            GetComponent<Rigidbody2D>().simulated = false;
        }

        // Subscribe and unsubscribe events for the delegates

        private void OnEnable()
        {
            GameManager.OnGameStart += OnGameStart;
            GameManager.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameManager.OnGameStart -= OnGameStart;
            GameManager.OnGameOver -= OnGameOver;
        }

        void OnGameStart()
        {
            // When the game restarts reset the velocity to stop gravity from
            // acting on the object
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            // Object will now be acted on by the physics
            GetComponent<Rigidbody2D>().simulated = true;
        }

        void OnGameOver()
        {
            // Reset the location of the player
            transform.localPosition = startPosition;
            // Reset the rotation of the player (no rotation)
            transform.rotation = Quaternion.identity;
        }

        private void Update()
        {
            // If the player is dead and "LeftAlt" is pressed then they can exit the entire game
            if(playerDead)
            {
                if(Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    gameManager.ExitGame();
                }
            }

            if(gameManager.GameOver)
            {
                // Get out of the update i.e. don't do anything in update
                return;
            }
                
            // If "Space" key is pressed then
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Play a jump sound
                jumpAudio.Play();
                // Zeroes out the velocity to slow the object down
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                // Go to a forward rotation everytime you press "Space"
                transform.rotation = forwardRotation;
                // Push the player upwards a bit
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * tapForce, ForceMode2D.Force);
            }

            // Rotate the object downwards smoothly by lerping down to the downRotation with the
            // tiltSmoothing variable
            transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltSmoothing * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Dead Region")
            {
                // Don't let the player drop when the game starts

                GetComponent<Rigidbody2D>().simulated = false;
                // Register death

                // Event sent to GameManager
                OnPlayerDeath();

                // Create explosion
                Instantiate(explosion, transform.position, transform.rotation);

                // Make the sprite invisible
                gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0);

                // Play death sound
                deathAudio.Play();

                // Play game over sound
                gameOverAudio.Play();

                // Allow to restart after 3 seconds
                StartCoroutine(AllowRestart());

                // The player is now dead so they can exit the game if they wish
                playerDead = true;

                // Don't allow the player to pause the game
                pauseGame.allowPause = false;
            }

            // Do code dependent on the tag that "this" game object collided with
            if (collision.gameObject.tag == "Score Region")
            {
                // Register score

                // Event is now sent to alert the "GameManager" script
                OnPlayerPoint();

                // Play sound
                pointAudio.Play();
            }
        }
        
        IEnumerator AllowRestart()
        {
            // Allow the player to pause the game
            pauseGame.allowPause = true;

            yield return new WaitForSeconds(3f);

            // Allow the game to be restarted
            gameManager.gameRestartedStartedGM = true;

            // Don't allow the player to pause the game
            pauseGame.allowPause = false;
        }
    }
}