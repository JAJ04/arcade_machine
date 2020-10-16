using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MartianAttack
{
    public class UIController : MonoBehaviour
    {
        // Bool to set pausing to true/false if countdown screen is enabled/disables
        public static bool countdownScreen = true;

        // Variable to keep track of the score
        public static int score = 0;
        // Variable to keep track of how many boss spaceships you killed
        public static int bossSpaceshipCounter = 0;
        // Variable to keep track of the hiscore
        public static int hiScoreInt = 0;
        // Variable for holding the randomly generated boss score to update the text on screen
        public static int bossScore;

        // Variable to change the lives sprite
        public Image livesImage;

        // Array to hold the array of sprites
        public Sprite[] livesArray;

        // Variable for the score text
        public Text scoreText;
        // Variable for the high score text
        public Text hiScore;

        // Variable to hold the Main Screen animation
        public GameObject mainScreenAnim;
        // Variable to hold the last alien sprite
        public GameObject lastAlien;
        // GameObject variable to hold the title screen
        public GameObject mainScreen;

        // Variable to increase the score when the boss is killed
        private int[] _bossScoreArray = new int[4] { 50, 100, 150, 300 };

        // Load PlayerPrefs data using Start() method
        private void Start()
        {
            hiScoreInt = PlayerPrefs.GetInt("Hiscore", 0);
        }

        // Method to update the score
        public void ScoreUpdate()
        {
            // Increase score by 10
            score += 10;
            // Update score variable and put it on the screen
            scoreText.text = "Score: " + score;
        }

        public void ScoreUpdateSpecial()
        {
            // Same as above but get a random element
            bossScore = _bossScoreArray[Random.Range(0, _bossScoreArray.Length)];
            score += bossScore;
            scoreText.text = "Score: " + score;
        }

        public void ScoreUpdateSpecialDouble()
        {
            // Same as above but get a random element and double the score again
            score += bossScore;
            scoreText.text = "Score: " + score;
        }

        // Method to update the lives
        public void LivesUpdate(int curLives)
        {
            livesImage.sprite = livesArray[curLives];
        }

        public void UpdateHiscore()
        {
            // If the score is greater than or equal to the hiScore
            // set hiScore to the score
            if(score >= hiScoreInt)
            {
                hiScoreInt = score;
                hiScore.text = "Hiscore: " + hiScoreInt;
                PlayerPrefs.SetInt("Hiscore", hiScoreInt);
            }
        }

        // Method to show the title screen
        public void ShowMainScreen()
        {
            // Sets the start anim to true when game is over
            mainScreenAnim.GetComponent<MainMenuAnimation>().startAnim = true;
            // Sets the main screen object to true to show the screen
            mainScreen.gameObject.SetActive(true);
            // Sets the last alien to true to enable the sprite that doesn't get disabled
            lastAlien.gameObject.SetActive(true);
            // Bool to set countdown screen to true 
            countdownScreen = true;
        }

        // Method to hide the title screen
        public void HideMainScreen()
        {
            // Reset the score and hiscore on the screen and the variable for score
            score = 0;
            scoreText.text = "Score: 0";
            hiScore.text = "Hiscore: " + hiScoreInt;

            // Do the opposite of the "ShowMainScreen()" essentially
            mainScreen.gameObject.SetActive(false);
            lastAlien.gameObject.SetActive(false);

            // Bool to set countdown screen to false 
            countdownScreen = false;
        }
    }
}