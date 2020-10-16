using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DodgeEm
{
    public class EnemySpawner : MonoBehaviour
    {
        // The enemy cars you want to instantiate (create a copy of)
        public GameObject[] enemyCars;

        // Clamp variables to prevent them from spawning outside of the road
        private float maxClamp = 1.67f;
        private float minClamp = -2.17f;

        // Variable used to store a random position
        public Vector3 randomPos;

        // Timers that spawns a car after a certain amount of time (1 second)
        private float enemyTimer = 0.5f;

        // Variable to select a car from the enemyCars array randomly
        private int selectedCar;

        // Update is called once per frame
        void Update()
        {
            // Decrease the timer every update
            enemyTimer -= Time.deltaTime;

            if(enemyTimer <= 0)
            {
                // Initializes the random position variable on the X axis
                randomPos = new Vector3(Random.Range(minClamp, maxClamp), transform.position.y,
                    transform.position.z);

                // Create selectedCar randomly
                selectedCar = Random.Range(0, enemyCars.Length);

                // Instantiate the enemy car at the random position 
                // and rotation of the empty game object
                Instantiate(enemyCars[selectedCar], randomPos, transform.rotation);

                // Reset so that the car can spawn again after 1 second
                enemyTimer = 0.5f;
            }
        }
    }
}