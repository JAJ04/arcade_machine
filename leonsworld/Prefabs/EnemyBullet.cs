using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class EnemyBullet : MonoBehaviour 
    {
        // Bullet speed
        private float speed;

        // Direction of bullet
        private Vector2 _direction; 

        // Check to see if bullet can be shot
        private bool isReady; 
	
        // This is used to play a sound when the bullet is destroyed
	    public AudioSource bulletDestroyed;

	    void Awake()
	    {
            // This is used to prevent overlapping between bullets
            GetComponent<SpriteRenderer>().sortingOrder = Random.Range(9, 32766);
            // Speed at which the bullet travels
            speed = 5f;
		    isReady = false;
	    }

	    void OnTriggerEnter(Collider other)
	    {
            // Destroy this game object if it hits any of these tags and possibly play a sound
		    if(other.gameObject.tag == "Wall" && gameObject != null)
		    {
			    Destroy(gameObject);
		    }
		
		    if(other.gameObject.tag == "PlayerBullet" && gameObject != null)
		    {
			    bulletDestroyed.Play();
			    Destroy(gameObject, 0.1f);
		    }
		
		    if(other.gameObject.tag == "PlayerBulletNerf" && gameObject != null)
		    {
			    bulletDestroyed.Play();
			    Destroy(gameObject, 0.1f);
		    }
		
		    if(other.gameObject.tag == "CanBullet" && gameObject != null)
		    {
			    Destroy(gameObject);
		    }
		
		    if(other.gameObject.tag == "ThrowerBullet" && gameObject != null)
		    {
			    Destroy(gameObject);
		    }
	    }

	    // where will the bullet shoot?
	    public void SetDirection(Vector2 direction)
	    {
		    // direction is normalized meaning give it a length (mag) of 1
		    _direction = direction.normalized;

            // Set to true so the bullet can be hit
            isReady = true; 
	    }
	
	    // Update is called once per frame
	    void Update ()
	    {
		    if (isReady)
            {
                // Rotate the bullet -45 degrees each update time
			    transform.Rotate (Vector3.forward, -45 * Time.deltaTime);
			
			    // Get the bullet current position
			    Vector2 position = transform.position;

			    // Move the bullet to a new position
			    position += _direction * speed * Time.deltaTime;

			    // Update bullet position
			    transform.position = position;

			    // Removing the bullet

			    // Bottom left of screen
			    Vector2 min = Camera.main.ViewportToWorldPoint (new Vector2 (0, 0));
			    // Top right of screen
			    Vector2 max = Camera.main.ViewportToWorldPoint (new Vector2 (1, 1));

			    // Destroy bullet if outside of these bounds
			    if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.x) || (transform.position.y > max.y))
			    {
				    StartCoroutine ("WaitAndDestroy");
			    }
		    }
	    }

	    IEnumerator WaitAndDestroy()
	    {
            // Wait 10 seconds before destroying the bullet
		    yield return new WaitForSeconds(10);
		    Destroy (gameObject);
	    }
    }
}