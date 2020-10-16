using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeonsWorld
{
    public class GoBackToGodMenuBoss : MonoBehaviour
    {
        // The code to go back to the "god menu" needs to be on its own as there is so much code going on
        // I needed to single it out because I was getting lost

        // Bool to only play the back sound once
        private bool soundPlayOnce = true;
        // Bool to have Time.timeScale = 1 at all times
        private bool forceUnpause = false;

        // Reference for the sound to play
        public AudioSource backSound;
        // Reference to the death sound to disable it
        public AudioSource deathSound;

        // Update is called once per frame
        void Update()
        {
            // If the game manager script on the object no longer exists then the player can go back 
            // to the god menu if he or she wishes
            if (GameObject.Find("GameManager"))
            {
                if (GameObject.Find("GameManager").GetComponent<GameManager>() == null)
                {
                    if(Input.GetKeyDown(KeyCode.LeftAlt))
                    {
                        // Unpause the game so the user can go backwards and force it to be unpaused
                        forceUnpause = true;                        
                        StartCoroutine(GoBackToGodMenu());
                    }
                }
            }

            // Always have the game be Time.timeScale = 1
            if(forceUnpause)
            {
                Time.timeScale = 1; 
            }
        }

        IEnumerator GoBackToGodMenu()
        {
            // Play a sound (only once) and switch scenes after 0.25f seconds
            if (soundPlayOnce)
            {
                backSound.Play();
                soundPlayOnce = false;
            }

            deathSound.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.25f);

            SceneManager.LoadScene("godMenu");
        }
    }
}