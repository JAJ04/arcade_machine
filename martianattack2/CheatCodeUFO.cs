using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UFO
{
    public class CheatCodeUFO : MonoBehaviour
    {
        // Data holders for cheatArray
        private string[] deleteUFOPrefsArray;
        public static int UFOArrayIndex;

        // Reference to the text hiscore
        public Text hiScore;

        // Reference to the game manager
        public GameManager gameManager;

        // Use this for initialization
        void Start()
        {
            // User needs to input this in the right order
            deleteUFOPrefsArray = new string[] { "escape", "escape", "m", "m" };
            UFOArrayIndex = 0;
        }

        // Update is called once per frame
        void Update()
        {
            // Only allow the cheat code to be entered when the game is not started
            if(gameManager.gameStartedGM == false)
            {
                if (Input.anyKeyDown)
                {
                    // Check if the next key in the code is pressed
                    if (Input.GetKeyDown(deleteUFOPrefsArray[UFOArrayIndex]))
                    {
                        // Add 1 to index to check the next key in the code
                        UFOArrayIndex++;
                    }
                    // Wrong key entered, reset arrayIndex back to 0 to start again
                    else
                    {
                        UFOArrayIndex = 0;
                    }

                    // Delete the player prefs and change the text on the screen
                    if (UFOArrayIndex == deleteUFOPrefsArray.Length)
                    {
                        PlayerPrefs.SetInt("UFO Hiscore", 0);

                        // Concatenate "Hiscore: "
                        hiScore.text = "Hiscore: " + PlayerPrefs.GetInt("UFO Hiscore").ToString();

                        UFOArrayIndex = 0;
                    }
                }
            }
        }
    }
}