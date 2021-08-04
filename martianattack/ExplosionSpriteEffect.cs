using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack 
{
    public class ExplosionSpriteEffect: MonoBehaviour 
    {
        // Reference to the audio source
        private AudioSource _audioSource;

        // Use this for initialization
        void Start() 
	{
            // Get audio source on this object
            _audioSource = GetComponent <AudioSource>();

            _audioSource.Play();

            if (gameObject.activeSelf == true) 
	    {
                // To save memory, make sure to delete the sprite after 3 seconds
                Destroy(gameObject, 3 f);
            }
        }
    }
}
