using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeonsWorld
{
    public class Flamethrower : MonoBehaviour
    {
        // Variables to get game manager data
        public GameObject gameManager;
        private GameManager gameManagerScript;

        // Reference to the Audio Sources to play a sound
        public AudioSource negativeBeeper;

        // Audio Source for the normal click sound
        public AudioSource clickSound;

        // Use this for initialization
        void Start () {
            gameManagerScript = gameManager.GetComponent<GameManager>();
	    }

        public void FlamethrowerFunction()
        {
            // WEAPON 2 BUTTON
            if (PlayerPrefs.GetInt("Weapon2", 0) >= 1)
            {
                // This allows the player to equip the flamethrower
                GameManager.bulletDamage = 6;
                gameManagerScript.weaponTitle.text = "Flamethrower";

                // Enabling/disabling to allow the player to have the flamethrower
                gameManagerScript.flameThrower.SetActive(true);
                gameManagerScript.aerosol.SetActive(false);
                gameManagerScript.nerfGun.SetActive(false);
                gameManagerScript.aerosolCanBool = false;
                gameManagerScript.nerfGunBool = false;
                gameManagerScript.flameThrowerBool = true;
                clickSound.Play();
            }
            else
            {
                if (GameManager.curCoins >= 500)
                {
                    // Set the "Weapon" PlayerPrefs so that the player can have the Flamethrower enabled

                    // SUBTRACTING WEAPON AMOUNT
                    GameManager.curCoins -= 500;
                    PlayerPrefs.SetInt("Weapon2", 1);
                    gameManagerScript.chaching.Play();
                }
                else
                {
                    // Play the invalid sound
                    negativeBeeper.Play();
                }
            }
        }
    }
}