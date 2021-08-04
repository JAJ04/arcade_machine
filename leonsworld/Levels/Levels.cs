using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace LeonsWorld 
{
    public class Levels : MonoBehaviour 
    {
        // This is used to play a back click sound
        public AudioSource backSound;

        // If any of these are true then the player can choose the next level
        private bool canPlayTwo = true;
        private bool canPlayOne = true;
        // Bool to only play the back sound once
        bool playSoundOnce = true;

        // References to the buttons to change the navigation selections
        public Button levelOne;
        public Button levelBoss;
        public Button levelTwo;
        public Button back;

        void Update() 
	{
            // If the "LEFT ALT" key is pressed then reload the main menu
            if (Input.GetKeyDown(KeyCode.LeftAlt))
	    {
                StartCoroutine(GoBackToMenu());
            }

            if (canPlayOne) 
	    {
                // Check if the first level button is clicked and if it is move the level
                levelOne.onClick.AddListener(() => 
		{
                    canPlayTwo = false;
                });
            }

            // Check if Level Unlock is >= 2; if it is show the Boss Level Button and
            // change the "Level Two" button and "Level One" button navigations
            if (PlayerPrefs.GetInt("LevelUnlock", 0) >= 2) 
	    {
                // Show the button so that it can move to the boss level
                levelBoss.GetComponent < Image > ().enabled = true;
                levelBoss.GetComponentInChildren < Text > ().enabled = true;

                // Change the navigation selection of the "Level One" and "Two" button
                Navigation customNav = new Navigation();
                customNav.mode = Navigation.Mode.Explicit;
                customNav.selectOnDown = levelBoss;
                customNav.selectOnUp = back;
                levelOne.navigation = customNav;

                Navigation customNav2 = new Navigation();
                customNav2.mode = Navigation.Mode.Explicit;
                customNav2.selectOnUp = levelBoss;
                customNav2.selectOnDown = back;
                levelTwo.navigation = customNav2;
            } 
	    else 
	    {
                // Don't show the button so that it can move to the boss level
                levelBoss.GetComponent<Image>().enabled = false;
                levelBoss.GetComponentInChildren<Text>().enabled = false;
            }

            if (canPlayTwo) 
	    {
                if (PlayerPrefs.GetInt("LevelUnlock", 0) == 3) 
		{
                    // If it isn't locked add remove (locked) next to the button text
                    levelTwo.GetComponentInChildren<Text>().text = "Level Two";

                    // Check if the second level button is clicked and if it is move the level
                    levelTwo.GetComponent<Button>().onClick.AddListener(() => 
		    {
                        canPlayOne = false;
                    });
                } 
		else 
		{
                    // Else if it is locked add a (Locked) next to the button text
                    levelTwo.GetComponentInChildren<Text>().text = "Level Two (Locked)";
                }
            }
        }

        // Play a sound and go to the quit menu
        IEnumerator GoBackToMenu() 
	{
            // Play the sound only once
            if (playSoundOnce)
	    {
                backSound.Play();
                playSoundOnce = false;
            }

            yield return new WaitForSeconds(0.25 f);

            SceneManager.LoadScene("menu");
        }
    }
}
