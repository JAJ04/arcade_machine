using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LeonsWorld
{
    public class PlayerControl : MonoBehaviour 
    {
        // Reference to get the transform components
        private Transform playerTransform;

        // Reference to Character Controller
        private CharacterController characterController;

        // This variable contains properties of the boss
        private Boss2D bossSnowmanCode;

        // Reference to GameManager
        public GameManager gameManager;

        // Variable to disable the evil laugh when the player is dead
        public GameObject bossSnowman;
        // References to change the angle of the gun and to disable them when 
        // the player is hit
        public GameObject _nerfGun;
        public GameObject _flameThrower;
        public GameObject _aerosol;

        // These variables are used to remove bullets from the scene
        GameObject[] gameObjectsNerf;
	    GameObject[] gameObjectsCan;
	    GameObject[] gameObjectsThrower;

        // This walk and jump height
        public float walkSpeed = 5;
        public float jumpHeight = 5;
        // Used to get the horizontal value from Unity
        public float horizontal = 0;
        // Floating point variable to store the player's movement speed
        public float speed;
        // Controls gravity i.e. the rate at which
        // you fall down
        public float gravity = 10f;

        // This adds a variety of variables for the jumping
        // e.g. how long the player can jump for and if they're in the air
        private float jumpTime;
        private float jumpDelay = 50f;
        // Float variable to capture the variable of the x position once when horizontal is 0
        private float playerX;

        // Variable to move the player
        public bool canMove = true;
        // Says whether the player can be controlled or not
        public bool controllable = true;
        // Bool to enable/disable the jumping
        public bool jumpBool = true;

        // Variables to check if the player has jumped or is in the air
        private bool jumped;
        private bool inAir = false;
        // Variable to enable the death sound after all Audio Sources are muted
        private bool allMuted = false;
        // Bool to capture the player X position only once
        private bool captureX = true;
        // Variable to decide when to turn off the guns for the flashing
        private bool displayGuns = true;
        // Variable to run the TakenDamage coroutine only once
        private bool takenDamageBool = true;
        // Bool to trigger the slip animation
        private bool iceSlip = false;

	    // AUDIO SOURCES TO PLAY
	    public AudioSource walking;
	    public AudioSource jump;
	    public AudioSource land;

        // Array of Audio Sources to play different hurt sounds
	    public AudioSource[] playerHurtSounds;

        // Audio sound to play a health pickup sound
        public AudioSource healthPickup;
	
	    // Controlling Player movement
	    public Vector2 moveDirection = Vector2.zero;

        // Reference to the boss title text
        public Text bossTitle;

        // Reference to the boss slider
        public Slider bossSlider;

        // Reference to the animator
        private Animator _animator;

        // Reference to the script that will allow the player to go back to the god menu in the "Boss" menu
        public GoBackToGodMenuBoss goBackToGodMenuBoss;

        void Start()
	    {
            // Set the animator to the animator component on this game object
            _animator = GetComponent<Animator>();

            // Initialize the transform variable to the transform component
            playerTransform = GetComponent<Transform>();

            // Used to move the player
            characterController = GetComponent<CharacterController>();

            // It is controllable at the start
            controllable = true;

            // Get the bossSnowmanCode if bossSnowman exists in the scene
            if(bossSnowman != null)
            {
                bossSnowmanCode = bossSnowman.GetComponent<Boss2D>();
            }
        }

        // Update is called once per frame
        void Update()
        {
            // Controls Character Controller
            characterController.Move(moveDirection * Time.deltaTime);
            horizontal = Input.GetAxis("Horizontal");
            horizontal = horizontal / 2;

            // Check to see what gun is enabled in the Game Manager (bulletDamage)
            // 6 is flamethrower, 1 is aerosol, 5 is nerf

            if(displayGuns)
            {
                if (GameManager.bulletDamage == 6)
                {
                    _nerfGun.GetComponent<SpriteRenderer>().enabled = false;
                    _aerosol.GetComponent<SpriteRenderer>().enabled = false;
                    _flameThrower.GetComponent<SpriteRenderer>().enabled = true;
                }

                if (GameManager.bulletDamage == 5)
                {
                    _nerfGun.GetComponent<SpriteRenderer>().enabled = true;
                    _aerosol.GetComponent<SpriteRenderer>().enabled = false;
                    _flameThrower.GetComponent<SpriteRenderer>().enabled = false;
                }

                if (GameManager.bulletDamage == 1)
                {
                    _nerfGun.GetComponent<SpriteRenderer>().enabled = false;
                    _aerosol.GetComponent<SpriteRenderer>().enabled = true;
                    _flameThrower.GetComponent<SpriteRenderer>().enabled = false;
                }
            }

            if(canMove)
            {
                // Keep the player still (prevents a bug where the player keeps moving all of the time)
                if (horizontal == 0)
                {
                    if (takenDamageBool)
                    {
                        // Set the bool so that the player is in the idle state
                        _animator.SetBool("HurtIdle", false);
                        _animator.SetBool("Walking", false);
                        _animator.SetBool("HurtWalking", false);
                        _animator.SetBool("Idle", true);
                    }
                    else
                    {
                        // Set the bool so that the player is in the idle state but hurt
                        _animator.SetBool("HurtIdle", true);
                        _animator.SetBool("Walking", false);
                        _animator.SetBool("HurtWalking", false);
                        _animator.SetBool("Idle", false);
                    }

                    // Only capture the X value once
                    if (captureX)
                    {
                        playerX = playerTransform.position.x;
                    }

                    // This sets captureX to false to it only captures the value once
                    captureX = false;
                    // This clamps the X position whilst no horizontal movement is being made so the player doesn't drift
                    playerTransform.transform.position = new Vector3(playerX, playerTransform.position.y, playerTransform.position.z);
                }

                // Set it so the player cannot move if you cannot control the player
                if (!controllable)
                {
                    horizontal = 0;
                }

                // Controls Player Gravity
                moveDirection.y -= gravity * Time.deltaTime;

                // Move Player Right
                if (horizontal > 0)
                {
                    // takenDamageBool is false when player is hit

                    if (takenDamageBool && iceSlip == false)
                    {
                        // Set bools so that the player is in a normal walking state
                        _animator.SetBool("HurtIdle", false);
                        _animator.SetBool("Walking", true);
                        _animator.SetBool("HurtWalking", false);
                        _animator.SetBool("Idle", false);
                    }

                    if (takenDamageBool == false && iceSlip == false)
                    {
                        // Set bools so that the player is in the hurting walk state
                        _animator.SetBool("HurtIdle", false);
                        _animator.SetBool("Walking", false);
                        _animator.SetBool("HurtWalking", true);
                        _animator.SetBool("Idle", false);
                    }

                    captureX = true;
                    transform.eulerAngles = new Vector2(0, 0);
                    moveDirection.x = horizontal * walkSpeed;

                    if (!walking.isPlaying)
                    {
                        walking.Play();
                    }
                }

                // Move Player Left
                if (horizontal < 0)
                {
                    // takenDamageBool is false when player is hit

                    if (takenDamageBool && iceSlip == false)
                    {
                        // Set the bool so that the player is in the normal walking state
                        _animator.SetBool("HurtIdle", false);
                        _animator.SetBool("Walking", true);
                        _animator.SetBool("HurtWalking", false);
                        _animator.SetBool("Idle", false);
                    }

                    if (takenDamageBool == false && iceSlip == false)
                    {
                        // Set bools so the player is in a hurt walking state
                        _animator.SetBool("HurtIdle", false);
                        _animator.SetBool("Walking", false);
                        _animator.SetBool("HurtWalking", true);
                        _animator.SetBool("Idle", false);
                    }

                    captureX = true;
                    transform.eulerAngles = new Vector2(0, 180);
                    moveDirection.x = horizontal * walkSpeed;

                    if (!walking.isPlaying)
                    {
                        walking.Play();
                    }
                }
            }

            // Jump if the jump bool is true
            if(jumpBool)
            {
                if (characterController.isGrounded && controllable)
                {
                    if (Input.GetKeyDown(KeyCode.Space) && inAir == false)
                    {
                        moveDirection.y = jumpHeight;
                        jump.Play();
                        jumpTime = jumpDelay;
                        jumped = true;
                        walking.volume = 0;
                        inAir = true;
                    }

                    jumpTime -= Time.deltaTime;
                    jumpTime = jumpTime / 4;

                    if (jumpTime <= 0 && characterController.isGrounded && jumped)
                    {
                        land.Play();
                        walking.volume = 1;
                        jumped = false;
                        inAir = false;
                    }
                }
            }

            // Stop the hurt sounds when the player is dead and make the 
            // guns invisible
            if(GameManager.curHealth < 1)
            {
                _nerfGun.GetComponent<SpriteRenderer>().enabled = false;
                _aerosol.GetComponent<SpriteRenderer>().enabled = false;
                _flameThrower.GetComponent<SpriteRenderer>().enabled = false;

                for (int i = 0; i < playerHurtSounds.Length; i++)
                {
                    playerHurtSounds[i].Stop();
                }
            }
        }

        // Flash the player sprite off and on if damaged; do not allow this coroutine to be ran again until it is over
        // Also sets displayGuns to false to allow flashing to happen on the guns
        public IEnumerator TakenDamage()
	    {
            if(takenDamageBool)
            {
                takenDamageBool = false;
                displayGuns = false;

                if(GameManager.bulletDamage == 5)
                {
                    _nerfGun.GetComponent<SpriteRenderer>().enabled = false;
                }

                if(GameManager.bulletDamage == 6)
                {
                    _flameThrower.GetComponent<SpriteRenderer>().enabled = false;
                }

                if(GameManager.bulletDamage == 1)
                {
                    _aerosol.GetComponent<SpriteRenderer>().enabled = false;
                }
                
                GetComponent<Renderer>().enabled = false;

		        yield return new WaitForSeconds(0.5f);

                if (GameManager.bulletDamage == 5)
                {
                    _nerfGun.GetComponent<SpriteRenderer>().enabled = true;
                }

                if (GameManager.bulletDamage == 6)
                {
                    _flameThrower.GetComponent<SpriteRenderer>().enabled = true;
                }

                if (GameManager.bulletDamage == 1)
                {
                    _aerosol.GetComponent<SpriteRenderer>().enabled = true;
                }

                GetComponent<Renderer>().enabled = true;

                yield return new WaitForSeconds(0.5f);

                if (GameManager.bulletDamage == 5)
                {
                    _nerfGun.GetComponent<SpriteRenderer>().enabled = false;
                }

                if (GameManager.bulletDamage == 6)
                {
                    _flameThrower.GetComponent<SpriteRenderer>().enabled = false;
                }

                if (GameManager.bulletDamage == 1)
                {
                    _aerosol.GetComponent<SpriteRenderer>().enabled = false;
                }

                GetComponent<Renderer>().enabled = false;

                yield return new WaitForSeconds(0.5f);

                if (GameManager.bulletDamage == 5)
                {
                    _nerfGun.GetComponent<SpriteRenderer>().enabled = true;
                }

                if (GameManager.bulletDamage == 6)
                {
                    _flameThrower.GetComponent<SpriteRenderer>().enabled = true;
                }

                if (GameManager.bulletDamage == 1)
                {
                    _aerosol.GetComponent<SpriteRenderer>().enabled = true;
                }

                GetComponent<Renderer>().enabled = true;

                takenDamageBool = true;
                displayGuns = true;
            }
        }

	    void OnTriggerEnter(Collider other)
	    {
            // Activate the slip animation if you go on ice
            if (other.tag == "Ice")
            {
                // Set a bool to trigger the Slip animation
                iceSlip = true;

                _animator.SetBool("HurtIdle", false);
                _animator.SetBool("Walking", false);
                _animator.SetBool("HurtWalking", false);
                _animator.SetBool("Idle", false);
                _animator.SetBool("Slip", true);

                // Start the coroutine to set the slip state to false after 0.5 secs
                StartCoroutine(SlipIceFalse()); 
            }

            // Give the player health if they encounter a potion
            if (other.tag == "HPotion")
            {
                healthPickup.Play();

                // Prevents slight text flickering for the health increase
                if (GameManager.curHealth < GameManager.maxHealth)
                {
                    GameManager.curHealth++;
                }

                Destroy(other.gameObject);
            }

            // Decrease the player health
            if (other.tag == "Bullet")
		    {
                GameManager.curHealth--;
			    Destroy (other.gameObject);
			    playerHurtSounds[Random.Range(0, playerHurtSounds.Length)].Play();
			    gameManager.controller2D.SendMessage("TakenDamage", SendMessageOptions.DontRequireReceiver);
		    }

            // Kill the player instantly and turn off UI objects and disable sound(s)
		    if(other.tag == "SnowballBoss")
		    {
                bossTitle.enabled = false;
                bossSlider.gameObject.SetActive(false);
                walking.Stop();
                gameManager.StopBulletSounds();
                gameManager.weaponBarSlider.gameObject.SetActive(false);
                gameManager.MJ.Stop();
                bossSnowmanCode.evilYell.Stop();
                StartCoroutine("RemoveCanBullets");
			    GameManager.curHealth = 0;
			    gameManager.player.GetComponent<SpriteRenderer>().enabled = false;
			    Time.timeScale = 0;
			    GameManager.pauseMenu = false;
			    Destroy(gameManager);
			    GameObject bulletType = GameObject.FindWithTag("PlayerBullet");

			    if(bulletType)
			    {
			        Destroy(bulletType.gameObject);
			    }

                gameManager.weaponTitle.enabled = false;
                gameManager.statsDisplay.enabled = false;
                gameManager.nerfGun.SetActive(false);
                gameManager.flameThrower.SetActive(false);
                gameManager.aerosol.SetActive(false);
                gameManager.bossFace.SetActive(false);
                StartCoroutine("MuteAudioExceptAngel");
                gameManager.canvas.SetActive(false);
            }

            // Decrease the player's health by 1
		    if (other.tag == "Enemy") 
		    {
                playerHurtSounds[Random.Range(0, playerHurtSounds.Length)].Play();
                GameManager.curHealth = GameManager.curHealth - 1;
		    }
		
            // Same as above but this is a test method
		    if(other.tag == "Enemy" && Input.GetKey(KeyCode.T))
		    {
                other.gameObject.SendMessage("EnemyDamaged", GameManager.bulletDamage, SendMessageOptions.DontRequireReceiver);
			    other.gameObject.SendMessage("TakenDamage", SendMessageOptions.DontRequireReceiver);
		    }
	    }
	
        // This is used to restart the scene after 5 seconds
	    public static class CoroutineUtil
	    {
		    public static IEnumerator WaitForRealSeconds(float time)
		    {
			    float start = Time.realtimeSinceStartup;
			    while (Time.realtimeSinceStartup < start + time)
			    {
				    yield return null;
			    }
		    }
	    }
	
	    IEnumerator RemoveCanBullets()
	    {
            // This function removes all of the bullets whenever this is called and mutes them
		     gameObjectsThrower = GameObject.FindGameObjectsWithTag ("CanBullet");
	 
		     for(var i = 0 ; i < gameObjectsThrower.Length; i ++)
		     {
                Destroy(gameObjectsThrower[i].gameObject);
		     }
		 
		     gameObjectsNerf = GameObject.FindGameObjectsWithTag ("PlayerBulletNerf");
	 
		     for(var i = 0 ; i < gameObjectsNerf.Length ; i ++)
		     {
                Destroy(gameObjectsNerf[i].gameObject);
		     }
		 
		     gameObjectsCan = GameObject.FindGameObjectsWithTag ("ThrowerBullet");
	 
		     for(var i = 0 ; i < gameObjectsCan.Length ; i ++)
		     {
                Destroy(gameObjectsCan[i].gameObject);
		     }

            yield return null;
	    }
	
	    void RestartScene()
	    {
            // Restarts the entire scene so the player can be controllable again
            // It's also used to give the player default values on the first game
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            GameManager.curHealth = 3;
		    GameManager.maxHealth = 3;
            GameManager.curEXP = 0;
		    gameManager.maxEXP = 50;
            GameManager.level = 1;
		    GameManager.bulletDamage = 1;
		    Time.timeScale = 1;
	    }
	
	    IEnumerator MuteAudioExceptAngel()
	    {
            // Mutes all of the audios except the angel sound/Mario death sound
		    AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		
		    foreach(AudioSource aud in audios)
		    {
			    aud.volume = 0;
		    }

            allMuted = true;

            if(allMuted)
            {
                gameManager.angel.volume = 1;
                goBackToGodMenuBoss.backSound.volume = 1;

                if(gameManager != null)
                {
                    gameManager.angel.Play();
                }
            }

		    yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(5));

            // Restart the scene
		    RestartScene ();
	    }

        // Coroutine to turn slipIce to false again to resume normal animation code
        IEnumerator SlipIceFalse()
        {
            yield return new WaitForSeconds(0.5f);

            // This resolves state collisions in mecanim
            _animator.SetBool("HurtIdle", false);
            _animator.SetBool("Walking", false);
            _animator.SetBool("HurtWalking", false);
            _animator.SetBool("Idle", false);
            _animator.SetBool("Slip", false);

            // Resume normal animation states
            iceSlip = false;
        }
    }
}