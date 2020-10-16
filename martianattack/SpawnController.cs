using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class SpawnController : MonoBehaviour
    {
        [SerializeField]
        // Variable to hold the enemy prefab
        private GameObject _enemySpritePrefab = null;

        [SerializeField]
        // Array variable to hold pickup sprites
        private GameObject[] _pickupArray = null;

        // Array just for the speedy bullet pickup
        [SerializeField]
        private GameObject _speedyBulletPrefab;

        // Variable for the movement speed of the enemy
        public float _enemyMovement = 0f;

        // Variable for pausing the _enemyMovement increase
        public bool gameIsPaused = false;

        // Time between spawns variable
        private float _timeBetweenSpawns = 10f;

        // Variable to only allow _decreaseRange to go down once
        private bool _triggerHealthPackBool = true;

        // Variable to increase the timer to make the health pack more rare
        private float _timerHealth;

        // Variable to trigger the health pack to spawn
        private float _triggerHealth;

        // Variable to increase and spawn the speedy bullet pickup
        private float _speedyBulletTimer;

        // Variable to decrease the range after 2 mins
        private int _decreaseRange = 4;

        // Use this for initialization
        void Start ()
        {
            // Starts the coroutines below
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(PickupSpawnRoutine());
        }

        private void Update()
        {
            // Change the _enemyMovement speed as time goes on by 0.001f
            if (GameManager.gameOver == false)
            {
                // Keep increasing this so that when the time comes
                // the _timerHealth can be used to check when 2 mins has passed
                _timerHealth += Time.deltaTime;

                // Start the _triggerHealth timer after 2 mins
                if(_triggerHealthPackBool == false)
                {
                    // Keep increasing this so that when the time comes
                    // the _triggerHealth can be used to check when 1 min has passed
                    // Do the same for the speedy bullet timer
                    _triggerHealth += Time.deltaTime;
                    _speedyBulletTimer += Time.deltaTime;
                }

                // Stop the health kit from spawning randomly
                if ((_timerHealth / 60) >= 2 && _triggerHealthPackBool)
                {
                    // Random.Range is inclusive
                    // Spawn an enemy on the X axis between the below ranges
                    float randomNumHealthCoord = Random.Range(0.07f, 0.93f);

                    // Code below is used to spawn pickups between specified ranges 
                    // despite resolutions

                    // Get the screen space
                    Vector3 xClamper = Camera.main.WorldToViewportPoint(transform.position);
                    // Clamp the x position of this game object between the values below according to the
                    // screen space
                    xClamper.x = Mathf.Clamp(xClamper.x, randomNumHealthCoord, randomNumHealthCoord);
                    // Set this game object to the clamped position always in the world
                    Vector3 viewportToWorld = Camera.main.ViewportToWorldPoint(xClamper);

                    // Make a copy of the health pack at a random x position
                    Instantiate(_pickupArray[3], new Vector3(viewportToWorld.x, 7, 0), Quaternion.identity);
                    // Decrease the range to prevent the health pack from spawning randomly
                    _decreaseRange--;
                    // Stop this code from executing again, no need as it cuts the health out of the random selection
                    _triggerHealthPackBool = false;
                }

                // If _speedyBulletTimer is greater than or equal to 0.9 of a minute
                if ((_speedyBulletTimer / 60) >= 0.9)
                {
                    // Random.Range is inclusive
                    // Spawn an enemy on the X axis between the below ranges
                    float randomNumSpeedyBulletCoord = Random.Range(0.07f, 0.93f);

                    // Get the screen space
                    Vector3 xClamper = Camera.main.WorldToViewportPoint(transform.position);
                    // Clamp the x position of this game object between the values below according to the
                    // screen space
                    xClamper.x = Mathf.Clamp(xClamper.x, randomNumSpeedyBulletCoord, randomNumSpeedyBulletCoord);
                    // Set this game object to the clamped position always in the world
                    Vector3 viewportToWorld = Camera.main.ViewportToWorldPoint(xClamper);

                    // Make a copy of the speed bullet pickup at a random x position
                    Instantiate(_speedyBulletPrefab, new Vector3(viewportToWorld.x, 7, 0), Quaternion.identity);
                    // Reset _triggerHealth so another spawn can happen after 1 min
                    _speedyBulletTimer = 0;
                }

                // If _triggerHealth is greater than or equal to 1 min
                if ((_triggerHealth / 60) >= 1)
                {
                    // Instantiate is used to make copies of the pickups whilst choosing random pickups
                    // from the array
                    // randomNumHealthCoord in this scope is used to get a random coord pos

                    // Random.Range is inclusive
                    // Spawn an enemy on the X axis between the below ranges
                    float randomNumHealthCoord = Random.Range(0.07f, 0.93f);

                    // Get the screen space
                    Vector3 xClamper = Camera.main.WorldToViewportPoint(transform.position);
                    // Clamp the x position of this game object between the values below according to the
                    // screen space
                    xClamper.x = Mathf.Clamp(xClamper.x, randomNumHealthCoord, randomNumHealthCoord);
                    // Set this game object to the clamped position always in the world
                    Vector3 viewportToWorld = Camera.main.ViewportToWorldPoint(xClamper);

                    // Make a copy of the health pack at a random x position
                    Instantiate(_pickupArray[3], new Vector3(viewportToWorld.x, 7, 0), Quaternion.identity);
                    // Reset _triggerHealth so another spawn can happen after 1 min
                    _triggerHealth = 0;
                }

                // Increase the _enemyMovement speed only if the game isn't paused
                if (gameIsPaused == false)
                {
                    // Increase the _enemyMovement by small fragments each Update()
                    _enemyMovement += 0.001f;
                }

                // Cap the _enemyMovement to 10f
                if (_enemyMovement >= 10f)
                {
                    _enemyMovement = 10f;
                }

                // Cap the _timeBetweenSpawns to 1f to prevent a bug-out
                if(_timeBetweenSpawns <= 1f)
                {
                    _timeBetweenSpawns = 1f;
                }
            }
            else
            { 
                // Reset the game as it was in the beginning and health spawning variables reset
                _timerHealth = 0;
                _triggerHealth = 0;
                _speedyBulletTimer = 0;
                _enemyMovement = 0f;
                _triggerHealthPackBool = true;
                _decreaseRange = 4;
                _timeBetweenSpawns = 10f;
            }
        }

        // Method to start the coroutines in this class again
        public void InitializeSpawnRoutines()
        {
            StartCoroutine(PickupSpawnRoutine());
            StartCoroutine(SpawnEnemyRoutine());
        }

        // Method to continually spawn enemies 
        public IEnumerator SpawnEnemyRoutine()
        {
            // Game loop to keep the enemies spawning
            while (GameManager.gameOver == false && gameIsPaused == false)
            {
                // Random.Range is inclusive
                // Spawn an enemy on the X axis between the below ranges
                float randomNumCoord = Random.Range(0.10f, 0.90f);

                // Get the screen space
                Vector3 xClamper = Camera.main.WorldToViewportPoint(transform.position);
                // Clamp the x position of this game object between the values below according to the
                // screen space
                xClamper.x = Mathf.Clamp(xClamper.x, randomNumCoord, randomNumCoord);
                // Set this game object to the clamped position always in the world
                Vector3 viewportToWorld = Camera.main.ViewportToWorldPoint(xClamper);

                // Instantiate is used to make copies of the enemies at random x positions
                // randomNum is placed into the X position of the vector to re-assign 
                // the X position
                GameObject enemyInstantiation = Instantiate(_enemySpritePrefab, new Vector3(viewportToWorld.x, 5.717694f, 0), Quaternion.identity);

                // Prevent sorting layer overlapping, random layers
                enemyInstantiation.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(-32768, 32767);

                // This decreases the time it takes to spawn an enemy by 0.9f
                // makes the game faster paced
                _timeBetweenSpawns *= .9f;

                // Clamp the WaitForSeconds value so that it doesn't become 0, it bugs out if it becomes 0
                yield return new WaitForSeconds(_timeBetweenSpawns);
            }
        }

        // Method to continually spawn pickups
        public IEnumerator PickupSpawnRoutine()
        {
            // Game loop to keep the pickups spawning
            while (GameManager.gameOver == false)
            {
                // Random.Range for ints is exclusive which means it will include 0, 1, 2 and 3 but not 4
                // Random.Range spits out a value between 0 and _decreaseRange (_decreaseRange decreases by 1)
                // to prevent health pack from spawning randomly after 2 mins
                int randomPickupSelector = Random.Range(0, _decreaseRange);

                // Random.Range is inclusive
                // Spawn an enemy on the X axis between the below ranges
                float randomPickupCoord = Random.Range(0.07f, 0.93f);

                // Get the screen space
                Vector3 xClamper = Camera.main.WorldToViewportPoint(transform.position);
                // Clamp the x position of this game object between the values below according to the
                // screen space
                xClamper.x = Mathf.Clamp(xClamper.x, randomPickupCoord, randomPickupCoord);
                // Set this game object to the clamped position always in the world
                Vector3 viewportToWorld = Camera.main.ViewportToWorldPoint(xClamper);

                // Make a copy of a pickup at a random x position
                Instantiate(_pickupArray[randomPickupSelector], new Vector3(viewportToWorld.x, 7, 0), Quaternion.identity);
                yield return new WaitForSeconds(5.0f);
            }
        }
    }
}