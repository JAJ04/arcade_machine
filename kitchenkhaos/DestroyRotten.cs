using UnityEngine;
using System.Collections;

namespace Catch
{
    public class DestroyRotten : MonoBehaviour
    {
        void OnCollisionEnter2D(Collision2D collision)
        {
            // If the collision is a hat then
            if (collision.gameObject.tag == "Protag")
            {
                // Destroy this game object
                Destroy(gameObject);
            }
        }
    }
}