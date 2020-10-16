using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class BossGun : MonoBehaviour
    {
        // This is our enemy bullet prefab
        public GameObject bossBulletGO;

        // Bool to allow the FireEnemyBullet to fire snowballs
        bool fireSnowballs = true;

        // Timer to only allow the snowballs to be shot every 3 seconds
        float timer = 0f;

        // Bool to slow down the spawning of the snowball when the snowman encounters a wall
        float timerOver = 3.0f;

        // Checks conditions every frame
        void Update()
        {
            // Keep increasing the timer
            timer += Time.deltaTime;

            if(fireSnowballs && timer >= timerOver)
            {
                // Keep firing the boss bullet if the boss is not hitting a wall
                FireBossBullet();
                // Reset the timer
                timer = 0f;
            }
        }

        // Function to fire a boss bullet
        void FireBossBullet()
        {
            // Get reference to player
            GameObject player = GameObject.Find("leon");

            // If player does exist
            if (player != null)
            {
                GameObject bullet = Instantiate(bossBulletGO);

                // Set bullet start position
                bullet.transform.position = transform.position;

                // Compute bullet direction towards player
                Vector2 direction = player.transform.position - bullet.transform.position;

                // Set bullet direction
                bullet.GetComponent<BossBullet>().SetDirection(direction);
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
