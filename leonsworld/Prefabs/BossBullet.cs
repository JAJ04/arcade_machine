using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class BossBullet : MonoBehaviour
    {
        // Bullet speed
        private float speed;

        // Direction of bullet
        private Vector2 _direction;

        // Check to see if bullet can be shot
        private bool isReady;

        void Awake()
        {
            // This is used to prevent overlapping between bullets
            GetComponent<SpriteRenderer>().sortingOrder = Random.Range(9, 32767);
            // Speed at which the bullet travels
            speed = 5f;
            isReady = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Wall")
            {
                Destroy(gameObject);
            }
        }

        // Where will the bullet shoot?
        public void SetDirection(Vector2 direction)
        {
            // Direction is normalized meaning give it a length (mag) of 1
            _direction = direction.normalized;

            isReady = true; // set to true
        }

        // Update is called once per frame
        void Update()
        {
            if (isReady)
            {
                // Get the bullet current position
                Vector2 position = transform.position;

                // Move the bullet to a new position
                position += _direction * speed * Time.deltaTime;

                // Update bullet position
                transform.position = position;

                // Removing the bullet

                // Bottom left of screen
                Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
                // Top right of screen
                Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

                // Destroy bullet if outside of these bounds
                if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.x) || (transform.position.y > max.y))
                {
                    StartCoroutine("WaitAndDestroy");
                }
            }
        }

        IEnumerator WaitAndDestroy()
        {
            // Destroy the bullet if it has gone out of bonds in 10 seconds
            yield return new WaitForSeconds(10);
            Destroy(gameObject);
        }
    }
}
