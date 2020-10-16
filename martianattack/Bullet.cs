using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class Bullet : MonoBehaviour
    {
        // Sets the speed of the bullet
        [SerializeField]
        private float _speed = 10.0f;
	
	    // Update is called once per frame
	    void Update ()
        {
            // Move up at 10 speed using Time.deltaTime
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            // If the bullet goes off the screen then destroy the bullet
            // to save memory
            if (transform.position.y > 5.51f)
            {
                // Destroy the left-over parent game objects to prevent
                // bloated memory
                if(transform.parent != null)
                {
                    Destroy(transform.parent.gameObject);
                }

                // Destroy "this" game object that the script is attached to 
                // i.e. the bullet
                Destroy(this.gameObject);
            }
        }
    }
}