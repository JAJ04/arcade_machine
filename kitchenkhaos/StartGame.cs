using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Catch
{
    public class StartGame : MonoBehaviour
    {
        // Reference to the GameManager and a Start Sound effect
        // As well as the theme song
        public GameManager gameManager;

        public AudioClip startSound;
        public AudioSource themeSong;

        // Audio for the sound to go back to the god menu
        public AudioSource backSound;

        // Bool that allows you to exit the game by pressing "Esc"
        private bool notStarted = true;

        // Bool to only allow the back sound to play once
        private bool soundPlayOnce = true;

        void Update()
        {
            // This is attached to the "Start" button and constantly
            // checks to see if "Space" is pressed and if it is,
            // start the game 
            if (Input.GetKeyDown(KeyCode.Space) && notStarted)
            {
                // Play start sound
                AudioSource.PlayClipAtPoint(startSound, Camera.main.transform.position, 1f);
                // Play theme sound
                themeSong.Play();
                // Start the game
                gameManager.StartGame();
                // Turn off the notStarted bool
                notStarted = false;
            }

            // The player can exit the game if notStarted is true and if "Left Alt" is pressed
            if(Input.GetKeyDown(KeyCode.LeftAlt) && notStarted)
            {
                // Go back to main menu
                if (Input.GetKeyDown(KeyCode.LeftAlt))
                {
                    StartCoroutine(GoToGodMenu());
                }
            }
        }

        IEnumerator GoToGodMenu()
        {
            // Play a sound (only once) and go back to the god menu after 0.25f seconds
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