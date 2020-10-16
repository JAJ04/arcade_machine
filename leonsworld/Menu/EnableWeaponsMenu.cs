using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LeonsWorld
{
    public class EnableWeaponsMenu : MonoBehaviour
    {
        // Holds the button in a variable
        public GameObject buttonWepsPage;
        public GameObject nerfButton;
        public GameObject aerosolButton;
        public GameObject flameThrower;
        public GameObject backMenu;

        // Variable to mute the button select sound on the next menu
        public AudioSource buttonSelectSound;

        // Enables the weapon menu and disables
        // the main pause menu
        public void ToggleWeaponsMenuBool()
        {
            GameManager.weaponsMenu = true;
            GameManager.pauseMenu = false;

            // Set the buttons to active to show them
            nerfButton.SetActive(true);
            aerosolButton.SetActive(true);
            flameThrower.SetActive(true);
            backMenu.SetActive(true);

            // Select the button weps page button at the top
            buttonWepsPage.GetComponent<Button>().Select();

            // Play a sound
            buttonSelectSound.Play();

            // Play a sound
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
        }
    }
}