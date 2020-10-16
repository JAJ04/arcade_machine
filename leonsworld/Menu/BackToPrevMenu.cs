using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LeonsWorld
{
    public class BackToPrevMenu : MonoBehaviour
    {
        // Variable to select the top of the first menu
        public GameObject mainMenuButton;

        // Reference to the select sound 
        public AudioSource buttonSelectSound;

        // Goes back to the main menu when the 
        // game is being played i.e. the one
        // before the weapons selected menu
        public void BackToPrevMenuFunction()
        {
            // Play the select sound
            buttonSelectSound.Play();

            // Select the button weps page button
            StartCoroutine(HighlightFirstButton());

            GameManager.pauseMenu = true;
            GameManager.weaponsMenu = false;
        }

        // Select and deselect the button with the EventSystem
        IEnumerator HighlightFirstButton()
        {
            EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            es.SetSelectedGameObject(null);
            yield return null;
            es.SetSelectedGameObject(mainMenuButton);
        }
    }
}