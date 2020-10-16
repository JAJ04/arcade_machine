using UnityEngine;
using System.Collections;

namespace Catch
{
    public class DestroyOnCollision : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag != "Bounds")
            {
                // Destroy the game object that collided with this
                // game object if they both hit
                Destroy(other.gameObject);
            }
        }
    }
}