using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

namespace RogueKnight 
{
    // REFERENCE: https://unity3d.com/learn/tutorials/s/2d-roguelike-tutorial
    // The tutorial above was used as a base to which to make modifications from						

    public class GameManager: MonoBehaviour 
    {
        // Static variable to hold the amount of chests collected
        public static int easterEggCount;
        // This is the amount of energy the knight will have in the start
        public static int energyPoints = 200;
        // The level you are in
        public static int dungeon = 1;

        // This is a static variable for an instance of this GameManager which will allow other scripts to access the properties of this script
        public static GameManager gmInstance = null;

        // This is the time to delay before starting a dungeon
        public float dungeonStartDelay = 2 f;
        // This is used to delay the knight movement
        public float knightTurnDelay = 0.05 f;

        // This is used to display the letter image
        private GameObject dungeonImage;
        // This is used to display what dungeon you're in
        private Text dungeonText;
        // This is used to display when you're allowed to restart
        private Text restartText;
        // This is used to get the properties and functions of the dungeon board manager
        private DungeonBoardManager dungeonBoardManager;

        // This is the list of ghosts that are in the game
        private List <Ghost> ghosts;

        // This is ture if the dungeon board is being set up
        private bool settingBoard = true;
        // This checks to see if the ghosts are moving to allow the knight to move or not
        private bool ghostsMoving;
        // Bool to only allow the catapult sound when restarting to play once
        private bool catapultSoundPlay = true;
        // Allow the god menu sound to be only played once
        private bool soundPlayOnce = true;
        // This allows a restart to happen or not
        private bool restartGame = false;
        // Bool to allow the player to go back to the god menu
        private bool goBackToGodMenuBool = false;

        // Reference to the catapult sound
        public AudioSource catapultSound;
        // Reference to the back audio source to play 
        public AudioSource backSound;

        // Reference to the "AllowPause" script to not allow pausing when the game is over
        public PauseGame pauseGame;

        // This is used to check if the knight can move, this is public but will not show in the inspector
        [HideInInspector]
        public bool knightsTurn = true;

        // Awake is the very first thing to be called in scripts
        void Awake() 
	{
            // Check to see if an instance of "THIS" script exists
            if (gmInstance == null) 
	    {
                // Create an instance if an instance does not exist
                gmInstance = this;
            }
            // If an instance does already exist then
            else if (gmInstance != this) 
	    {
                // Destroy the instance which enforces the singleton design pattern
                Destroy(gameObject);
                enabled = false;
                return;
            }

            // This "GameManager" will be carried across all scenes
            DontDestroyOnLoad(gameObject);

            // Assign all the ghosts to a list
            ghosts = new List <Ghost>();

            // Gets a reference to the "DungeonBoardManager" script
            dungeonBoardManager = GetComponent<DungeonBoardManager>();

            // This calls InitialiseGame function to start the first dungeon
            InitializeGame();
        }

        //Update is called every frame.
        void Update() {
            // If you're allowed to now go back to the menu, then you can if you press "LeftAlt"
            if (goBackToGodMenuBool) 
	    {
                if (Input.GetKeyDown(KeyCode.LeftAlt)) 
		{
                    StartCoroutine(GoBackToGodMenu());
                }
            }

            // If restartGame is true then
            if (restartGame)
	    {
                if (Input.GetKeyDown(KeyCode.Space)) 
		{
                    InitializeGame();

                    // Destroy the sound when you want to restart the game and reset the easter egg count
                    easterEggCount = 0;

                    // Only destroy this game object if it exists
                    if (GameObject.Find("SoundPrefab(Clone)")) 
		    {
                        Destroy(GameObject.Find("SoundPrefab(Clone)").gameObject);
                    }

                    // Don't allow the player to restart the game
                    restartGame = false;

                    // Go to the "welcome" scene
                    StartCoroutine(LoadWelcome());
                }
            }

            // Check to see if you collected 10 crates and if you have change to the easter egg scene
            if (easterEggCount >= 10) 
	    {
                SceneManager.LoadScene("rogue-knight-easter-egg");
            }

            // This checks to see if any of these "OR" conditions are true
            if (knightsTurn || ghostsMoving || settingBoard) 
	    {
                // Do not move any ghosts if any of these "OR" conditions are true
                return;
            }

            // If the above statement is false, move the ghosts
            StartCoroutine(MoveGhosts());
        }

        void InitializeGame() 
	{
            // Reset the static variables and reload the scene
            dungeon = 1;
            easterEggCount = 0;
            energyPoints = 200;
        }

