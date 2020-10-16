using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class Pickup : MonoBehaviour
    {
        // This is the speed at which the pickup will go down the screen
        [SerializeField]
        private float _pickUpSpeed = 3.0f;
        [SerializeField]

        // Variable for the pickupID to differentiate between the pickups
        private int pickupID;

        // Variable to test the time
        private float _timeLeft;

        // AudioSource to play pickup sound
        [SerializeField]
        private AudioClip _pickupSound;

        // 0 = Double Shot
        // 1 = Speed Boost
        // 2 = Shield
        // 3 = Health

        // Update is called once per frame
        void Update ()
        {
            // Moves the pickup downwards according to the speed
            // by using Time.deltaTime also
            transform.Translate(Vector3.down * _pickUpSpeed * Time.deltaTime);

            // When it goes off the screen, save some memory and delete it
            if(transform.position.y < -8)
            {
                Destroy(this.gameObject);
            }
        }
    
        // OnTriggerEnter2D is used when a collision is detected
        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Player")
            {
                // Access the script that is on the Player sprite (Player)
                Player player = other.GetComponent<Player>();

                // Play sound
                AudioSource.PlayClipAtPoint(_pickupSound, Camera.main.transform.position, 1f);

                if (player != null)
                {
                    // If statement below gives different pickups dependent on the pickupID
                    if (pickupID == 0)
                    {
                        player.AddDoubleShot(5.0f);
                    }
                    else if (pickupID == 1)
                    {
                        // Keeps adding 5.0f to the lifetime of the power up
                        // to keep it alive even when more are collected
                        player.AddSpeedUp(5.0f);
                    }
                    else if (pickupID == 2)
                    {
                        player.AddShield(5.0f);
                    }
                    else if (pickupID == 3)
                    {
                        player.AddHealth();
                    }
                    else if (pickupID == 4)
                    {
                        player.AddSpeedyBullet(10.0f);
                    }
                }

                // Destroy the pickup (ourself) when the player has collided with the pickup
                Destroy(this.gameObject);
            }
        }
    }
}