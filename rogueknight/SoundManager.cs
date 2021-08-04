using UnityEngine;
using System.Collections;

namespace RogueKnight 
{
    public class SoundManager : MonoBehaviour 
    {
        // Variables for pitch ranges and references to elements
        public float lowPitch = .85 f;
        public float highPitch = 1.25 f;

        // musicSource and soundSource are different
        // efxSource is used when RandomizeSfx is called
        public AudioSource musicSource;
        public AudioSource soundSource;

        // Makes use of the singleton design pattern
        public static SoundManager instance = null;

        void Awake() 
        {
            // Checks to see if there is an instance of "THIS"
            if (instance == null) 
            {
                // Create an instance
                instance = this;
            }

            // If there is an instance of "THIS" class that already exists then
            else if (instance != this) 
            {
                // Destroy this game object
                Destroy(gameObject);
            }

            // Used so that this instance will never be destroyed during scene transitions
            DontDestroyOnLoad(gameObject);
        }

        // RandomizeSfx chooses between two audio clips randomly
        public void RandomSoundEffect(params AudioClip[] audioClips) 
        {
            // Generate a random number between 0 and the length of the audioClips array
            int randomIndex = Random.Range(0, audioClips.Length);

            // Choose a random pitch to play
            float randomPitch = Random.Range(lowPitch, highPitch);

            // Set the pitch of the audio source 
            soundSource.pitch = randomPitch;

            // Play audio clip at random index
            soundSource.clip = audioClips[randomIndex];

            // Play 
            soundSource.Play();
        }
    }
}
