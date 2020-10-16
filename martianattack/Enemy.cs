using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class Enemy : MonoBehaviour
    {
        // Prefab for the destroyed animation
        [SerializeField]
        private GameObject _explosionPrefab;

        // Audio Clip to play the explosion sound
        [SerializeField]
        private AudioClip _alienExplosion;

        // Audio Clip to play the player hit
        [SerializeField]
        private AudioClip _playerHit;

        // Script communication variable to go into the UIController
        private UIController _uIController;

        // Script communication for player
        [SerializeField]
        private GameObject _player;

        // Variable to get and change the enemy speed in this class
        private SpawnController _spawnController;

        // Bool to prevent enemy explosion from spawning twice
        private bool _oneEnemyExplosion;

        void Start()
        {
            // Sets the _uIController to the script on the "Canvas" object
            _uIController = GameObject.Find("Canvas").GetComponent<UIController>();

            // Get the _spawnController script to change variables
            _spawnController = GameObject.Find("Spawn_Controller").GetComponent<SpawnController>();
        }

        // Update is called once per frame
        void Update ()
        {
            // Moves the enemy downwards according to the speed
            // by using Time.deltaTime also
            transform.Translate(Vector3.down * _spawnController._enemyMovement * Time.deltaTime);

            // When the y position of the enemy is less than -8 e.g. -9 then
            // respawn the enemy at a random X position
            if(transform.position.y < -8)
            {
                // Spawn enemy between these values
                float randomNumCoord = Random.Range(-4.956415f, 4.970408f);
                // randomNum is placed into the X position of the vector to re-assign 
                // the X position
                transform.position = new Vector3(randomNumCoord, 7, 0);
            }
        }

        // Unity function that is implemented already in the Unity API
        // that gets a collision on this object
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
            {
                // Script communication
                // Get the player object from the collision and the script
                Player player = collision.GetComponent<Player>();

                // If the player exists
                if (player != null)
                {
                    // Subtract lives method
                    player.HitEncountered();
                }

                // Instantiate (make a copy of) the explosion prefab at this game object's position
                // using transform.position
                // Quaternion.identity means no rotation
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

                // Play explosion sound
                AudioSource.PlayClipAtPoint(_alienExplosion, Camera.main.transform.position, 1f);

                // Destroy the current object i.e. the enemy
                Destroy(this.gameObject);
            }
            else if (collision.tag == "Bullet")
            {
                // Update the score in the score script
                _uIController.ScoreUpdate();

                // Make a copy of the explosion prefab to indicate an enemy death
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

                // Play explosion sound
                AudioSource.PlayClipAtPoint(_alienExplosion, Camera.main.transform.position, 1f);

                // Set _oneEnemyExplosion to true
                _oneEnemyExplosion = true;

                // Destroy the left bullet
                Destroy(collision.gameObject);
                // Destroy the enemy
                Destroy(this.gameObject, 0.05f);
            }
            else if (collision.tag == "Bullet2")
            {
                // Update the score in the score script
                _uIController.ScoreUpdate();

                if(!_oneEnemyExplosion)
                {
                    // Make a copy of the explosion prefab to indicate an enemy death
                    Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

                    // Play explosion sound
                    AudioSource.PlayClipAtPoint(_alienExplosion, Camera.main.transform.position, 1f);
                }

                // Destroy the right bullet
                Destroy(collision.gameObject);
                // Destroy the enemy
                Destroy(this.gameObject, 0.05f);
            }
        }
    }
}