using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LeonsWorld
{
    public class NerfGun : MonoBehaviour
    {
        // Variable to get the game manager
        public GameObject gameManager;

        // Variable to get the game manager properties
        private GameManager gameManagerScript;

        // Reference to the sound to play
        public AudioSource selectWepSound;

	    // Use this for initialization
	    void Start ()
        {
            gameManagerScript = gameManager.GetComponent<GameManager>();
        }

        public void GiveNerfGun()
        {
            // Does enabling/renabling/playing to give the player the nerf gun
            GameManager.bulletDamage = 5;

            gameManagerScript.weaponTitle.text = "Nerf Gun";

            // Play the weapon sound
            selectWepSound.Play();

            gameManagerScript.flameThrower.SetActive(false);
            gameManagerScript.aerosol.SetActive(false);
            gameManagerScript.nerfGun.SetActive(true);
            gameManagerScript.aerosolCanBool = false;
            gameManagerScript.nerfGunBool = true;
            gameManagerScript.flameThrowerBool = false;
        }
    }
}