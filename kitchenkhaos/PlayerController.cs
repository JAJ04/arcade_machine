using UnityEngine;
using System.Collections;

namespace Catch
{
    public class PlayerController : MonoBehaviour
    {
        // Reference to the clip that plays a scraping sound
        public AudioSource scrapeSound;

        // This variable is turned on when the game is over and is
        // turned off when the game is allowed to be played
        private bool _canPlay;

        // This is the max width that the player cannot exceed
        private float maxWidth;

        // Use this for initialization
        void Start()
        {
            // Cannot control the player when the game first loads
            _canPlay = false;

            // Get the upper corner of the screen
            Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
            // Get the screen view and translate it to the world view
            Vector3 targetWidth = Camera.main.ScreenToWorldPoint(upperCorner);
            // Gets the player width
            float playerWidth = GetComponent<Renderer>().bounds.extents.x;
            // Max width that the player can move between is the code below
            maxWidth = targetWidth.x - playerWidth;
        }

        // Update is called once per physics timestep
        void FixedUpdate()
        {
            // If canPlay is true
            if (_canPlay)
            {
                // Move the position of this to the right by 8f with Time.deltaTime
                if (Input.GetKey(KeyCode.D))
                {
                    // Play the scrape/move sound if there isn't one already playing
                    if (scrapeSound.isPlaying == false)
                    {
                        scrapeSound.Play();
                    }

                    transform.position += transform.right * 12f * Time.deltaTime;
                }

                // Move the position of this to the left by -8f with Time.deltaTime
                if (Input.GetKey(KeyCode.A))
                {
                    // Play the scrape/move sound if there isn't one already playing
                    if(scrapeSound.isPlaying == false)
                    {
                        scrapeSound.Play();
                    }

                    transform.position += transform.right * -12f * Time.deltaTime;
                }

                // We are now clamping the max width of the player to move around in
                // using the translations above
                Vector3 targetPosition = new Vector3(transform.position.x, -0.69f, 0.0f);
                float targetWidth = Mathf.Clamp(targetPosition.x, -maxWidth, maxWidth);
                targetPosition = new Vector3(targetWidth, targetPosition.y, targetPosition.z);
                GetComponent<Rigidbody2D>().MovePosition(targetPosition);
            }
        }

        // A parameter is passed into PlayerMove to allow the player to move
        public void PlayerMove(bool allow)
        {
            _canPlay = allow;
        }
    }
}