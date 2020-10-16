using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UFO
{
    // Adds text component to the game object the script is attached to
    [RequireComponent(typeof(Text))]
    public class CountdownTextScript : MonoBehaviour
    {
        // Creating an event the game manager will listen to using a delegate
        public delegate void CountdownFinish();
        public static event CountdownFinish OnCountdownFinish;

        // This is used to set a countdown text on the screen
        private Text countdownText;

        // Audio for the countdown
        public AudioSource countdownAudio;

        // When this game object is enabled then...
        private void OnEnable()
        {
            // Allows you to access the ".text" component
            countdownText = GetComponent<Text>();
            // Change the text
            countdownText.text = "5";
            // Start the audio
            countdownAudio.Play();
            // Start a coroutine
            StartCoroutine(CountdownText());
        }

        IEnumerator CountdownText()
        {
            // Variable used to decrease for a countdown
            int count = 5;

            // Go through the for loop 3 times and wait 1 second, then
            // decrease the count again and put it into countdownText.text
            for(int i = 0; i < count; i++)
            {
                countdownText.text = (count - i).ToString();
                yield return new WaitForSeconds(1f);
            }

            // Go to the game manager
            OnCountdownFinish();
        }
    }
}