using UnityEngine;
using System.Collections;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

namespace RogueKnight
{
    // "Knight" inherits from MovingElement
    public class Knight : MovingElement
    {
        public SpriteRenderer spriteRenderer;
        // Adds more energy when the knight picks up a cake
        public int pointsPerCake = 15;

        //Delay time to restart level
        public float levelRestartDelay = 1f;

        // The amount of damage a knight does to a wall when stabbing it
        public int damagedWall = 1;

        // Adds more energy when the knight picks up wine
        public int pointsPerWine = 25;

        // This is used to store the knight energy points total during the level
        private int Energy
        {
            get { return GameManager.energyPoints; }
            set { GameManager.energyPoints = value; }
        }

        // Text to display how much energy you have
        public Text energyText;

        // References to "AudioClips" to play sounds for a variety of different reasons
        public AudioClip moveSound1;
        public AudioClip moveSound2;
        public AudioClip eatSound1;
        public AudioClip eatSound2;
        public AudioClip drinkSound1;
        public AudioClip drinkSound2;
        public AudioClip gameOverSound;

        // Reference to access the knight animations
        private Animator animator;

        // This will override the Start function in "MovingObject"
        protected override void Start()
        {
            // Gets a reference to the Knight's "animator" component
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            // Gets the current energy point total stored in GameManager.instance and is shared between levels
            Energy = GameManager.energyPoints;
            // Sets the energyText to reflect the current energy total
            energyText.text = "Energy: " + Energy;

            // Calls the "Start" function of the "MovingObject" base class
            base.Start();
        }

        private void Update()
        {
            // If it isn't the knight's turn then don't run this function anymore
            if (!GameManager.gmInstance.knightsTurn)
            {
                return;
            }

            // These variables are used to store the horizontal and vertical direction
            int vertical = 0;
            int horizontal = 0; 

            // Assigns "horizontal" and "vertical" to the axes in Unity
            vertical = (int)(Input.GetAxisRaw("Vertical"));
            horizontal = (int)(Input.GetAxisRaw("Horizontal"));

            // If moving horizontally, you aren't moving vertically
            if (horizontal != 0)
            {
                vertical = 0;
            }

            // Flip the knight's sprite depending on "horizontal"
            if (horizontal > 0)
            {
                spriteRenderer.flipX = false;
            }

            if (horizontal < 0)
            {
                spriteRenderer.flipX = true;
            }

            //Check if we have a non-zero value for horizontal or vertical
            if (horizontal != 0 || vertical != 0)
            {
                // Invoke "TryMove" whilst passing in the param "BreakableWall," since that is what the Knight might encounter 
                // Pass in the "horizontal" and "vertical" as params to tell the direction to move the Knight in
                TryMove<BreakableWall>(horizontal, vertical);
            }
        }

        // Takes a generic "T" which will be of type "Wall," it also takes the integers for the x and y direction to move in
        protected override void TryMove<T>(float x, float y)
        {
            // Each time the knight moves, subtract the energy points
            Energy--;

            // Update the food text display to reflect the current amount of energy
            energyText.text = "Energy: " + Energy;

            // Call the "TryMove" function in the base class by passing in the generic "T" (the wall) and the x and y to move in
            base.TryMove<T>(x, y);

            // RaycastHit2D allows a reference to the result of the Linecast done in "MoveObject"
            RaycastHit2D raycastHit;

            // If "MoveObject" does return true then the knight can move into an empty space
            if (MoveElement(x, y, out raycastHit))
            {
                // This will play a random sound, either moveSound1/2
                SoundManager.instance.RandomSoundEffect(moveSound1, moveSound2);
            }

            // Check to see if the knight has no cake points left
            IsGameOver();

            // The knight's turn is now over, so they cannot move when the enemies are moving
            GameManager.gmInstance.knightsTurn = false;
        }

        // This function takes a generic "T" which in this scenario is the "Wall"
        protected override void IfCantMove<T>(T component)
        {
            // Set hitWall to equal the component passed in as the param
            BreakableWall wallHit = component as BreakableWall;

            // Call the HitWall function of the Wall we are now hitting
            wallHit.HitWall(damagedWall);

            // Set it so the knight is now stabbing
            animator.SetTrigger("knightStab");
        }

        // This reloads the scene
        private void Restart()
        {
            GameManager.dungeon++;
            // Load the last scene loaded and load it in "Single" mode to replace the existing one
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }


        // This is called when any ghost attacks the knight
        public void LoseEnergy(int energyLoss)
        {
            // Set it so the knight has been hurt
            animator.SetTrigger("knightHurt");

            //Subtract energyLoss from the total amount of energy
            Energy -= energyLoss;

            // Update the food display with the most up-to-date energy total
            energyText.text = "-" + energyLoss + " Energy: " + Energy;

            // Check to see if game has been ended
            IsGameOver();
        }


        // If there is no more energy left then play a sound and end the game
        private void IsGameOver()
        {
            if (Energy <= 0)
            {
                SoundManager.instance.musicSource.Stop();
                GameManager.gmInstance.GameOver();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Check if the knight collided with a game object with the tag "NextDungeon"
            if (other.tag == "NextDungeon")
            {
                // Call the "Restart" function to start the next level with a delay of restartLevelDelay
                Invoke("Restart", levelRestartDelay);

                // Disable the knight object since the level is now over
                enabled = false;
            }

            // If the knight collected some cake
            else if (other.tag == "Cake")
            {
                // Add some energy
                Energy += pointsPerCake;

                // Update the energyText to show the current amount of energy that the knight possesses
                energyText.text = "+" + pointsPerCake + " Energy: " + Energy;

                // Invoke the RandomSoundEffect function pass in two eat sounds to choose from
                SoundManager.instance.RandomSoundEffect(eatSound1, eatSound2);

                // Don't show the cake the knight collided with
                other.gameObject.SetActive(false);
            }

            // Same as above but for the wine
            else if (other.tag == "Wine")
            {
                Energy += pointsPerWine;

                energyText.text = "+" + pointsPerWine + " Energy: " + Energy;

                SoundManager.instance.RandomSoundEffect(drinkSound1, drinkSound2);

                other.gameObject.SetActive(false);
            }
        }
    }
}