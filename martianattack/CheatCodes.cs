using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MartianAttack
{
    public class CheatCodes : MonoBehaviour
    {
        // Code taken from this link and adapted to my liking:
        // https://answers.unity.com/questions/553597/how-to-implement-cheat-codes.html

        // This code is consistently used throughout multiple scenes to give 
        // the player credits for the main game "Martian Attack"

        // Data holders for cheatArray
        private string[] cheatArray;
        public static int arrayIndex;

        // Data holders for "C" key array
        private string[] cArray;
        public static int cArrayIndex;

        // Data holders for resetting PlayerPrefs
        private string[] playerPrefsArray;
        public static int playerPrefsIndex;

        // Variable created to access the game manager
        GameManager _gameManager;

        // Variable created to access the score
        UIController _uIController;

        // Text object to change the credit text
        private Text _textCredits;

        // Bool to prevent spamming of credits
        public bool oneUp = true;

        // Bool to allow/disallow PlayerPrefs deletion
        bool sceneNameMartian = false;

        // This is used to allow access of the game
        [SerializeField]
        public static int credits = 0;

        // AudioClip when a credit is added
        [SerializeField]
        private AudioClip _coinClip;

        // Variable that gets the reference to the current scene
        Scene _currentScene;

        // Variable to hold the current scene
        string sceneName;

        void Start()
        {            
            // User needs to input this in the right order
            cheatArray = new string[] {"escape", "m", "escape", "m", "escape" };
            arrayIndex = 0;

            // Code is "c", user needs to input this in the right order, this is used to 
            // make the credits increase even when Time.timeScale is 0
            cArray = new string[] { "c" };
            cArrayIndex = 0;

            // User needs to input this in the right order, this is used to 
            // delete all of the scores
            playerPrefsArray = new string[] { "escape", "escape", "m", "m" };
            playerPrefsIndex = 0;

            // Start flashing 
            Invoke("flashOff", 1f);
        }

        void Update()
        {
            // Keep on updating these variables to check what the scene is
            _currentScene = SceneManager.GetActiveScene();

            sceneName = _currentScene.name.ToString();

            // Only do this if the scene is the "game"
            if (sceneName == "game")
            {
                // Set the _gameManager variable
                _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            }

            // Only do this if the scene is the "game"
            if (sceneName == "game")
            {
                // Set the _uIController variable
                _uIController = GameObject.Find("Canvas").GetComponent<UIController>();
            }

            // Only do this if the scene is specified below
            if (sceneName == "game" || sceneName == "scoreboard" 
                || sceneName == "main-screen-ufo" || sceneName == "ufo"
                || sceneName == "godMenu" || sceneName == "godMenuHidden"
                || sceneName == "dodge-em-menu" || sceneName == "dodge-em"
                || sceneName == "kitchen-khaos" || sceneName == "welcome"
                || sceneName == "rogue-knight-easter-egg" || sceneName == "rogue-knight-menu"
                || sceneName == "rogue-knight" || sceneName == "cutscene1FadeIn"
                || sceneName == "cutscene1FadeOut" || sceneName == "cutscene2FadeIn"
                || sceneName == "cutscene2FadeOut" || sceneName == "cutscene12FadeIn"
                || sceneName == "cutscene12FadeOut" || sceneName == "endcredits"
                || sceneName == "level1" || sceneName == "level1-boss"
                || sceneName == "level2" || sceneName == "levelselect"
                || sceneName == "menu")
            {
                // Initialize _textCredits
                _textCredits = GameObject.Find("Credits_Text").GetComponent<Text>();
            }

            // Keep updating Credits text
            if (_textCredits != null)
            {
                _textCredits.text = "Credits: " + credits;
            }

            // Show credits info immediately if paused for game
            if(sceneName == "game")
            {
                // Set sceneNameMartian to true so you can delete PlayerPrefs
                sceneNameMartian = true;

                if (_gameManager.pauseMenuActive)
                {
                    _textCredits.enabled = true;
                }
            }
            else
            {
                // Set sceneNameMartian to false so you can't delete PlayerPrefs
                sceneNameMartian = false;
            }

            // Show credits info immediately if paused for game
            if (sceneName == "scoreboard")
            {
                if (SceneController.pauseMenuActive)
                {
                    _textCredits.enabled = true;
                }
            }

            // Play "Credit" information such as sound and reset state
            if (Input.GetKey(KeyCode.C) && oneUp == true)
            {
                // Prevent spamming
                oneUp = false;
                // Set oneUp to true after a bit
                Invoke("OneUpTrue", 0.25f);
            }

            // Check if any key is pressed
            if (Input.anyKeyDown)
            {
                // Check if the next key in the code is pressed
                if (Input.GetKeyDown(cheatArray[arrayIndex]))
                {
                    // Add 1 to index to check the next key in the code
                    arrayIndex++;
                }
                // Wrong key entered, reset arrayIndex back to 0 to start again
                else
                {
                    arrayIndex = 0;
                }

                // Same as above but for "C," ignores time.timeScale somehow...
                // Check if the next key in the code is pressed
                if (Input.GetKeyDown(cArray[cArrayIndex]))
                {
                    // Add 1 to index to check the next key in the code
                    cArrayIndex++;
                }
                // Wrong key entered, reset arrayIndex back to 0 to start again
                else
                {
                    cArrayIndex = 0;
                }

                // Allow the deletion of the PlayerPrefs only on the main screen and if it is in the "game" scene
                if (GameManager.gameOver && SceneController.pauseMenuActive == false && sceneNameMartian == true)
                {
                    // Same as above but for resetting Player prefs
                    // Check if the next key in the code is pressed
                    if (Input.GetKeyDown(playerPrefsArray[playerPrefsIndex]))
                    {
                        // Add 1 to index to check the next key in the code
                        playerPrefsIndex++;
                    }
                    // Wrong key entered, reset arrayIndex back to 0 to start again
                    else
                    {
                        playerPrefsIndex = 0;
                    }
                }
            }

            // If index reaches the length of the cheatArray string, 
            // the entire code was correctly entered
            if (arrayIndex == cheatArray.Length)
            {
                // Play Coin Clip
                PlayCoin();
                // Cheat code successfully inputted!
                // Add credits
                credits++;
                arrayIndex = 0;
            }

            // Same as above but for "C," this ignores time.timeScale somehow...
            // If index reaches the length of the cheatArray string, 
            // the entire code was correctly entered
            if (cArrayIndex == cArray.Length)
            {
                // Play Coin Clip
                PlayCoin();
                // Cheat code successfully inputted!
                // Add credits
                credits++;
                cArrayIndex = 0;
            }

            // Same as above but for resetting Player Prefs
            // If index reaches the length of the cheatArray string, 
            // the entire code was correctly entered
            if (playerPrefsIndex == playerPrefsArray.Length)
            {
                PlayerPrefs.DeleteKey("Hiscore");

                // Delete leaderboard scores
                PlayerPrefs.DeleteKey("highScoreValues0");
                PlayerPrefs.DeleteKey("bossMultipliers0");
                PlayerPrefs.DeleteKey("highScoreNames0");
                PlayerPrefs.DeleteKey("highScoreValues1");
                PlayerPrefs.DeleteKey("bossMultipliers1");
                PlayerPrefs.DeleteKey("highScoreNames1");
                PlayerPrefs.DeleteKey("highScoreValues2");
                PlayerPrefs.DeleteKey("bossMultipliers2");
                PlayerPrefs.DeleteKey("highScoreNames2");
                PlayerPrefs.DeleteKey("highScoreValues3");
                PlayerPrefs.DeleteKey("bossMultipliers3");
                PlayerPrefs.DeleteKey("highScoreNames3");
                PlayerPrefs.DeleteKey("highScoreValues4");
                PlayerPrefs.DeleteKey("bossMultipliers4");
                PlayerPrefs.DeleteKey("highScoreNames4");

                UIController.hiScoreInt = 0;
                _uIController.hiScore.text = "Hiscore: " + UIController.hiScoreInt;
                playerPrefsIndex = 0;
            }
        }

        // Sets oneUp to true
        private void OneUpTrue()
        {
            oneUp = true;
        }

        // Methods to flash the text on/off
        private void flashOn()
        {
            if (_textCredits != null)
            {
                _textCredits.GetComponent<Text>().enabled = true;
            }
            Invoke("flashOff", 0.5f);
        }

        private void flashOff()
        {
            if(_textCredits != null)
            {
                _textCredits.GetComponent<Text>().enabled = false;
            }
            Invoke("flashOn", 0.5f);
        }

        public void PlayCoin()
        {
            // Start Credit Sound Clip
            float lastTimeScale = Time.timeScale;
            Time.timeScale = 1f;
            AudioSource.PlayClipAtPoint(_coinClip, Camera.main.transform.position, 1f);
            Time.timeScale = lastTimeScale;
        }
    }
}