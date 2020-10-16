using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueKnight
{
    public class GoToWelcome : MonoBehaviour
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
                // If "Space" is pressed go to the dodge-em scene
                if (Input.GetKey(KeyCode.Space))
                {
                    // Play the sound
                    clickSound.Play();
                    // Go to the main game
                    StartCoroutine(SwitchScene());
                    // Don't allow this code to be done again
                    spacePressed = false;
                }
            }
        }

        // Switch to the scene after 0.5f seconds
        IEnumerator SwitchScene()
        {
            yield return new WaitForSeconds(0.5f);
            SceneManager.LoadScene("welcome");
        }
    }
}