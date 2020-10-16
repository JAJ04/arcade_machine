using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MartianAttack;

// Namespace has to be MartianAttack to access "CheatCodes"
namespace UFO
{
    public class GoToGame : MonoBehaviour
    {
        // Play the click sound
        public AudioSource clickSound;

        // Don't allow space to be pressed again
        private bool spacePressed = true;

        // Update is called once per frame
        void Update()
        {
            if (spacePressed)
            {
                // Load the game if "Space" is pressed and you have sufficient credits
                if (Input.GetKeyDown(KeyCode.Space) && CheatCodes.credits > 0)
                {
                    // Go to the main game scene
                    StartCoroutine(SwitchScene());
                    // Don't allow this code to be done again
                    spacePressed = false;
                }
            }
        }

        // Switch to the scene after 0.25 seconds
        IEnumerator SwitchScene()
        {
            // Play the sound
            clickSound.Play();
            yield return new WaitForSeconds(0.25f);
            SceneManager.LoadScene("ufo");
        }
    }
}
