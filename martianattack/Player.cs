using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class Player : MonoBehaviour
    {
        // canShield starts off at false and can be changed when the shield is collected
        public bool canShield = false;
        // canSpeedUp starts off at false and can be changed when a speed pickup is picked up
        public bool canSpeedUp = false;
        // canDoubleShot starts off as false because there is no pickup
        public bool canDoubleShot = false;
        // This is used to make sure that a random num only happens once
        public bool setRandomNumOnce = true;
        // Default the position when in idle
        public bool idleOn = true;
        // Bool to play the shield down when hit by an enemy with the shield on 
        public bool playShieldDownClip = false;
        // Bool to set when fast shoot is true/false
        public bool fastShoot = false;

        // This is used to stop and start lerping
        private bool throb = false;
        // This is used to start the backwards lerp
        private bool throbEnd = false;

        // Variable for the amount of lives
        public int lives = 3;

        // _bulletPrefab stores the prefab that will be used to instantiate a sprite
        // SerializeField allows the variable to be private, but it can be edited in
        // the Unity inspector
        [SerializeField]
        private GameObject _bulletPrefab;
        // Variable to hold the explosion prefab for the player
        [SerializeField]
        private GameObject _explosionPlayerPrefab;
        // Stores the double shot prefab which has a parent and two children
        // to show the double shot on the screen
        [SerializeField]
        private GameObject _doubleShotPrefab;
        // Variable to hold the shield prefab
        [SerializeField]
        private GameObject _shieldSprite;

        // Array for the engine sprites
        public GameObject[] _engineSprites;

        // Hit counter to know when to trigger the engine sprites
        private int _hitCounter = 0;

        // Variable to hold first/second engine failures
        private int _firstEngineFailure;
        private int _secondEngineFailure;

        // Variable to hold the height of the divided screen
        private float _halfHeight;
        // Random number variable
        private float _randomNum = 0f;

        // AudioClip to play the single bullet
        [SerializeField]
        private AudioClip _singleShotClip;

        // AudioClip to play the double shot
        [SerializeField]
        private AudioClip _doubleShotClip;

        // AudioClip to play the shield down
        [SerializeField]
        private AudioClip _shieldDownClip;

        // AudioClip to play the engine failure down
        [SerializeField]
        private AudioClip _engineFailureClip;

        // AudioClip to play the hit sound effect
        [SerializeField]
        public AudioClip _hitClip;

        // AudioClip to play the hit sound effect
        [SerializeField]
        public AudioClip _playerExplosionSound;

        // Used to set the speed that the player will move
        [SerializeField]
        private float _movementSpeed = 5f;

        // Is used as a timer for the single bullet to check when the player can fire again
        private float _fireAllowed = 0.0f;
        // Is used as a timer for the double bullet to check when the player can fire again
        private float _fireAllowedDoubleBullet = 0.0f;
        // Used to allow the bullets to shoot fast/slow
        private float _timeAllowed = 0.25f;

        // Variable to control the duration of the lerping for the throb effect
        float t = 0;
        float t2 = 0;

        // Variable to control the time of the lerping
        float duration = 1.0f;
        float duration2 = 1.0f;

        // If these are < 0 then disable the powerup
        float speedTimeRemaining = 0;
        float shieldTimeRemaining = 0;
        float doubleShotTimeRemaining = 0;
        float speedyBulletTimeRemaining = 0;

        // Variable to get the game manager script (script communication)
        private GameManager _gameManager;
        // Variable for script communication of the UIController
        private UIController _uIController;
        // Variable for script communication of the Spawn Controller
        private SpawnController _spawnManager;

        // Add two game objects to enable/disable double thruster/single thruster sounds
        public GameObject _normalThrusterSound;
        public GameObject _doubleThrusterSound;

        // Add handles for the two engines when the player is damaged
        private GameObject _firstEngine;
        private GameObject _secondEngine;

        // Enable/disable thruster and double thruster
        public GameObject _normalThruster;
        public GameObject _doubleThruster;

        // Images for the left/right/default sprites
        [SerializeField]
        private Sprite turnLeft;
        [SerializeField]
        private Sprite turnRight;
        [SerializeField]
        private Sprite defaultPos;

        void Start()
        {
            // Reset this variable when the player spawns
            GameManager._bossSpawner = 600;

            // Current position = new position
            transform.position = new Vector3(0, 0, 0);

            // Sets the _uIController to the script on the "Canvas" object
            _uIController = GameObject.Find("Canvas").GetComponent<UIController>();

            // Sets the _gameManager to the script on the "GameManager" object
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

            // Sets the _spawnManager to the script on the "Spawn_Controller" object
            _spawnManager = GameObject.Find("Spawn_Controller").GetComponent<SpawnController>();

            // Play the normal thruster sound
            _normalThrusterSound.gameObject.SetActive(true);

            // Set normal thruster active
            _normalThruster.gameObject.SetActive(true);

            // Set hit counter to 0 when game restarts
            _hitCounter = 0;

            // Reset randomNumOnce variable
            setRandomNumOnce = true;

            // If there is a Spawn Manager script then
            if (_spawnManager != null)
            {
                // Start spawn routines again to start the game again
                _spawnManager.InitializeSpawnRoutines();
            }
        }

        void Update()
        {
            // Gives a throbbing effect to the player e.g. goes from the normal texture to red and then back
            // Only executes when throb is true
            if(throb)
            {
                // REFERENCE: https://answers.unity.com/questions/328891/controlling-duration-of-colorlerp-in-seconds.html

                // Lerp from white to red whilst t is incremented below (t is the time it takes to 
                // go from white to red)
                gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, t);

                if (t < 1)
                { // while t below the end limit...
                  // increment it at the desired rate every update:
                    t += Time.deltaTime / duration;
                }

                // If the lerping is done then move onto reversing the lerp
                if(t > 1)
                {
                    // Stop this lerping and set throbEnd to true which starts the reverse
                    throbEnd = true;
                    throb = false;
                }
            }

            // Start the reverse lerp if throbEnd is true
            if(throbEnd)
            {
                // Lerp from red to white under t2; this part is essentially the same as the above
                // except in reverse
                gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, t2);

                if (t2 < 1)
                { // while t below the end limit...
                  // increment it at the desired rate every update:
                    t2 += Time.deltaTime / duration2;
                }

                // Stop the reverse lerp
                if(t2 > 1)
                {
                    throbEnd = false;
                }
            }

            // Keep incrementing this variable to determine when to fire again
            _fireAllowed += Time.deltaTime;

            // Keep incrementing this variable to determine when to fire again for double shot
            _fireAllowedDoubleBullet += Time.deltaTime;

            // Keep checking these methods to see when to disable the powerups
            CheckSpeedUp();
            CheckShield();
            CheckDoubleShot();
            CheckSpeedyBullet();

            // Update lives to lives variable, keep checking every update
            _uIController.LivesUpdate(lives);

            // Keep checking to see if the user wants to move
            Movement();

            // If the space key is pressed, make an instantiation
            // of the laser prefab above the player

            if(fastShoot == false)
            {
                // Only execute this code when the player has not picked up fast shoot
                // GetKeyDown is the key distinction
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    Fire();
                }
            }

            if(fastShoot)
            {
                // Only execute this code when the player has not picked up fast shoot
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    Fire();
                }
            }


            // Keep calling the below methods for checking
            ResetRandomNumOnce();
        }

        // LateUpdate() is called after Update()
        private void LateUpdate()
        {
            // Method to clamp the X position to prevent boundary exit
            CheckXBoundaries();
        }

        // If the TimeRemaining variable is > 0 then don't disable it
        // If it is < 0, disable it 
        void CheckSpeedyBullet()
        {
            if (speedyBulletTimeRemaining > 0)
            {
                speedyBulletTimeRemaining -= Time.deltaTime;

                if (speedyBulletTimeRemaining < 0)
                {
                    speedyBulletTimeRemaining = 0;
                    ActivateSpeedyBullet(false);
                }
            }
        }

        // If the TimeRemaining variable is > 0 then don't disable it
        // If it is < 0, disable it 
        void CheckSpeedUp()
        {
            if (speedTimeRemaining > 0)
            {
                speedTimeRemaining -= Time.deltaTime;

                if (speedTimeRemaining < 0)
                {
                    speedTimeRemaining = 0;
                    ActivateSpeedUp(false);
                }
            }
        }

        void CheckShield()
        {
            if (shieldTimeRemaining > 0)
            {
                shieldTimeRemaining -= Time.deltaTime;

                if (shieldTimeRemaining < 0)
                {
                    AudioSource.PlayClipAtPoint(_shieldDownClip, Camera.main.transform.position, 1f);
                    shieldTimeRemaining = 0;
                    ActivateShield(false);
                }
            }
        }

        void CheckDoubleShot()
        {
            if (doubleShotTimeRemaining > 0)
            {
                doubleShotTimeRemaining -= Time.deltaTime;

                if (doubleShotTimeRemaining < 0)
                {
                    doubleShotTimeRemaining = 0;
                    ActivateDoubleShot(false);
                }
            }
        }

        // Method to clamp the X position to prevent boundary exit
        private void CheckXBoundaries()
        {
            // Get the screen space
            Vector3 xClamper = Camera.main.WorldToViewportPoint(transform.position);
            // Clamp the x position of this game object between the values below according to the
            // screen space
            xClamper.x = Mathf.Clamp(xClamper.x, 0.05f, 0.95f);
            // Set this game object to the clamped position always in the world
            transform.position = Camera.main.ViewportToWorldPoint(xClamper);
        }

        // Method to reset the random num once to generate an engine sprite on either side
        private void ResetRandomNumOnce()
        {
            // Reset setRandomNumOnce if lives = 3 so a random engine can be spawned at the start
            if (lives == 3)
            {
                setRandomNumOnce = true;
            }
        }

        private void Movement()
        {
            // If you are idling then change to default position
            if(idleOn == true)
            {
                // Code to change the sprite position
                this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultPos;
            }

            // Checks to see if the Left Arrow key is pressed
            if (Input.GetKey(KeyCode.A))
            {
                // Idle is off
                idleOn = false;

                // Code to change the sprite position
                this.gameObject.GetComponent<SpriteRenderer>().sprite = turnLeft;

                // Code below checks to see if canSpeedUp is true and if it is
                // times the speed by 1.5 so the player can move faster
                // This code is duplicated for the other vectors below
                if (canSpeedUp == true)
                {
                    transform.position += Vector3.left * _movementSpeed * 3f * Time.deltaTime;
                }
                // Changes the current position of the object to go left according to the speed variable and Time.deltaTime which allows
                // smooth movement
                else
                {
                    transform.position += Vector3.left * _movementSpeed * Time.deltaTime;
                }
            }
            else
            {
                idleOn = true;
            }

            // Same as above but for the right
            if (Input.GetKey(KeyCode.D))
            {
                // Idle is off
                idleOn = false;

                // Code to change the sprite position
                this.gameObject.GetComponent<SpriteRenderer>().sprite = turnRight;

                if (canSpeedUp == true)
                {
                    transform.position += Vector3.right * _movementSpeed * 2f * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.right * _movementSpeed * Time.deltaTime;
                }
            }
            else
            {
                idleOn = true;
            }

            // Same as above but for the up
            if (Input.GetKey(KeyCode.W))
            {
                // Code to change the sprite position
                this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultPos;

                if (canSpeedUp == true)
                {
                    transform.position += Vector3.up * _movementSpeed * 2f * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.up * _movementSpeed * Time.deltaTime;
                }
            }

            // Same as above but for down
            if (Input.GetKey(KeyCode.S))
            {
                // Code to change the sprite position
                this.gameObject.GetComponent<SpriteRenderer>().sprite = defaultPos;

                if (canSpeedUp == true)
                {
                    transform.position += Vector3.down * _movementSpeed * 2f * Time.deltaTime;
                }
                else
                {
                    transform.position += Vector3.down * _movementSpeed * Time.deltaTime;
                }
            }

            // Clamp the player so that he or she does not leave the boundaries for the Y
            if (transform.position.y > 4.32f)
            {
                transform.position = new Vector3(transform.position.x, 4.32f, 0);
            }

            if (transform.position.y < -3.96)
            {
                transform.position = new Vector3(transform.position.x, -3.96f, 0);
            }
        }

        public void HitEncountered()
        {
            if (canShield == false)
            {
                // Hit encountered
                _hitCounter++;

                // Subtract a life 
                lives = lives - 1;
                // Update lives sprite
                _uIController.LivesUpdate(lives);

                // Play the hit audio clip
                AudioSource.PlayClipAtPoint(_hitClip, Camera.main.transform.position, 1f);

                // Set random number variable only once
                if (setRandomNumOnce == true)
                {
                    _randomNum = Random.Range(0, _engineSprites.Length);
                    setRandomNumOnce = false;
                }

                // If logic for the player hits and the damaged engines
                if (_hitCounter == 1)
                {
                    // Assign the _firstEngine generated to a game object 
                    // to allow disabling of it in the "AddHealth()" method
                    _firstEngine = _engineSprites[(int)_randomNum];
                    _firstEngine.SetActive(true);
                }
                else if (_hitCounter == 2)
                {
                    if(_randomNum == 0f)
                    {
                        // Assign the _secondEngine generated to a game object 
                        // to allow disabling of it in the "AddHealth()" method
                        _secondEngine = _engineSprites[1];
                        _secondEngine.SetActive(true);
                    }
                    else
                    {
                        _secondEngine = _engineSprites[0];
                        _secondEngine.SetActive(true);
                    }
                }
            }
            else
            {
                // Disable bool after half a second
                StartCoroutine(DisableShieldBool());
                // Disable the sprite shield that shows the player has a sheild
                _shieldSprite.SetActive(false);
            }

            // If lives is less than 1 destroy the player i.e. dead
            if (lives < 1)
            {
                PlayerDead();
            }
        }

        private void Fire()
        {
            // _fireAllowedDoubleBullet is used to check when to fire again
            if (_fireAllowedDoubleBullet >= _timeAllowed && canDoubleShot == true)
            {
                // Code below works the same as when canDoubleShot is false but a different prefab is used
                Instantiate(_doubleShotPrefab, transform.position, Quaternion.identity);
                // Reset _fireAllowedDoubleBullet
                _fireAllowedDoubleBullet = 0;
                // Play double shot sound
                AudioSource.PlayClipAtPoint(_doubleShotClip, Camera.main.transform.position, 1f);
            }

            // _fireAllowed is used to check when to fire again
            if (_fireAllowed >= _timeAllowed && canDoubleShot == false)
            {
                // Instantiate (make a copy of) the prefab "bulletPrefab" 0.88f above the player sprite
                Instantiate(_bulletPrefab, transform.position + new Vector3(0, 0.88f, 0), Quaternion.identity);
                // Reset _fireAllowed
                _fireAllowed = 0;
                // Play single shot sound
                AudioSource.PlayClipAtPoint(_singleShotClip, Camera.main.transform.position, 1f);
            }
        }

        // If the player collides with the enemy bullet then instantly kill them
        public void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.tag == "EnemyBullet")
            {
                // Reset the game completely i.e. the player is dead
                PlayerDead();
            }

            // Kill the player if they hit the boss
            if (collision.tag == "Boss")
            {
                // Reset the game completely i.e. the player is dead
                PlayerDead();
            }

            // Indicate to the player that they have been damaged
            if(collision.tag == "Enemy")
            {
                // Lerp between two colors like a throbbing effect and stop it after a few secs
                if(canShield == false)
                {
                    // Play the engine failure sound
                    AudioSource.PlayClipAtPoint(_engineFailureClip, Camera.main.transform.position, 1f);

                    // t and t2 being set to 0 returns sets the lerping back to the start
                    t = 0;
                    t2 = 0;
                    throb = true;
                }
                else
                {
                    // Play the shield down effect
                    AudioSource.PlayClipAtPoint(_shieldDownClip, Camera.main.transform.position, 1f);
                }
            }
        }

        public void PlayerDead()
        {
            // When the player disappears make the explosion appear at the player position
            Instantiate(_explosionPlayerPrefab, transform.position, Quaternion.identity);
            // Play explosion sound
            AudioSource.PlayClipAtPoint(_playerExplosionSound, Camera.main.transform.position, 1f);
            // Show the main screen when the player is dead
            _uIController.ShowMainScreen();
            // Reset timeLeft in Game Manager
            _gameManager.timeLeft = 6.5f;
            // Disable lives image
            _uIController.livesImage.enabled = false;
            // The game is now over
            GameManager.gameOver = true;
            // Stop the counter from decrementing until the game has started again
            _gameManager.pauseCountdown = false;
            // Reset the timer to enable the pause menu
            _gameManager.timeLeftPause = 2f;
            // Play countdown sound
            _gameManager.audioPlayOnce = true;
            // Play game over sound
            _gameManager.audioPlayOnceGameOver = true;
            // Update the hiscore when the player dies
            _uIController.UpdateHiscore();
            // Set first game to true
            _gameManager.firstGame = true;
            // Reset cheat for credit iterator and player prefs deletion
            CheatCodes.arrayIndex = 0;
            CheatCodes.playerPrefsIndex = 0;
            Destroy(this.gameObject);
        }

        // This keeps adding to speedyBulletTimeRemaining and when it is 5.0f the "Check" method
        // will call "ActivateSpeedyBullet" and pass false to disable it
        public void AddSpeedyBullet(float seconds)
        {
            speedyBulletTimeRemaining += seconds;
            ActivateSpeedyBullet(true);
        }

        // Adds a pickup ability so it's quicker to shoot a bullet when this pickup is picked up
        public void ActivateSpeedyBullet(bool activate)
        {
            if(activate)
            {
                _timeAllowed = 0.15f;
                fastShoot = true;
            }
            else
            {
                _timeAllowed = 0.25f;
                fastShoot = false;
            }
        }

        // Adds a health to the player dependent on the condition below
        public void AddHealth()
        {
            if(lives == 2)
            {
                lives++;

                // Decrease _hitCounter to prevent confusion between if statements
                _hitCounter--;

                // To prevent an error, check to see if the game object _firstEngine exists
                if (_firstEngine != null)
                {
                    _firstEngine.SetActive(false);
                }
            }

            if(lives == 1)
            {
                lives++;

                // Decrease _hitCounter to prevent confusion between if statements
                _hitCounter--;

                // To prevent an error, check to see if the game object _secondEngine exists
                if (_secondEngine != null)
                {
                    _secondEngine.SetActive(false);
                }          
            }
        }

        // This sets the relevant properties to enable the power up
        public void ActivateSpeedUp(bool activate)
        {
            _doubleThrusterSound.gameObject.SetActive(activate);
            _doubleThruster.gameObject.SetActive(activate);
            _normalThruster.gameObject.SetActive(!activate);
            _normalThrusterSound.gameObject.SetActive(!activate);

            if (activate)
            {
                canSpeedUp = true;
            }
            else
            {
                canSpeedUp = false;
            }
        }

        // This is called in Pickup.cs and keeps adding a value to
        // speedTimeRemaining to keep the powerup alive when more
        // pickups are "picked up"
        public void AddSpeedUp(float seconds)
        {
            speedTimeRemaining += seconds;
            ActivateSpeedUp(true);
        }

        public void ActivateShield(bool activate)
        {
            _shieldSprite.SetActive(!activate);
            _shieldSprite.SetActive(activate);

            if (activate)
            {
                canShield = true;
            }
            else
            {
                canShield = false;
            }
        }

        public void AddShield(float seconds)
        {
            shieldTimeRemaining += seconds;
            ActivateShield(true);
        }

        public void ActivateDoubleShot(bool activate)
        {
            if (activate)
            {
                canDoubleShot = true;
            }
            else
            {
                canDoubleShot = false;
            }
        }

        public void AddDoubleShot(float seconds)
        {
            doubleShotTimeRemaining += seconds;
            ActivateDoubleShot(true);
        }


        IEnumerator DisableShieldBool()
        {
            yield return new WaitForSeconds(0.5f);
            // Disable the shield if a hit is encountered and play a sound 

            // This prevents the sound from playing a little later
            shieldTimeRemaining = -1;
            canShield = false;
        }
    }
}