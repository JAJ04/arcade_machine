using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LeonsWorld
{
    public class AerosolCan : MonoBehaviour
    {
        // Variable to get game manager data
        public GameObject gameManager;
        private GameManager gameManagerScript;

        // Reference to the Audio Source to play the sound
        public AudioSource negativeBeeper;

        // Audio Source for the normal click sound
        public AudioSource clickSound;

        // Use this for initialization
        void Start ()
        {
            gameManagerScript = gameManager.GetComponent<GameManager>();
	    }

        public void AerosolCanFunction()
        {
            if (GameManager.curCoins >= 250 && PlayerPrefs.GetInt("Weapon1", 0) <= 0)
            {
                // This allows them to now use the AerosolCan by assigning the value "1"
                // to the key "Weapon1"
                // SUBTRACTING WEAPON AMOUNT
                GameManager.curCoins -= 250;
                PlayerPrefs.SetInt("Weapon1", 1);
                gameManagerScript.chaching.Play();
            }

            if (GameManager.curCoins < 250 && PlayerPrefs.GetInt("Weapon1", 0) <= 0)
            {
                // Play an invalid buzz sound
                negativeBeeper.Play();
            }

            // Allow the player to equip an aerosol can if PlayerPrefs Weapon 1 is >= 1
            if (PlayerPrefs.GetInt("Weapon1", 0) >= 1)
            {
                GameManager.bulletDamage = 1;
                gameManagerScript.weaponTitle.text = "Aerosol Can";

                // These are enables/disables to allow the player to use an AerosolCan
                gameManagerScript.flameThrower.SetActive(false);
                gameManagerScript.aerosol.SetActive(true);
                gameManagerScript.nerfGun.SetActive(false);
                gameManagerScript.aerosolCanBool = true;
                gameManagerScript.nerfGunBool = false;
                gameManagerScript.flameThrowerBool = false;
                clickSound.Play();
            }
        }
    }
}
