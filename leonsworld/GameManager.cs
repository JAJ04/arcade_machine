using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace LeonsWorld
{
    public class GameManager : MonoBehaviour {

        // Variables for each of the buttons on the pause menu
        public GameObject button1;
        public GameObject button2;
        public GameObject button3;
        public GameObject button4;

        // Variables for the weapon menu
        public GameObject nerfButton;
        public GameObject aerosolButton;
        public GameObject flamethrowerButton;
        public GameObject backMenuButton;

        // GameObject variables that can be used to access properties in the scene 
        // e.g. for the player, bossFace, evil snowmen, muting the select sound etc
        public GameObject player;
        public GameObject bossFace;
        private GameObject[] gameObjectsNerf;
        private GameObject[] gameObjectsCan;
        private GameObject[] gameObjectsThrower;
        public GameObject snowman1;
        public GameObject snowman2;
        public GameObject snowman3;
        public GameObject snowman4;
        public GameObject face;
        public GameObject face2;
        public GameObject face3;
        public GameObject canvas;
        public GameObject pausedText;
        public GameObject saveBlock;
        public GameObject aerosol;
        public GameObject flameThrower;
        public GameObject nerfGun;

        // Used to mute the snowmen when the game is over
        public GameObject[] snowmanArray;

        // Used to get the properties of the player
        public PlayerControl controller2D;

	    // Bullet variables
	    public Rigidbody bulletPrefab;
	    public Rigidbody bulletPrefab2;
	    public Rigidbody bulletPrefab3;

	    // This displays what weapon you have selected on the bottom-left
	    public Text weaponTitle;

        // Reference to the credits text
        public Text creditsText;

        // This is a Text that allows the stats to be displayed on the screen in a GUIText
        public Text statsDisplay;

        // Controls Icons
        public float iconSizeX = 25;
	    public float iconSizeY = 25;

        // Used for the saving timer in the game
        public float saveTimer;
        public float timerBullet;

        // Set it so the player doesn't move at all
        private float horizontal = 0;

        // A variety of rates/cooldowns for the weapons
        private float attackRate = 0.0f;
        private float attackFlame = 0.02f;
        private float canFireAfter = 0.0f;
        private float nerf = 1f;
        private float coolDown;

        // This is used to check if the key for firing a weapon is pressed
        private bool isKeyPressed = false;

        // Sets the max amount of EXP the player can have (this changes with gameplay)
        public int maxEXP;
        [HideInInspector]

        // Starting health
        public static int curHealth;
        // Max health the player can have
	    public static int maxHealth;
        // Sets the level to 1 by default; carries across scenes
        public static int level = 1;
        // Sets the amount of coins the player has
        public static int curCoins;
        // Bullet damage should be 5 in the beginning
        public static int bulletDamage = 5;
	
        // Static so it can be carried across multiple scenes
	    public static int weapon1Unlocked;
	    public static int weapon2Unlocked;

        // Static so it can be carried across multiple scenes
        public static int curEXP = 0;
        [HideInInspector]

        // A variety of bools that enables/disables various states in the game e.g.
        // pausing the game and oolDowns for the weapons
        public static bool pauseMenu;

        // This is a handler that shows the weaponBarSlider to show how much ammo the player has
        public Slider weaponBarSlider;

        // Audio bits that this script can enable/paly
        public AudioSource bulletFired;
	    public AudioSource bulletFired2;
	    public AudioSource bulletFired3;
	    public AudioSource click;
	    public AudioSource MJ;
        public AudioSource angel;
        public AudioSource background;
        public AudioSource hbo;
        public AudioSource chaching;
        public AudioSource angelSource;
        public AudioClip angelClip;
        public AudioSource backSound;
	
        // Enables whether the player can equip these weapons/use them
	    public bool aerosolCanBool = false;
        public bool flameThrowerBool = false;
        public bool nerfGunBool = true;
        // Enables whether the timer is done or not
        public bool timerIsNotDone = true;
        // This allows the user to go back to the god menu when they die
        public bool goBackToGodMenu = false;
        // Additional boolean for the pause menu enabling
        public bool pauseMenuOther;
        // Allows the player to fire or not
        public bool canFire;

        // Boolean to enable the "weapons menu" on the pause menu
        public static bool weaponsMenu;
        // Allows the player to select the weapons with the keyboard keys
        public static bool keysActive = true;
        // Used to add a delay to the weapon
        public static bool wepKeys = true;
        // This is used to set whether the player is dead or not
        public static bool death = false;
        // This allows the player to enable the menu if the game is not paused (prevents overlaps)
        public static bool canEnableMenu = true;

        // Sets whether it is the first game or not
        private bool firstGame = true;
        // Allows the "back sound" to play only once
        private bool soundPlayOnce = true;
        // This is a bool that shows the player stats on the screen or not
        private bool playerStats = false;
        // Checks to see if the player is dead
        private bool dead;
        // Used to play the angel sound once
        private bool angelSourceOnce = true;
        // The player looks right by default
        private bool lookRight = true;


        // Awake() executes before Start()
        void Awake()
	    {
            // Make it so the player can change weapons with LCTRL and the num keys when this script is awoken
            wepKeys = true;
            // Allow the pause menu to be activated again
            canEnableMenu = true;
            // Don't show the paused text when the scene starts
            pausedText.SetActive(false);
            // Set it so the player can move in the beginning
            controller2D.enabled = true;
		    curEXP = PlayerPrefs.GetInt("Player Exp");
		    level = PlayerPrefs.GetInt("Player Level");
            curHealth = PlayerPrefs.GetInt("Player Health");

            // Set the cur coins amount on the first start
            if (!PlayerPrefs.HasKey("Player Coins") || PlayerPrefs.GetInt("Player Coins") <= 0)
            {
                curCoins = 300;
                PlayerPrefs.SetInt("Player Coins", curCoins);
            }
            else
            {
                // Set the cur coins to whatever it was before
                curCoins = PlayerPrefs.GetInt("Player Coins");
            }

            // Set the cur health amount on the first start
            if (!PlayerPrefs.HasKey("Player Health") || PlayerPrefs.GetInt("Player Health") <= 0)
            {
                curHealth = maxHealth + 1;
                PlayerPrefs.SetInt("Player Health", curCoins);
            }
            else
            {
                // Set the cur coins to whatever it was before
                curHealth = PlayerPrefs.GetInt("Player Health");
            }

            bulletDamage = PlayerPrefs.GetInt("Player Damage");
		    weapon1Unlocked = PlayerPrefs.GetInt("Weapon1");
		    weapon2Unlocked = PlayerPrefs.GetInt("Weapon2");
            
            // Allow the bullets to be shown when the game starts
		    bulletPrefab.GetComponent<SpriteRenderer>().enabled = true;
		    bulletPrefab2.GetComponent<SpriteRenderer>().enabled = true;
		    bulletPrefab3.GetComponent<SpriteRenderer>().enabled = true;
		
            // Set bulletDamage to be 1 as the default
		    if(bulletDamage == 0)
		    {
			    bulletDamage = 5;
		    }

            // The code below executes depending on what the bulletDamage is
            // depending on what the bulletDamage is, it determines what gun you can get
		    if (bulletDamage == 5) 
		    {
                GiveNerfGun();
		    }

            if (bulletDamage == 1)
            {
                GiveAerosolCan();
            }

            if (bulletDamage == 6)
            {
                GiveFlameThrower();
            }

            // Set the default values for the maxEXP (experience) and maxHealth
            maxEXP = level * 50;
		    maxHealth = level + 2;
	    }

	    void Start()
	    {
            // Make both bools false at the start and make sure that all of the variables
            // are in the right place
            weaponsMenu = false;
            pauseMenu = false;
            death = false;
		
            // Save block doesn't show when the game starts
		    saveBlock.SetActive(false);
            // Used for cool downs
		    canFire = false;
	    }
	
	    void Update()
	    {
            // This is for the autosaving timer
            saveTimer += Time.deltaTime;

            // These are variables used to adjust when the player can fire bullets again
		    timerBullet += Time.deltaTime;
		    canFireAfter += Time.deltaTime;

            // If this bool is true then the player can go back to the god menu if "LeftAlt" is also pressed
            if(goBackToGodMenu)
            {
                if(Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    // This needs to be false to move the game backwards
                    death = false;
                    // Don't play the angel sound
                    angelSource.Stop();
                    StartCoroutine(GoBackToGodMenu());
                }
            }

            // When the curHealth is less than or equal to 0
            if (curHealth <= 0)
            {
                // Check if the player isn't dead
                if (dead == false)
                {
                    // Kill the player
                    KillPlayer();                
                    // Don't run this code again
                    dead = true;
                    // Pause the game
                    death = true;
                }
            }

            // Show the menu if canEnableMenu is true (if the game is not paused)
            if (canEnableMenu)
		    {
			     if (Input.GetKeyDown(KeyCode.Escape))
			     {
                    // Starts the coroutine to select and deselect the button as 
                    // it runs on the next frame
                    StartCoroutine(HighlightFirstButton());                   

                    pauseMenu = !pauseMenu;
                    click.Play();
                }
		    }
		 
            // If the game is paused then set timeScale to 0 and set everything that controls
            // the player to false and disable the boss slider/boss text if it is available
		    if(death)
            {
                controller2D.walking.Stop();
                StopBulletSounds();
                Time.timeScale = 0;
                flameThrowerBool = false;
                aerosolCanBool = false;
			    controller2D.controllable = false;
			    canEnableMenu = false;
			    pauseMenu = false;

                // Allow the user to go back to the god menu
                goBackToGodMenu = true;

                // Don't show the credits text
                creditsText.GetComponent<Text>().enabled = false;

                // Only show the pause menu text when the player is pausing
                if (dead == true)
                {
                    if(angelSourceOnce == true)
                    {
                        // Disable the weapon slider and play a sound 
                        angelSource.PlayOneShot(angelClip);
                        angelSourceOnce = false;
                        weaponBarSlider.gameObject.SetActive(false);
                    }
                }
		     }
		     else
		     {
                controller2D.controllable = true;

                // Only unpause the game in this statement when the player is dead
                if(dead == true)
                {
                    Time.timeScale = 1;
                }

                aerosolCanBool = true;
                flameThrowerBool = true;
                statsDisplay.enabled = true;
			    weaponTitle.enabled = true;
            }
		 
             // Alternative keys to equip the weapons
		     if(wepKeys)
		     {
			     if(Input.GetKeyDown(KeyCode.LeftControl))
			     {
                     GiveNerfGun();
			     }
			 
			     if(Input.GetKeyDown(KeyCode.Alpha2) && PlayerPrefs.GetInt("Weapon1", 0) >= 1)
			     {
                    GiveAerosolCan();
			     }
			 
			     if(Input.GetKeyDown(KeyCode.Alpha3) && PlayerPrefs.GetInt("Weapon2", 0) >= 1)
			     {
                    GiveFlameThrower();
			     }
		     }

            if (weaponsMenu)
		     {
                // Stop the walking sound
                controller2D.walking.Stop();

                // Don't allow CTRL, 1, 2 wep switching whilst weapon menu is open
                wepKeys = false;

                // This will change the text on the buttons depending on if you can equip an aerosol can or not
                if (PlayerPrefs.GetInt("Weapon1", 0) >= 1)
                {
                    aerosolButton.GetComponentInChildren<Text>().text = "Equip Aerosol Can";
                    SaveGame();
                }
                else
                {
                    aerosolButton.GetComponentInChildren<Text>().text = "Buy Aerosol Can - 250 Coins";
                    SaveGame();
                }

                if (PlayerPrefs.GetInt("Weapon2", 0) >= 1)
                {
                    flamethrowerButton.GetComponentInChildren<Text>().text = "Equip Flamethrower";
                    SaveGame();
                }
                else
                {
                    flamethrowerButton.GetComponentInChildren<Text>().text = "Buy Flamethrower - 500 Coins";
                    SaveGame();
                }

                // This enables the pause menu if "Escape" is pressed and plays a click sound
                if (Input.GetKeyDown(KeyCode.Escape))
			    {
                    click.Play();

				    weaponsMenu = !weaponsMenu;
				 
				    if (!weaponsMenu)
				    {
                       pauseMenu = true;
					   keysActive = true;
                       wepKeys = false;
                    }
				    else
				    {
                       pauseMenu = false;
                    }
                }
		     }

             // Set the bullet damage to be 1 by default/the nerf gun for safety precautions
		    if(bulletDamage == 0)
		    {
			    bulletDamage = 5;
		    }
		 
		    // AUTOSAVE
		    if(saveTimer >= 15f)
		    {
			    SaveGame();
                saveTimer = 0;
		    }
		
            // If all of the snowmen are dead then show the save block
		    if(snowman1 == null && snowman2 == null && snowman3 == null && snowman4 == null)
		    {
			    saveBlock.SetActive(true);
		    }
		
            // The code below changes which face shows depending on what the current 
            // health of the player i.e. the DOOM face changes depending on what the health is
		    if (curHealth >= 3)
		    {
                // If there are SpriteRenderer components attached then run the code in the brackets
                if (face.GetComponent<SpriteRenderer>() && face2.GetComponent<SpriteRenderer>() && face3.GetComponent<SpriteRenderer>())
                {
                    face.GetComponent<SpriteRenderer>().enabled = true;
                    face2.GetComponent<SpriteRenderer>().enabled = false;
                    face3.GetComponent<SpriteRenderer>().enabled = false;
                }
		    }

		    if (curHealth == 2) 
		    {
                if (face.GetComponent<SpriteRenderer>() && face2.GetComponent<SpriteRenderer>() && face3.GetComponent<SpriteRenderer>())
                {
                    face.GetComponent<SpriteRenderer>().enabled = false;
                    face2.GetComponent<SpriteRenderer>().enabled = true;
                    face3.GetComponent<SpriteRenderer>().enabled = false;
                }
		    }

		    if (curHealth == 1) 
		    {
                if (face.GetComponent<SpriteRenderer>() && face2.GetComponent<SpriteRenderer>() && face3.GetComponent<SpriteRenderer>())
                {
                    face.GetComponent<SpriteRenderer>().enabled = false;
                    face2.GetComponent<SpriteRenderer>().enabled = false;
                    face3.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

            // If the aerosol can is selected
            if(bulletDamage == 1)
            {
                // The statements essentially act like a timeout for the shooting
                // of the Aerosol Can

                // Only allows the Aerosol Can to be shot for 0.10f of a second
                // Otherwise you can't shoot it at i.e. 0f
                if (Input.GetKeyDown(KeyCode.LeftAlt) && !isKeyPressed)
                {
                    isKeyPressed = true;
                    timerBullet = 0f;
                }

                if (Input.GetKeyUp(KeyCode.LeftAlt) && isKeyPressed)
                {
                    isKeyPressed = false;
                    timerBullet = 0.15f;
                }

                if (timerBullet >= 0f)
                {
                    canFire = true;
                }

                if (timerBullet >= 0.15f)
                {
                    canFire = false;
                }
            }

            // If the flamethrower is selected
            if (bulletDamage == 6)
            {
                // Same as above except timeout values have changed
                if (Input.GetKeyDown(KeyCode.LeftAlt) && !isKeyPressed)
                {
                    isKeyPressed = true;
                    timerBullet = 0f;
                }

                if (Input.GetKeyUp(KeyCode.LeftAlt) && isKeyPressed)
                {
                    isKeyPressed = false;
                    timerBullet = 0.15f;
                }

                if (timerBullet >= 0f)
                {
                    canFire = true;
                }

                if (timerBullet >= 0.15f)
                {
                    canFire = false;
                }
            }

            // Keep updating horizontal
            horizontal = Input.GetAxis("Horizontal");

            // Move Player Right
            if (horizontal > 0)
		    {
			    lookRight = true;
		    }
		
		    // Move Player Left
		    if(horizontal < 0)
		    {
			    lookRight = false;
		    }

            // Allow "Space" to be used again after the time has gone over coolDown for the nerf gun
			if(Time.time >= coolDown)
			{
				if(keysActive)
				{
					if(nerfGunBool)
					{
						if(Input.GetKey(KeyCode.LeftAlt))
						{
							NerfGun();
							weaponBarSlider.value -= 1f;
							StartCoroutine(ResetBar());
						}
					}
				}
			}	

            // Allow "Space" to be used again after the time has gone over coolDown for the aerosol can
            if (canFire)
		    {
			    if(Time.time >= coolDown)
			    {
				    if(keysActive)
				    {
					    if(aerosolCanBool)
					    {
						    if(Input.GetKey(KeyCode.LeftAlt))
						    {
                                AerosolCan();
							    weaponBarSlider.value -= 0.35f;
							    StartCoroutine(ResetBar());
						    }
					    }
				    }
			    }
			
                // Allow "Space" to be used again after the time has gone over coolDown for the flame thrower
			    if(Time.time >= coolDown)
			    {
				    if(keysActive)
				    {
					    if(flameThrowerBool)
					    {
						    if(Input.GetKey(KeyCode.LeftAlt))
						    {
                                FlameThrower();
							    weaponBarSlider.value -= 0.10f;
							    StartCoroutine(ResetBar());
						    }
					    }
				    }
			    }
		    }
		
            // Allow the player to level up if their level is not 10
		    if(curEXP >= maxEXP)
		    {
			    if(level != 10)
			    {
				    LevelUp();
			    }
		    }
		
            // Once the level is 10 you cannot increase your EXP anymore
		    if(level == 10)
		    {
			    maxEXP = 0;
		    }

		    playerStats = true;
		
            // This is a cheat code to increase the curEXP
		    if(Input.GetKeyDown(KeyCode.E))
		    {
			    curEXP += 10;
		    }
		
            // Display the player stats to let the player know how they are doing, else, don't display anything
		    if(playerStats)
		    {
                if(level != 10)
                {
                    statsDisplay.text = "Level " + level + ":  XP " + curEXP + " / " + maxEXP + "\n"
                        + "Cur Health " + curHealth + " / " + "Max Health " + maxHealth + "\n"
                        + "Bullet Damage: " + bulletDamage + "\n" + "Coins: " + curCoins;
                }
                
                if(level == 10)
                {
                    statsDisplay.text = "Level " + level + ":  XP " + curEXP + " / " + "MAX" + "\n"
                        + "Cur Health " + curHealth + " / " + "Max Health " + maxHealth + "\n"
                        + "Bullet Damage: " + bulletDamage + "\n" + "Coins: " + curCoins;
                }
		    }
		    else
		    {
			    statsDisplay.text = "";
		    }
		
            // Don't allow the curHealth to go up anymore than the maxHealth
		    if(curHealth > maxHealth)
		    {
			    curHealth = maxHealth;
		    }
		
            // This is a cheat just for me to increase the curHealth quicker
		    if(Input.GetKeyDown(KeyCode.H))
		    {
			    curHealth++;
		    }

            if(timerIsNotDone)
            {
                if (pauseMenu)
                {
                    // Stop the walking sound
                    controller2D.walking.Stop();
                    // Show the paused text
                    pausedText.SetActive(true);
                    // Disable the player from using the guns
                    keysActive = false;
                    // Disable CTRL, 1, 2 wep switching
                    wepKeys = false;

                    // Show the credits text
                    creditsText.GetComponent<Text>().enabled = true;

                    // Pause the game
                    Time.timeScale = 0;

                    // Show the menu buttons
                    button1.SetActive(true);
                    button2.SetActive(true);
                    button3.SetActive(true);
                    button4.SetActive(true);
                    nerfButton.SetActive(false);
                    aerosolButton.SetActive(false);
                    flamethrowerButton.SetActive(false);
                    backMenuButton.SetActive(false);
                }
                else
                {
                    // Unpause the game if Escape is pressed whilst in the pause menu
                    if (Input.GetKeyDown(KeyCode.Escape))
                    {
                        // Don't show the paused text and allow the controls to be used as usual
                        pausedText.SetActive(false);
                        wepKeys = true;
                        keysActive = true;
                        Time.timeScale = 1;
                    }

                    // Disable the menu buttons
                    button1.SetActive(false);
                    button2.SetActive(false);
                    button3.SetActive(false);
                    button4.SetActive(false);
                }
            }
        }

        // Assign different properties so the nerf gun can be used right
        // Play the sound when firstGame is false (set to false at the end of this method)
        public void GiveNerfGun()
        {
            bulletDamage = 5;
            weaponTitle.text = "Nerf Gun";
            if(firstGame == false)
            {
                click.Play();
            }
            flameThrower.SetActive(false);
            aerosol.SetActive(false);
            nerfGun.SetActive(true);
            aerosolCanBool = false;
            nerfGunBool = true;
            flameThrowerBool = false;
            firstGame = false;
        }

        // Assign different properties so the aerosol can can be used right
        // Play the sound when firstGame is false (set to false at the end of this method)
        public void GiveAerosolCan()
        {
            bulletDamage = 1;
            weaponTitle.text = "Aerosol Can";
            if (firstGame == false)
            {
                click.Play();
            }
            flameThrower.SetActive(false);
            aerosol.SetActive(true);
            nerfGun.SetActive(false);
            aerosolCanBool = true;
            nerfGunBool = false;
            flameThrowerBool = false;
            firstGame = false;
        }

        // Assign different properties so the flame thrower can be used right
        // Play the sound when firstGame is false (set to false at the end of this method)
        public void GiveFlameThrower()
        {
            bulletDamage = 6;
            weaponTitle.text = "Flamethrower";
            if (firstGame == false)
            {
                click.Play();
            }
            flameThrower.SetActive(true);
            aerosol.SetActive(false);
            nerfGun.SetActive(false);
            aerosolCanBool = false;
            nerfGunBool = false;
            flameThrowerBool = true;
            firstGame = false;
        }
	
        // Increase the max health, level and allows the EXP points to 
        // drag over if the player levels up
	    void LevelUp()
	    {
		    curEXP = curEXP - maxEXP;
		    maxEXP = maxEXP + 50;
		    level++;
		    maxHealth++;
	    }

        // This will call when the player selects an Aerosol Can
        // It essentially adds some force to the gas as well as a cool down and sounds
        // It also flips the direction of the gas depending on where the player is facing (left/right)
        void AerosolCan()
        {
            if (lookRight)
            {
                if (bulletDamage == 1)
                {
                    Rigidbody bPrefab1 = Instantiate(bulletPrefab, new Vector2(transform.position.x - 0.50f, transform.position.y - 0.24f), Quaternion.identity) as Rigidbody;
                    bPrefab1.GetComponent<Rigidbody>().AddForce(Vector3.right * 20);
                    coolDown = Time.time + attackRate;
                    bPrefab1.transform.eulerAngles = new Vector2(0, 0);
                    if (!bulletFired.isPlaying)
                    {
                        bulletFired.Play();
                    }
                }
            }

            if (!lookRight)
            {
                if (bulletDamage == 1)
                {
                    Rigidbody bPrefab1 = Instantiate(bulletPrefab, new Vector2(transform.position.x + 0.50f, transform.position.y - 0.24f), Quaternion.identity) as Rigidbody;
                    bPrefab1.GetComponent<Rigidbody>().AddForce(-Vector3.right * 20);
                    coolDown = Time.time + attackRate;
                    bPrefab1.transform.eulerAngles = new Vector2(0, 180);
                    if (!bulletFired.isPlaying)
                    {
                        bulletFired.Play();
                    }
                }
            }
        }

        // This will call when the player selects a Nerf Gun
        // It essentially adds some force to the bullet as well as a cool down and sounds
        // It also flips the direction of the bullet depending on where the player is facing (left/right)
        void NerfGun()
        {
            if (lookRight)
            {
                if (bulletDamage == 5)
                {
                    Rigidbody bPrefab2 = Instantiate(bulletPrefab2, new Vector2(transform.position.x - 0.22f, transform.position.y - 0.33f), Quaternion.identity) as Rigidbody;
                    bPrefab2.GetComponent<Rigidbody>().AddForce(Vector3.right * 500);
                    coolDown = Time.time + nerf;
                    bPrefab2.transform.eulerAngles = new Vector2(0, 0);
                    bulletFired2.Play();
                }
            }

            if (!lookRight)
            {
                if (bulletDamage == 5)
                {
                    Rigidbody bPrefab2 = Instantiate(bulletPrefab2, new Vector2(transform.position.x + 0.22f, transform.position.y - 0.33f), Quaternion.identity) as Rigidbody;
                    bPrefab2.GetComponent<Rigidbody>().AddForce(-Vector3.right * 500);
                    coolDown = Time.time + nerf;
                    bPrefab2.transform.eulerAngles = new Vector2(0, 180);
                    bulletFired2.Play();
                }
            }
        }

        // This will call when the player selects a flamethrower
        // It essentially adds some force to the flames as well as a cool down and sounds
        // It also flips the direction of the flames depending on where the player is facing (left/right)
        void FlameThrower()
        {
            if (lookRight)
            {
                if (bulletDamage == 6)
                {
                    Rigidbody bPrefab3 = Instantiate(bulletPrefab3, new Vector2(transform.position.x + 0.38f, transform.position.y - 0.26f), Quaternion.identity) as Rigidbody;
                    bPrefab3.GetComponent<Rigidbody>().AddForce(Vector3.right * 500);
                    coolDown = Time.time + attackFlame;
                    bPrefab3.transform.eulerAngles = new Vector2(0, 0);

                    if (!bulletFired3.isPlaying)
                    {
                        bulletFired3.Play();
                    }
                }
            }

            if (!lookRight)
            {
                if (bulletDamage == 6)
                {
                    Rigidbody bPrefab3 = Instantiate(bulletPrefab3, new Vector2(transform.position.x - 0.38f, transform.position.y - 0.26f), Quaternion.identity) as Rigidbody;
                    bPrefab3.GetComponent<Rigidbody>().AddForce(-Vector3.right * 500);
                    coolDown = Time.time + attackFlame;
                    bPrefab3.transform.eulerAngles = new Vector2(0, 180);

                    if (!bulletFired3.isPlaying)
                    {
                        bulletFired3.Play();
                    }
                }
            }
        }

        // This saves several key/value pairs into the PlayerPrefs so that
        // the game can recall the state of the game after the save even when
        // the game quits
        public void SaveGame()
	    {
		    print("SAVED GAME");
		    PlayerPrefs.SetInt("Player Level", level);
		    PlayerPrefs.SetInt("Player Exp", curEXP);
		    PlayerPrefs.SetInt("Player Coins", curCoins);
		    PlayerPrefs.SetInt("Player Damage", bulletDamage);
            PlayerPrefs.SetInt("Player Health", curHealth);

            PlayerPrefs.Save();
	    }
	
        // Run code to kill the player
        // This resets states and variables so the scene
        // appears as it was at the start
	    public void KillPlayer()
	    {
            MuteSnowmen();
            StartCoroutine(RemoveCanBullets());
            player.GetComponent<Renderer>().enabled = false;
            bulletPrefab.GetComponent<SpriteRenderer>().enabled = false;
            bulletPrefab2.GetComponent<SpriteRenderer>().enabled = false;
            bulletPrefab3.GetComponent<SpriteRenderer>().enabled = false;
            StartCoroutine("PlayDeathSound");
            weaponTitle.enabled = false;
            statsDisplay.enabled = false;
            nerfGun.SetActive(false);
            flameThrower.SetActive(false);
            aerosol.SetActive(false);
            face.SetActive(false);
            face2.SetActive(false);
            face3.SetActive(false);
            canvas.SetActive(false);
        }

        // Mute the snowman sounds
        void MuteSnowmen()
        {
            // Empty snowmanArray
            snowmanArray = null;

            // Find game objects with tag enemy and put them into an array
            if (snowmanArray == null)
                snowmanArray = GameObject.FindGameObjectsWithTag("Enemy");

            // Traverse the array and stop all audio sources
            foreach (GameObject snowman in snowmanArray)
            {
                snowman.GetComponent<AudioSource>().Stop();
            }
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

        // This looks through each array full of game objects and 
        // destroys the elements in these arrays
        IEnumerator RemoveCanBullets()
	    {
		     gameObjectsThrower = GameObject.FindGameObjectsWithTag ("CanBullet");
     
		     for(var i = 0 ; i < gameObjectsThrower.Length ; i ++)
		     {
			     Destroy(gameObjectsThrower[i]);
		     }
		 
		     gameObjectsNerf = GameObject.FindGameObjectsWithTag ("PlayerBulletNerf");
     
		     for(var i = 0 ; i < gameObjectsNerf.Length ; i ++)
		     {
			     Destroy(gameObjectsNerf[i]);
		     }
		 
		     gameObjectsCan = GameObject.FindGameObjectsWithTag ("ThrowerBullet");
     
		     for(var i = 0 ; i < gameObjectsCan.Length ; i ++)
		     {
			     Destroy(gameObjectsCan[i]);
		     }
		 
		     yield return new WaitForSeconds(0.5f);
	    }
	
        // This resets the bar after 0.35f seconds
	    IEnumerator ResetBar()
	    {
		    yield return new WaitForSeconds(0.35f);
		    weaponBarSlider.value += 1f;
	    }
	
        // This mutes all of the audio sources and only allows the death sound effect to play as well as the "back" sound
	    IEnumerator PlayDeathSound()
	    {
		    AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		
		    foreach(AudioSource aud in audios)
		    {
			    aud.volume = 0;
                angelSource.volume = 1;
                backSound.volume = 1;
		    }

            // It then aims to restart the entire scene
            StartCoroutine(RestartSceneWait());

            yield return null;
	    }

        // This is the coroutine to restart the scene after 5 seconds
        IEnumerator RestartSceneWait()
        {
            yield return new WaitForSecondsRealtime(5f);
            RestartScene();
        }

        // If the player dies reload the scene and reset the variables
        void RestartScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            curHealth = 3;
            maxHealth = 3;
            curEXP = 0;
            maxEXP = 50;
            level = 1;
            Time.timeScale = 1;
            bulletPrefab.GetComponent<SpriteRenderer>().enabled = true;
            bulletPrefab2.GetComponent<SpriteRenderer>().enabled = true;
            bulletPrefab3.GetComponent<SpriteRenderer>().enabled = true;
        }

        public void StopBulletSounds()
        {
            // Disable the bullet firing sounds
            bulletFired.Pause();
            bulletFired2.Pause();
            bulletFired3.Pause();
        }

        // Select and deselect the button with the EventSystem
        IEnumerator HighlightFirstButton()
        {
            EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            es.SetSelectedGameObject(null);
            yield return null;
            es.SetSelectedGameObject(button1);
        }
    }
}