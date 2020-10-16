using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelowSceneCollision : MonoBehaviour
{
    // Reference to the missed sound
    public AudioClip missedSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Play missed sound
        AudioSource.PlayClipAtPoint(missedSound, Camera.main.transform.position, 1f);
    }
}
