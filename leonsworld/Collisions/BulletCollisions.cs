using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class BulletCollisions : MonoBehaviour 
    {
            // Sound properties to play when an
            // event happens
	    public AudioSource snowballDestroyed;
	    public AudioSource bulletDestroyed;

	    void OnTriggerEnter(Collider other)
	    {
                    // Destroy the bullet on
                    // any object that this script
                    // is attached to
		    if (other.tag == "PlayerBullet")
		    {
			    Destroy (other);
		    }
		
                    // Play the bullet destroyed sound
                    // and destroy the nerf bullet
		    if (other.tag == "PlayerBulletNerf")
		    {
			    Destroy (other);
			    bulletDestroyed.Play();
		    }
		
                    // Play the snowballDestroyed sound
                    // if the tag is "Bullet"
		    if (other.tag == "Bullet")
		    {
			    snowballDestroyed.Play();
		    }
	    }	
    }
}
