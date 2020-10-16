using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueKnight
{
    public class GoToGodMenu : MonoBehaviour
    {
        // Reference to the back sound
        public AudioSource backSound;

        // Bool to only allow the backSound to play once
        private bool playSoundOnce = true;

        // Update is called once per frame
        void Update()
        {
            // When "LeftAlt" is pressed go back to the god menu
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                StartCoroutine(GoBackToGodMenu());
            }
        }

        IEnumerator GoBackToGodMenu()
        {
            // Play a sound (only once) and go to the god menu after 0.25f seconds
            if(playSoundOnce)
            {
                backSound.Play();
                playSoundOnce = false;
            }

            yield return new WaitForSeconds(0.25f);

            SceneManager.LoadScene("godMenu");
        }
    }
}