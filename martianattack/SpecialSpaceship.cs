using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class SpecialSpaceship : MonoBehaviour
    {
        // Used to get variables in GameManager
        private UIController _uIController;

        // Prefab for the destroyed animation
        [SerializeField]
        private GameObject _explosionPrefab;

        // Prefab for the destroyed animation
        [SerializeField]
        private GameObject _textPrefab;

        // Enemy Bullet prefab to keep shooting
        [SerializeField]
        private GameObject _enemyBullet;

        // Bool to only allow the ScoreUpdate method to do once
        private bool _scoreOnce = false;

        // Audio Clip to play the explosion sound
        [SerializeField]
        private AudioClip _alienExplosion;

        // Audio Clip to play the missile firing
        [SerializeField]
        private AudioClip _missileFiring;

        // Used to slow down the changing of the sprite color
        private float _timer = 0;

        // Used to keep shooting bullets
        private float _timerBullet = 0;

        // Bool to only spawn one explosion
        private bool _oneExplosion;

        // Use this for initialization
        void Start ()
        {
            // Set the _gameManager variable
            _uIController = GameObject.Find("Canvas").GetComponent<UIController>();
        }

        // Update is called once per frame
        void Update()
        {
            // Keep changing the sprite color after 2 seconds
            _timer += Time.deltaTime;

            // Timer for when to shoot bullets from the boss spaceship
            _timerBullet += Time.deltaTime;

            if (_timer > 1f)
            {
                // Keep changing colors over and over again
                GetComponent<SpriteRenderer>().color = Random.ColorHSV();
                _timer = 0f;
            }

            if (_timerBullet > 0.5f)
            {
                // Instantiate bullet below enemy
                Instantiate(_enemyBullet, transform.position - new Vector3(0, 2.0f, 0), transform.rotation);
                // Play missile fire sound
                AudioSource.PlayClipAtPoint(_missileFiring, Camera.main.transform.position, 1f);
                _timerBullet = 0f;
            }

            // Make the boss spaceship move when it is instantiated
            Vector3 position = transform.position;
            position.x += 0.05f;
            transform.position = position;

            // If the boss spaceship is off the screen destroy the spaceship to save memory
            if (transform.position.x >= 7.9f)
            {
                Destroy(this.gameObject);
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

                // Destroy the current object i.e. the enemy
                Destroy(this.gameObject);
            }

            if (collision.tag == "Bullet")
            {
                // Only allow the method to fire once (ScoreUpdateSpecial())
                _scoreOnce = true;

                // Increment boss counter
                UIController.bossSpaceshipCounter++;
                Debug.Log("Bullet Hit 1: " + UIController.bossSpaceshipCounter.ToString());

                // Update the score in the score script
                _uIController.ScoreUpdateSpecial();

                // Make a copy of the explosion prefab to indicate an enemy death
                Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

                // Play explosion sound
                AudioSource.PlayClipAtPoint(_alienExplosion, Camera.main.transform.position, 1f);

                // Trigger the bool so that no more explosion prefabs are spawned
                _oneExplosion = true;

                // Make a copy of the _textPrefab score to indicate an enemy score
                Instantiate(_textPrefab, transform.position + new Vector3(0.1f, 1.1f, 0), Quaternion.identity);

                // Destroy the left bullet
                Destroy(collision.gameObject);
                // Destroy the enemy
                Destroy(this.gameObject, 0.05f);
            }

            if (collision.tag == "Bullet2")
            {
                if (_scoreOnce == true)
                {
                    Debug.Log("Bullet Hit 2 Score Once True: " + UIController.bossSpaceshipCounter.ToString());

                    // Update the score in the score script to make it double
                    _uIController.ScoreUpdateSpecialDouble();

                    if(!_oneExplosion)
                    {
                        // Play explosion sound
                        AudioSource.PlayClipAtPoint(_alienExplosion, Camera.main.transform.position, 1f);

                        // Make a copy of the explosion prefab to indicate an enemy death
                        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
                    }

                    // Make a copy of the _textPrefab score to indicate an enemy score
                    Instantiate(_textPrefab, transform.position + new Vector3(0.1f, -1.1f, 0), Quaternion.identity);
                    // Destroy the right bullet
                    Destroy(collision.gameObject);
                    // Destroy the enemy
                    Destroy(this.gameObject, 0.05f);
                }

                // This allows the method to fire once only
                if (_scoreOnce == false)
                {
                    UIController.bossSpaceshipCounter++;
                    Debug.Log("Bullet Hit 2 Score Once False: " + UIController.bossSpaceshipCounter.ToString());

                    // Update the score in the score script
                    _uIController.ScoreUpdateSpecial();

                    if (!_oneExplosion)
                    {
                        // Make a copy of the explosion prefab to indicate an enemy death
                        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

                        // Play explosion sound
                        AudioSource.PlayClipAtPoint(_alienExplosion, Camera.main.transform.position, 1f);
                    }

                    // Make a copy of the _textPrefab score to indicate an enemy score
                    Instantiate(_textPrefab, transform.position + new Vector3(0.1f, -1.1f, 0), Quaternion.identity);

                    // Destroy the right bullet
                    Destroy(collision.gameObject);
                    // Destroy the enemy
                    Destroy(this.gameObject, 0.05f);
                }
            }
        }
    }
}