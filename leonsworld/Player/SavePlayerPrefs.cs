using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeonsWorld
{
    public class SavePlayerPrefs : MonoBehaviour
    {
        // Variable to play the click sound
        public AudioSource click;

        public void SaveGame()
        {
            // This saves values to the PlayerPrefs so that after the game has ended
            // the player still has their values restored
            print("SAVED GAME");
            PlayerPrefs.SetInt("Player Level", GameManager.level);
            PlayerPrefs.SetInt("Player Exp", GameManager.curEXP);
            PlayerPrefs.SetInt("Player Coins", GameManager.curCoins);
            PlayerPrefs.SetInt("Player Damage", GameManager.bulletDamage);
            PlayerPrefs.SetInt("Player Health", GameManager.curHealth);

            // This is played when the PlayerPrefs is saved
            click.Play();

            // Save the PlayerPrefs 
            PlayerPrefs.Save();
        }
    }
}