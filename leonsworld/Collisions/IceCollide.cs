using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class IceCollide : MonoBehaviour {
	
        // Gets the Player properties
	    public PlayerControl playerControl;
	
        // This is used to play the slip sound
	    public AudioSource slip;

        // This script is attached to the
        // slippy ice game objects
	    void OnTriggerEnter(Collider other)
	    {
            // If the tag is the "Player" then
            // increase the speed of the player slightly
		    if(other.tag == "Player")
		    {
			    slip.Play();
			    playerControl.walkSpeed = 20f;
		    }
	    }
	
	    void OnTriggerExit(Collider other)
	    {
		    if(other.tag == "Player")
		    {
                // Return the player walk speed to normal
			    playerControl.walkSpeed = 5f;
		    }
	    }
    }
}