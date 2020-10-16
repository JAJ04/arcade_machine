using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DodgeEm
{
    public class PlayerController : MonoBehaviour
    {
        // REFERENCE TUTORIAL: https://www.youtube.com/playlist?list=PLytjVIyAOStpcOGg6HIHhnnOZAdxkAr1U

        // This will be used to change the position of the car
        private Vector3 playerPosition;

        // This is the amount the car can move at
        private float playerSpeed = 10f;

        // Clamping positions to prevent the player going off the road
        private float minClamp = -2.012007f;
        private float maxClamp = 1.680774f;

        // Bool to disable the controls
        private bool disableControls = false;

        // Play the skid sound only once
        private bool skidSoundOnce = true;

        // Instantiate bomb only once
        private bool bombOnce = true;

        // Bool to either destroy the player immediately or not
        private bool destroyPlayerImmediately;

        // Bool to decide whether to spin the car left or right depending
        // on the horizontal value
        private bool rotateLeft;

        // This will decide whether a rotation will happen
        private bool doRotation;

        // If the car is rotating then destroy the car with the rotation code
        private bool destroyRotate;

        // This bool is used to disable the collision code
        private bool disableCollisionCode;

        // Bool to trigger constant skid marks
        private bool reentrySkidMarks;

        // Bool for the UI Manager to activate the BTTF gui
        public bool activateBTTFGUI;

        // Reference to the speedometer
        public Text speedometer;

        // Create a reference to the UI manager
        public UIManager uiManager;

        // Reference to the Pause Game script
        public PauseGame pauseGame;

        // Reference to the theme music
        public AudioSource themeMusic;

        // Reference to the outatime license plate
        public GameObject outatime;

        // Reference to engine sound game object
        public GameObject engineSoundObject;

        // Reference to horn sound game object
        public GameObject hornSoundObject;

        // Reference to the re-entry smoke
        public GameObject smokeReentry;

        // Reference to the re-entry back smoke
        public GameObject smokeReentryBack;

        // Reference to the skid marks to instantiate it
        public GameObject skidMarks;

        // Reference to the blue explosion to instantiate it
        public GameObject blueExplosion;

        // Reference to the blue gas to instantiate it
        public GameObject blueGas;

        // Reference to the fire to instantiate it
        public GameObject firePrefab;

        // Reference to the bttf trail to instantiate it
        public GameObject bttfTrail;

        // Reference to the explosion to instantiate it
        public GameObject explosionPrefab;

        // Reference to the smoke to instantiate it
        public GameObject smokePrefab;

        // Reference to the road script
        public GameObject road;

        // Reference to the enemy spawner
        public GameObject enemySpawner;

        // Reference to the score game object
        public GameObject scoreGameObject;

        // Reference to the BTTF text to disable it when the car comes back
        public Text bttfText;

        // Reference to the plutonium chamber sound 
        public AudioSource plutoniumEmptySound;

        // Reference to the horn sound 
        public AudioSource hornSound;

        // Reference to the skid sound 
        public AudioSource skidSound;

        // Reference to the time travel sound 
        public AudioSource timeTravelSound;

        // Reference to the blast sound 
        public AudioSource blastSound;

        // Reference to the re-entry sound
        public AudioSource reentrySound;

        // Reference to the sparks particle system
        public ParticleSystem sparks;

        // Reference to both plutonium lights
        public GameObject plutoniumOn;
        public GameObject plutoniumOff;

        // Use this for initialization
        void Start()
        {
            // Current position of the car
            playerPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            // Checks if Horizontal axis changes i.e. anything other than 0
            // and multiplies it by the speed and Time.deltaTime which
            // makes it frame independent i.e. always moves at the same speed

            // If disable controls is true then the player can no longer move
            if (disableControls == false)
            {
                // Change the x position all the time
                playerPosition.x += Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;

                // Clamp the player so it doesn't go off the road
                playerPosition.x = Mathf.Clamp(playerPosition.x, minClamp, maxClamp);

                // Now change the position on the object
                transform.position = playerPosition;
            }
            else
            {
                // If you're going left or right then
                if(doRotation)
                {
                    // If you're going right keep rotating and instantiating skid marks
                    if (rotateLeft == false)
                    {
                        // Rotate the player right
                        transform.Rotate(0, 0, -500f * Time.deltaTime);

                        // Player can no longer move it but the car keeps spinning
                        Instantiate(skidMarks, transform.position, transform.rotation);
                    }
                    // If you're going left keep rotating and instantiating skid marks
                    else if (rotateLeft)
                    {
                        // Keep rotating the player left
                        transform.Rotate(0, 0, 500f * Time.deltaTime);

                        // Player can no longer move it but the car keeps spinning
                        Instantiate(skidMarks, transform.position, transform.rotation);
                    }
                }

                // If you're not turning left or right then destroy the player right away
                if (destroyPlayerImmediately)
                {
                    // Activate the BTTF gui
                    activateBTTFGUI = true;

                    // Start the time travel animation
                    GetComponent<Animator>().enabled = true;

                    // Stop the theme music
                    themeMusic.Stop();

                    // The game is now over
                    uiManager.GameOverConfirmed();

                    // Instantiate the bttf trail (after half a second) and explosion where the enemy crashed
                    // Quaternion.Euler flips the fire trails
                    StartCoroutine(BTTFTrail());

                    // Changes the speedometer to increase
                    StartCoroutine(ChangeSpeedometerIncrease());

                    // Disable the controls
                    disableControls = true;
                    // Stop the moving road
                    road.GetComponent<MoveRoad>().roadSpeed = 0;
                    // Stop the enemies spawning
                    enemySpawner.gameObject.SetActive(false);

                    // Don't allow the game to be paused
                    pauseGame.allowPause = false;

                    // Don't run this code again
                    destroyPlayerImmediately = false;
                }
                
                // If you are going to rotate do the destroying etc. after 2 seconds and disable the 
                // road moving amongst others
                if(destroyRotate)
                {
                    // Stop the theme music
                    themeMusic.Stop();
                    // Instantiate the fire where the enemy crashed
                    Instantiate(firePrefab, transform.position, transform.rotation);
                    // The game is now over
                    uiManager.GameOverConfirmed();
                    // Destroy this car
                    Destroy(gameObject, 2f);
                    // Instantiate bomb after 2.0f seconds
                    if (bombOnce)
                    {
                        StartCoroutine(InstantiateBomb());
                    }
                    // Stop the moving road
                    road.GetComponent<MoveRoad>().roadSpeed = 0;
                    // Stop the enemies spawning
                    enemySpawner.gameObject.SetActive(false);

                    // Play the skid sound once
                    if (skidSoundOnce)
                    {
                        skidSound.Play();
                        skidSoundOnce = false;
                    }

                    // Don't allow the game to be paused
                    pauseGame.allowPause = false;

                    // Don't run this code again
                    destroyRotate = false;
                }
            }

            // Create new skids marks when the re-entry is being called
            if(reentrySkidMarks)
            {
                Instantiate(skidMarks, transform.position, transform.rotation);
            }

            // If the "ESC" key is pressed then play the honk
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                hornSound.Play();
            }
        }

        // When this game object collides with another object, do something
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(disableCollisionCode == false)
            {
                if (collision.gameObject.tag == "EnemyCar")
                {
                    // Don't allow another collision to happen
                    disableCollisionCode = true;

                    // Disable the controls
                    disableControls = true;

                    // If you're moving left then do a rotation left
                    if (Input.GetAxis("Horizontal") < 0)
                    {
                        doRotation = true;
                        rotateLeft = true;
                        destroyRotate = true;
                    }

                    // If you're moving right do a rotation right
                    if (Input.GetAxis("Horizontal") > 0)
                    {
                        doRotation = true;
                        rotateLeft = false;
                        destroyRotate = true;
                    }

                    // If you're not moving, destroy the car immediately
                    if (Input.GetAxis("Horizontal") == 0)
                    {
                        destroyPlayerImmediately = true;
                    }
                }
            } 
        }

        IEnumerator InstantiateBomb()
        {
            // Don't instantiate another bomb
            bombOnce = false;

            yield return new WaitForSeconds(0.25f);

            // Instantiate another fire where the enemy crashed with some delays
            Instantiate(firePrefab, transform.position, transform.rotation);

            yield return new WaitForSeconds(0.25f);

            // Instantiate another fire where the enemy crashed
            Instantiate(firePrefab, transform.position, transform.rotation);

            yield return new WaitForSeconds(0.25f);

            // Instantiate another fire where the enemy crashed
            Instantiate(firePrefab, transform.position, transform.rotation);

            yield return new WaitForSeconds(0.25f);

            // Instantiate another fire where the enemy crashed
            Instantiate(firePrefab, transform.position, transform.rotation);

            yield return new WaitForSeconds(0.85f);

            // Instantiate the explosion and play the blast/bomb sound where the enemy crashed after 0.9f seconds
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            blastSound.Play();
        }

        // Start the entire BTTF trail sequence
        IEnumerator BTTFTrail()
        {
            // Show the speedometer
            speedometer.gameObject.SetActive(true);

            // Play the time travel sound
            timeTravelSound.Play();

            // Activate the blue explosion
            blueExplosion.SetActive(true);

            // Activate the blue gas
            blueGas.SetActive(true);

            // Set the sparks to activate
            sparks.gameObject.SetActive(true);

            // Make the car go forward smoothly as well as the blue explosion and gas
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);
            yield return new WaitForSeconds(0.02f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.1f);
            blueExplosion.transform.position = new Vector3(transform.position.x + 0.02f, transform.position.y + 1.0f);
            blueGas.transform.position = new Vector3(transform.position.x - 0.10f, transform.position.y - 0.2f);

            // Show the license plate spinning and detatch from the parent
            outatime.SetActive(true);
            outatime.transform.parent = null;

            // Disable the car
            GetComponent<SpriteRenderer>().enabled = false;

            // Disable the collider and sounds
            engineSoundObject.gameObject.SetActive(false);
            hornSoundObject.gameObject.SetActive(false);
            GetComponent<BoxCollider2D>().enabled = false;

            // Instantiate normal explosion
            Instantiate(explosionPrefab, new Vector3(transform.position.x + 0.04f, transform.position.y - 0.1f), transform.rotation);

            // Disable the blue explosion
            blueExplosion.SetActive(false);

            // Disable the blue gas
            blueGas.SetActive(false);

            // Disable the sparks
            sparks.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.45f);

            // Instantiate the smoke after the explosion and the trail
            Instantiate(smokePrefab, new Vector3(transform.position.x + 0.05f, transform.position.y - 0.7f), transform.rotation);
            Instantiate(bttfTrail, new Vector3(transform.position.x + 0.38f, transform.position.y - 2.15f), Quaternion.Euler(0, 0, 360));

            // Start re-entry after 7 seconds
            yield return new WaitForSeconds(7f);

            // Do the blue explosion flashing 3 times and start re-entry

            // Play the re-entry sound
            reentrySound.Play();

            // Move the car up so it can be shown again as well as the blue explosion
            transform.position = new Vector3(transform.position.x, transform.position.y + 6.5f);
            blueExplosion.gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.55f);

            // Enable the blue explosion and then disable the sprite after a bit
            blueExplosion.SetActive(true);
            blueExplosion.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            // Disable the blue explosion
            blueExplosion.GetComponent<SpriteRenderer>().enabled = false;

            // Wait before showing the blue explosion again
            yield return new WaitForSeconds(0.1f);

            // Enable the blue explosion and then disable after a bit
            blueExplosion.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            // Disable the blue explosion
            blueExplosion.GetComponent<SpriteRenderer>().enabled = false;

            // Wait before showing the blue explosion again
            yield return new WaitForSeconds(0.1f);

            // Enable the blue explosion and then disable after a bit
            blueExplosion.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.1f);
            // Disable the blue explosion
            blueExplosion.GetComponent<SpriteRenderer>().enabled = false;

            // Start the re-entry sequence
            GetComponent<SpriteRenderer>().enabled = true;

            // Don't show the score anymore
            scoreGameObject.SetActive(false);

            // Enable the re-entry animation
            GetComponent<Animator>().SetBool("reentry", true);

            // Show the reentry smoke front/back
            smokeReentry.SetActive(true);
            smokeReentryBack.SetActive(true);

            // Create new skid marks
            reentrySkidMarks = true;

            // Decrease the speedometer
            StartCoroutine(ChangeSpeedometerDecrease());

            // Generate a random value and dependent on the condition,
            // do either of the two rotation codes
            if (Random.value >= 0.5)
            {
                rotateLeft = true;
            }

            // Disable the BTTF text
            bttfText.gameObject.SetActive(false);

            if (rotateLeft)
            {
                // Make the car go forward smoothly and rotate left
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                transform.rotation = Quaternion.Euler(0, 0, -175);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -170);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -165);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -160);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -155);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -150);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -145);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -140);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -135);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -130);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -125);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -120);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -115);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -110);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -105);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -100);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -95);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, -90);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);

                // Don't create anymore skidmarks
                reentrySkidMarks = false;

                // Start the plutonium sequence after 5 seconds
                yield return new WaitForSeconds(4f);

                // Coroutine to switch plutonium light on/off
                StartCoroutine(PlutoniumEmpty());
            }
            else
            {
                // Make the car go forward smoothly and rotate right
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                transform.rotation = Quaternion.Euler(0, 0, 175);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 170);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 165);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 160);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 155);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 150);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 145);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 140);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 135);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 130);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 125);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 120);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 115);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 110);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 105);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 100);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 95);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);
                yield return new WaitForSeconds(0.01f);
                transform.rotation = Quaternion.Euler(0, 0, 90);
                transform.position = new Vector3(transform.position.x, transform.position.y + 0.15f);

                // Don't create anymore skidmarks
                reentrySkidMarks = false;

                // Wait a bit before alerting the player that they have ran out of plutonium
                yield return new WaitForSeconds(5f);

                // Coroutine to switch plutonium light on/off
                StartCoroutine(PlutoniumEmpty());
            }
        }

        // Changes the speedometer from 55 to 88
        IEnumerator ChangeSpeedometerIncrease()
        {
            for (int i = 55; i <= 88; i++)
            {
                speedometer.text = i.ToString();
                yield return new WaitForSeconds(0.01f);
            }
        }

        // Changes the speedometer from 88 to 0
        IEnumerator ChangeSpeedometerDecrease()
        {
            for (int i = 88; i >= 0; i--)
            {
                speedometer.text = i.ToString();
                yield return new WaitForSeconds (0.01f);
            }
        }

        // Switch the plutonium light on/off
        IEnumerator PlutoniumEmpty()
        {
            // Play the chamber empty sound a few times
            plutoniumEmptySound.Play();

            plutoniumOn.SetActive(true);
            plutoniumOff.SetActive(false);

            yield return new WaitForSeconds(0.42f);

            plutoniumOn.SetActive(false);
            plutoniumOff.SetActive(true);

            yield return new WaitForSeconds(0.42f);

            plutoniumEmptySound.Play();

            plutoniumOn.SetActive(true);
            plutoniumOff.SetActive(false);

            yield return new WaitForSeconds(0.41f);

            plutoniumOn.SetActive(false);
            plutoniumOff.SetActive(true);

            yield return new WaitForSeconds(0.41f);

            plutoniumEmptySound.Play();

            plutoniumOn.SetActive(true);
            plutoniumOff.SetActive(false);

            yield return new WaitForSeconds(0.41f);

            plutoniumOn.SetActive(false);
            plutoniumOff.SetActive(true);

            yield return new WaitForSeconds(0.41f);

            plutoniumEmptySound.Play();

            plutoniumOn.SetActive(true);
            plutoniumOff.SetActive(false);

            yield return new WaitForSeconds(0.41f);

            plutoniumOn.SetActive(false);
            plutoniumOff.SetActive(true);

            // Disable the smoke after 3 seconds
            yield return new WaitForSeconds(3f);

            smokeReentry.SetActive(false);
            smokeReentryBack.SetActive(false);
        }
    }
}