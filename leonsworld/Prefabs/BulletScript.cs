using UnityEngine;
using System.Collections;

namespace LeonsWorld 
{
    public class BulletScript : MonoBehaviour 
    {
        // If the bullet hits any of the tags do something and destroy the
        // game object attached to the tag no matter what
        void OnTriggerEnter(Collider other)
	{
            if (other.gameObject.tag == "Enemy") 
	    {
                // Fire these methods if this condition above is true and do it for the below tags too
                Destroy(gameObject);
                other.gameObject.SendMessage("EnemyDamaged", GameManager.bulletDamage, SendMessageOptions.DontRequireReceiver);
                other.gameObject.SendMessage("TakenDamage", SendMessageOptions.DontRequireReceiver);
            }

            // Same as the below "Wall" tag but you will send a message to the "BossDamaged" function
            if (other.gameObject.tag == "SnowballBoss")
	    {
                Destroy(gameObject);
                other.gameObject.SendMessage("BossDamaged", GameManager.bulletDamage, SendMessageOptions.DontRequireReceiver);
            }

            // If a bullet hits a wall then destroy "this" bullet/game object
            if (other.gameObject.tag == "Wall") 
	    {
                Destroy(gameObject);
            }
        }

        // Destroy the bullet after 0.6f seconds automatically
        void FixedUpdate() 
	{
            Destroy(gameObject, 0.6 f);
        }
    }
}
