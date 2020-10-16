using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DodgeEm
{
    public class DestroyEnemy : MonoBehaviour
    {
        // Check for collisions with the enemy cars
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.tag == "EnemyCar")
            {
                // Destroy the collided game object
                Destroy(collision.gameObject);
            }
        }
    }
}