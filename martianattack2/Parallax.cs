using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFO
{
    public class Parallax : MonoBehaviour
    {
        // Object Pooling:
        // Instantiate and OnDestroy will only be called once in the configuration process

        // Keeps track of the pipes parallaxing
        // Determines when the object will be in use
        class PoolObject
        {
            // Determines if the object is in use
            public bool isUsed;
            // Variable to move the object
            public Transform transform;

            // Sets t in the parameter to the "transform" variable
            public PoolObject(Transform t)
            {
                transform = t;
            }

            // Functions that flip isUsed
            public void UsePipe()
            {
                isUsed = true;
            }

            public void LosePipe()
            {
                isUsed = false;
            }
        }

        // Make it show in the Unity inspector
        [System.Serializable]
        public struct YSpawnRange
        {
            // The minimum and max height of the spawns
            public float minHeight;
            public float maxHeight;
        }

        // Reference to the struct above
        public YSpawnRange ySpawnRange;
        // How many pipes we are going to spawn
        public int sizeOfPool;
        // What GameObject we are going to spawn
        public GameObject objectPrefab;
        // The rate at which new objects spawn
        public float spawnRate;
        // How fast the parallax objects will move
        public float parallaxSpeed;

        // Sets the default position
        public Vector3 originalSpawnPos;

        // Used to spawn the pipes off the screen no matter the res
        // Default Spawn Pos need to be relative to the
        // target aspect ratio
        public Vector2 aspectRatioTarget;

        // The aspect ratio you're working from
        float aspectTarget;

        // Timer for the spawns
        float spawnTimer;

        // All of the objects that go into the pool
        PoolObject[] poolObjects;

        // Reference to the GameManager
        GameManager gameManager;

        // The first initialisation
        private void Awake()
        {
            ConfigureParallax();
        }

        // Initialise the game
        private void Start()
        {
            // Get the game manager properties using the instance variable
            gameManager = GameManager.Instance;
        }

        // Subscribing to events
        private void OnEnable()
        {
            GameManager.OnGameOver += OnGameOver;
        }

        // Unsubscring to events
        private void OnDisable()
        {
            GameManager.OnGameOver -= OnGameOver;
        }

        // Used for shifting the parallax objects
        private void Update()
        {
            // If the game is over don't bother updating
            if(gameManager.GameOver)
            {
                return;
            }

            // Move the parallax objects if the game isn't over
            MoveParallax();

            // Increase spawn timer gradually
            spawnTimer += Time.deltaTime;

            // When the spawnTimer is greater than the spawn rate, spawn an object
            // and reset spawnTimer so another object can be spawned
            if(spawnTimer > spawnRate)
            {
                SpawnParallax();
                spawnTimer = 0;
            }
        }

        // Returns the transform information of a pool object
        Transform GetPoolElement()
        {
            // Get first object in the pool array

            // Go through all of the pool objects
            for (int i = 0; i < poolObjects.Length; i++)
            {
                // If the pool object is not in use then
                if(!poolObjects[i].isUsed)
                {
                    // Sets the pool object in question to true
                    poolObjects[i].UsePipe();
                    return poolObjects[i].transform;
                }
            }

            return null;
        }

        void ConfigureParallax()
        {
            // Set what the target aspect normally is

            // Target aspect should be reset everytime you reconfigure due to the inspector
            // variable
            aspectTarget = aspectRatioTarget.x / aspectRatioTarget.y;

            // Create the pool objects array
            poolObjects = new PoolObject[sizeOfPool];

            // This is used to instantiate multiple objects/pipes at the size 
            // of the poolObjects array
            for(int i = 0; i < poolObjects.Length; i++)
            {
                // Instantiate a game object
                GameObject gameObject = Instantiate(objectPrefab) as GameObject;
                // Get the position properties of the game object defined above
                Transform trans = gameObject.transform;
                // Set the parent of "trans" to this current game object
                // Parallax script is attached to relevant game objects
                trans.SetParent(transform);
                // Initialise the position
                trans.position = Vector3.one * 1000;

                // A pool object element is a new PoolObject with the property "trans"
                poolObjects[i] = new PoolObject(trans);
            }
        }

        // If prewarm isn't chosen then this will not be called
        void SpawnParallax()
        {
            // Get the first available pool object
            Transform trans = GetPoolElement();

            // If there is no pool object, return i.e. exit out of this function or
            // pool size is too small
            if(trans == null)
            {
                return;
            }

            // Set the pos object that the parallax object will be placed at
            Vector3 position = Vector3.zero;

            // Pos of the parallax object will be paused at the originalSpawnPos.x
            // Take the aspect ratio into account also for pipe pop-ins/pop-outs
            position.x = (originalSpawnPos.x * Camera.main.aspect) / aspectTarget;
            // Pos on the Y axis will be a random number between min and max
            position.y = Random.Range(ySpawnRange.minHeight, ySpawnRange.maxHeight);

            // Set the actual pos object where it spawns (pipe)
            trans.position = position;
        }

        // This will move the objects
        void MoveParallax()
        {
            // Loop through pool objects
            for(int i = 0; i < poolObjects.Length; i++)
            {
                // Move the positions of the objects with the speed defined above

                // Time.deltaTime is used for frame independencies
                poolObjects[i].transform.localPosition += -Vector3.right * parallaxSpeed * Time.deltaTime;

                // Is the position less than the default spawn pos?
                CheckDisposableObject(poolObjects[i]);
            }
        }

        // This will check if an object needs to be disposed
        void CheckDisposableObject(PoolObject poolObject)
        {
            // Get rid of the parallax object on the left and take the aspect 
            // ratio into account to prevent pop-ins/pop-outs
            if(poolObject.transform.position.x < (-originalSpawnPos.x * Camera.main.aspect) / aspectTarget)
            {
                // Sets isUsed to false
                poolObject.LosePipe();
                // Sets the position/hides it from the player
                poolObject.transform.position = Vector3.one * 1000;
            }
        }

        // Dispose the pool objects and reconfigure
        void OnGameOver()
        {
            // Dispose of all the pipes in the pool and reset the positions off-screen
            for(int i = 0; i < poolObjects.Length; i++)
            {
                poolObjects[i].LosePipe();
                poolObjects[i].transform.position = Vector3.one * 1000;
            }
        }
    }
}