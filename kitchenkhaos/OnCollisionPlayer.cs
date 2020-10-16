using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionPlayer : MonoBehaviour
{
    // Reference to the ding sound
    public AudioClip garbageSound;
    public AudioClip garbageSound2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Fruit")
        {
            // Play garbage sound when normal object hits
            AudioSource.PlayClipAtPoint(garbageSound, Camera.main.transform.position, 1f);
        }

        if (collision.tag == "Rotten")
        {
            // Play second garbage sound when normal object hits
            AudioSource.PlayClipAtPoint(garbageSound2, Camera.main.transform.position, 1f);
        }
    }
}