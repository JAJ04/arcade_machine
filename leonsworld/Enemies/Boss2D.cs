using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LeonsWorld 
{
    public class Boss2D: MonoBehaviour 
    {
        // Slider to show the health of the boss
        public Slider bossSlider;

        // title text for the boss
        public Text bossTitle;

        // Access the player properties
        public PlayerControl controller2D;

        // Units that the enemy will move right
        public int unitsToMove = 5;

        // Enemies movement speed
        public int moveSpeed = 2;

        //Enemy health
        public int bossHealth = 1;

        // Damage to player
        private int damageValue = 1;

        // Bool to trigger the coroutine
        private bool flashBoss = false;

        // This is used to keep repeating the evil yells
        public float evilTimer;

        // These are used to show the exclamation mark and the bullet
        public GameObject exclamation;
        public GameObject bullet;

        // Property to get the gameManager properties
        public GameManager gameManager;

        // Properties to enable the use of sounds
        public AudioSource evilYell;
        public AudioSource menacingLaugh;
        public AudioSource win;
        public AudioSource pain;

        // Awake() executed before start
        void Awake() 
	{
            // Set the enemy speed based on bools
            bossHealth = 1000;
        }

        void Update()
	{
            evilTimer += Time.deltaTime;
            GetComponent<Rigidbody>().position -= Vector3.right * moveSpeed * Time.deltaTime;

            // Keep updating the slider to what the enemy health is
            bossSlider.value = bossHealth;

            // Start the MJ theme and Vincent Price sound effect
            if (evilTimer >= 4.0 f) 
	    {
                if (!evilYell.isPlaying && !gameManager.MJ.isPlaying) 
		{
                    gameManager.MJ.Play();
                    evilYell.Play();
                }
            }
        }

        void OnTriggerEnter(Collider col) 
	{
            if (col.gameObject.tag == "Player") 
	    {
                // Checker method to prevent errors
                if (gameManager != null) 
		{
                    gameManager.SendMessage("PlayerDamaged", damageValue, SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        // Boss Taking Damage
        void BossDamaged(int damage)
	{
            // Decrease the enemy health and fire the "TakenDamage" function
            if (bossHealth > 0) 
	    {
                // This prevents the boss from flashing weirdly
                // Only allows the coroutine to start only once, never restart
                // or start again when the boss is hit
                if (flashBoss == false)
		{
                    StartCoroutine(TakenDamage());
                }

                bossHealth -= damage;
            }

            // The boss is now dead and unlock "Level 2" and give some rewards and remove the text and 
            // health slider
            if (bossHealth <= 0) 
	    {
                bossHealth = 0;
                bossTitle.enabled = false;
                bossSlider.gameObject.SetActive(false);
                GameManager.curCoins += 250;
                GameManager.curEXP += 250;
                gameManager.SaveGame();
                PlayerPrefs.SetInt("LevelUnlock", 3);
                Destroy(gameObject);
            }
        }

        // Flash the exclamation mark and the snowman sprite renderer if
        // damage has been taken
        public IEnumerator TakenDamage() 
	{
            flashBoss = true;

            pain.Play();
            yield return new WaitForSeconds(0.25 f);
            GetComponent<Renderer>().enabled = false;
            exclamation.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.25 f);
            GetComponent<Renderer>().enabled = true;
            exclamation.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(0.25 f);
            GetComponent<Renderer>().enabled = false;
            exclamation.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.25 f);
            GetComponent<Renderer>().enabled = true;
            exclamation.GetComponent<Renderer>().enabled = true;

            flashBoss = false;
        }
    }
}
