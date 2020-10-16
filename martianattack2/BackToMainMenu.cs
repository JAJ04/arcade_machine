using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UFO
{
    public class BackToMainMenu : MonoBehaviour
    {
        // Reference to the Audio Source to play
        public AudioSource backSound;

        // Bool to only allow the sound to play once
        private bool soundPlayOnce = true;

        // Update is called once per frame
        void Update()
        {
            // Go back to the main menu of everything if "Left Alt" is pressed
            if(Input.GetKeyDown(KeyCode.LeftAlt))
            {
                StartCoroutine(BackToGodMenu());
            }
        }

        // Play a sound and go back to the god menu
        IEnumerator BackToGodMenu()
        {
            // Only play the sound once
            if(soundPlayOnce)
            {
                backSound.Play();
                soundPlayOnce = false;
            }

            yield return new WaitForSeconds(0.25f);

            SceneManager.LoadScene("godMenu");
        }
    }
}
