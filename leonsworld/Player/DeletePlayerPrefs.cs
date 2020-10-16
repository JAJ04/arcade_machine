using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeonsWorld
{
    public class DeletePlayerPrefs : MonoBehaviour
    {
        // Variable to play the click sound
        public AudioSource click;

        // This script plays a "Click" and deletes all of the
        // associated PlayerPrefs to Leon's World
        public void DeletePlayerPrefsFunction()
        {
            PlayerPrefs.DeleteKey("Player Health");
            PlayerPrefs.DeleteKey("Player Exp");
            PlayerPrefs.DeleteKey("Player Level");
            PlayerPrefs.DeleteKey("Player Coins");
            PlayerPrefs.DeleteKey("Player Damage");
            PlayerPrefs.DeleteKey("LevelUnlock");
            PlayerPrefs.DeleteKey("Weapon1");
            PlayerPrefs.DeleteKey("Weapon2");

            // Play a click sound to confirm deletion
            click.Play();
        }
    }
}