using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class EnemyGun : MonoBehaviour
    {
        // This is our enemy bullet prefab
        public GameObject enemyBulletGO;

        // Bool to allow the FireEnemyBullet to fire snowballs
        private bool fireSnowballs = true;

        // Timer to only allow the snowballs to be shot every 3 seconds
        private float timer = 0f;

        // Bool to slow down the spawning of the snowball when the snowman encounters a wall
        private float timerOver = 3.0f;

        // Checks conditions every frame
        void Update()
        {
            // Keep increasing the timer
            timer += Time.deltaTime;

            if(fireSnowballs && timer >= timerOver)
            {
                // Keep firing the enemy bullet if the boss is not hitting a wall
                FireEnemyBullet();
                // Reset the timer
                timer = 0f;
            }
        }

        // Function to fire an enemy bullet
        void FireEnemyBullet()
        {
            // Get reference to player
            GameObject player = GameObject.Find("Leon");

            // If player does exist
            if (player != null)
            {
                GameObject bullet = Instantiate(enemyBulletGO);

                // Set bullet start position
                bullet.transform.position = transform.position;

                // Compute bullet direction towards player
                Vector2 direction = player.transform.position - bullet.transform.position;

                // Set bullet direction
                bullet.GetComponent<EnemyBullet>().SetDirection(direction);
            }
        }

        void OnTriggerEnter(Collider col)
        {
            if(col.gameObject.tag == "Wall")
            {
                // Don't fire the snowball
                fireSnowballs = false;
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (col.gameObject.tag == "Wall")
            {
                // Fire the snowball
                fireSnowballs = true;
            }
        }
    }

}