        // Initializes the game for each new dungeon
        void InitializeLevel() 
	{
            // Set the knight to not move
            knightsTurn = false;

            // If settingBoard is true the knight will not be allowed to move
            settingBoard = true;

            // This finds the dungeon image in the scene
            dungeonImage = GameObject.Find("DungeonImage");

            // This finds the dungeon text in the scene
            dungeonText = GameObject.Find("DungeonText").GetComponent < Text > ();

            // This gets the dungeon text in the game
            dungeonText.text = "Dungeon " + dungeon;

            // This shows the dungeon text when the game first starts
            dungeonImage.SetActive(true);

            // This will hide the dungeon image after a little delay
            Invoke("HideDungeonImage", dungeonStartDelay);

            // Prepare for the next level, so get rid of all of the other ghosts so that new ones can be generated
            ghosts.Clear();

            // Invoke the "SetupDungeon" in the "Dungeon Board Manager"
            dungeonBoardManager.SetupDungeon(dungeon);

        }

        // This will move the ghosts in sequence
        IEnumerator MoveGhosts() 
	{
            // This will disable the knight from moving
            ghostsMoving = true;

            // Move the ghosts after the knight's turn delay is up
            yield
            return new WaitForSeconds(knightTurnDelay);

            // If the number if ghosts in the first level is 0 then
            if (ghosts.Count == 0) 
	    {
                // This will replace the delay that causes the enemies to move when there are no ghosts
                yield
                return new WaitForSeconds(knightTurnDelay);
            }

            // Go through the ghost list
            for (int i = 0; i < ghosts.Count; i++) 
	    {
                // Move every single ghost in the list
                ghosts[i].MoveGhost();

                // Wait before moving the next ghost by movingElementTime
                yield return new WaitForSeconds(ghosts[i].movingElementTime);
            }

            // The knight can now move when the ghosts are no longer moving
            knightsTurn = true;

            // The ghosts are now also no longer moving
            ghostsMoving = false;
        }

        // This will be invoked when you need to add a new ghost to the list
        public void AddGhostToList(Ghost script) 
	{
            ghosts.Add(script);
        }

        // This will hide the dungeon image between each different dungeon
        void HideDungeonImage() 
	{
            dungeonImage.SetActive(false);
            settingBoard = false;
        }

        // All of this will happen when the knight has no more energy points
        public void GameOver() 
	{
            // This will show that you survived for x amount of dungeons
            dungeonText.text = "After " + dungeon + " dungeons, \nyour quest ended.";

            // It will show the dungeon image
            dungeonImage.SetActive(true);

            // Don't allow the player to move
            GameObject.Find("Knight").GetComponent < Knight > ().enabled = false;

            // Bool to allow the coroutine to go back to the god menu to start
            goBackToGodMenuBool = true;

            // Don't allow the player to pause the game when the game is over
            pauseGame = GameObject.Find("PauseGame").GetComponent < PauseGame > ();
            pauseGame.gameObject.SetActive(false);

            // Show the text to restart the game and allow a restart after a second
            StartCoroutine(AllowRestart());
        }

        IEnumerator AllowRestart() 
	{
            // Wait for one second and then allow a restart and show the restart text
            yield
            return new WaitForSeconds(1 f);

            restartText = GameObject.Find("RestartText").GetComponent < Text > ();;
            restartText.enabled = true;

            restartGame = true;
        }

        // Plays a sound and goes to "welcome" after a second
        IEnumerator LoadWelcome() 
	{
            catapultSound = GameObject.Find("CatapultSound").GetComponent < AudioSource > ();

            // Only play this sound once
            if (catapultSoundPlay) 
	    {
                catapultSound.Play();
                catapultSoundPlay = false;
            }

            yield return new WaitForSeconds(1 f);

            SceneManager.LoadScene("welcome");
            catapultSoundPlay = true;
        }

        IEnumerator GoBackToGodMenu() 
	{
            // Capture the backSound if it exists
            if (GameObject.Find("BackSound")) 
	    {
                backSound = GameObject.Find("BackSound").GetComponent < AudioSource > ();
            }

            // Play a sound (only once) and switch scenes after 0.25f seconds
            if (soundPlayOnce) 
	    {
                backSound.Play();
                soundPlayOnce = false;
            }

            yield return new WaitForSeconds(0.25 f);

            SceneManager.LoadScene("godMenu");
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        // Set the energy points variable to 200 before the scene loads
        static public void GameLoadCallback() 
	{
            energyPoints = 200;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static public void CallbackInitialization() 
	{
            // Registers a callback to be called everytime a scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        // This function is called everytime you go to a next dungeon
        static private void OnSceneLoaded(Scene sceneName, LoadSceneMode sceneMode) 
	{
            if (sceneName.name == "rogue-knight") 
	    {
                gmInstance.InitializeLevel();
            }
        }
    }
}
