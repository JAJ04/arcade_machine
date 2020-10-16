using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MartianAttack
{
    public class LeaderBoard : MonoBehaviour
    {
        // Array that stores all of the high scores
        public Text[] _highScores;

        // Array that stores all of the boss multiplier values
        public Text[] _bossMultipliersText;

        // Store the high score values
        private int[] _highScoreValues;

        // Store the boss multiplier values
        private int[] _bossMultipliers;

        // This stores the usernames
        private string[] _highScoreNames;

	    // Use this for initialization
	    void Start ()
        {
            // Initialize array
            _highScoreValues = new int[_highScores.Length];
            // Initialize array for boss multipliers
            _bossMultipliers = new int[_highScores.Length];
            // Initialize array for names
            _highScoreNames = new string[_highScores.Length];

            for (int x = 0; x < _highScores.Length; x++)
            {
                // Get the high score values
                _highScoreValues[x] = PlayerPrefs.GetInt("highScoreValues" + x);
                // Get the boss multiplier values
                _bossMultipliers[x] = PlayerPrefs.GetInt("bossMultipliers" + x);
                // Get the high score usernames
                _highScoreNames[x] = PlayerPrefs.GetString("highScoreNames" + x);
            }

            // 0s will pop up when there is nothing in PlayerPrefs at first
            DrawScores();
        }

        void SaveScores()
        {
            // Store the scores even after the game has ended
            for (int x = 0; x < _highScores.Length; x++)
            {
                // Set the high score values
                PlayerPrefs.SetInt("bossMultipliers" + x, _bossMultipliers[x]);
                // Set the high score values
                PlayerPrefs.SetInt("highScoreValues" + x, _highScoreValues[x]);
                // Set the user names
                PlayerPrefs.SetString("highScoreNames" + x, _highScoreNames[x]);
            }
        }

        public void CheckForHighScore(int value, string userName, int bossMultiplier)
        {
            for(int x = 0; x < _highScores.Length; x++)
            {
                // Enter a new score
                if(value >= _highScoreValues[x])
                {
                    // Move the score up for the new score to go in
                    // its place

                    // 100
                    // 50
                    // 30
                    // 25
                    // 10
                    // 5

                    // Go to the bottom of the list first i.e. 5
                    // Move elements less than the value put in down
                    // By copying the elements less than the element
                    // down by 1
                    // Six elements are not allowed
                    for(int y = _highScores.Length - 1; y > x; y--)
                    {
                        // Place new score into where it should be
                        _highScoreValues[y] = _highScoreValues[y - 1];
                        // Place new name where it should be
                        _highScoreNames[y] = _highScoreNames[y - 1];
                        // Place new boss multiplier where it should be
                        _bossMultipliers[y] = _bossMultipliers[y - 1];
                    }

                    // Amend the highscore and username
                    _highScoreValues[x] = value;
                    _highScoreNames[x] = userName;
                    _bossMultipliers[x] = bossMultiplier;

                    DrawScores();
                    SaveScores();

                    break;
                }
            }
        }

        void DrawScores()
        {
            for (int x = 0; x < _highScores.Length; x++)
            {
                // Display the scores

                // If there is a blank in _highScoreNames then display "NO NAME" instead
                if(_highScoreNames[x].ToString() == "")
                {
                    _highScores[x].text = "NO NAME" + ": " + _highScoreValues[x].ToString();
                    _bossMultipliersText[x].text = _bossMultipliers[x].ToString();
                }
                else // display the name in the array
                {
                    // Remove space as that is what is used to confirm the name
                    _highScores[x].text = _highScoreNames[x].ToString() + ": " + _highScoreValues[x].ToString();
                    _bossMultipliersText[x].text = _bossMultipliers[x].ToString();
                }
            }
        }
    }
}

