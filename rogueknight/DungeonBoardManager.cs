using UnityEngine;
using System;
using System.Collections.Generic;

using Random = UnityEngine.Random; 		

namespace RogueKnight
{
	public class DungeonBoardManager : MonoBehaviour
	{
		// Serializable allows properties to be set in the inspector
		[Serializable]

        // Going to hold some values in the "BoardCount" class which will be instantiated on different copies
        public class BoardCount
		{
            // Min and max values for the "BoardCount" class
            public int min; 			
			public int max; 			
			
			
			// Constructor
			public BoardCount(int min, int max)
			{
                this.min = min;
				this.max = max;
			}
		}
		
		// Number of cols and rows
		public int cols = 8; 										
		public int rows = 8;

        // Game object and game object arrays to hold various items
        public GameObject[] floorTiles;
        public GameObject nextDungeon;
        public GameObject[] energyTiles;
        public GameObject[] barrierTiles;
        public GameObject[] ghostTiles;
        public GameObject[] wallTiles;

        // Stores transform of the board game object
        private Transform dungeonBoardHolder;
        // Locations to place tiles
        private List<Vector3> dungeonTilesPositions = new List<Vector3>();

        // Max and min energy items per dungeon
        public BoardCount energyCount = new BoardCount(1, 5);
        // Max and min limit for no. of walls per level
        public BoardCount wallCount = new BoardCount(5, 9);

        // SetupDungeon sets up each dungeon
        public void SetupDungeon(int level)
        {
            // Creates barrier and floor
            DungeonBoardSetup();

            // Resets the list of dungeonTilesPositions
            InitialiseDungeonTilesList();

            // Makes a copy of a random num. of dungeon wall tiles based on min and max at random positions
            LayoutObjectAtRandom(wallTiles, wallCount.min, wallCount.max);

            //Instantiate a random number of food tiles based on minimum and maximum, at randomized positions.
            LayoutObjectAtRandom(energyTiles, energyCount.min, energyCount.max);

            // This will spawn a certain amount of enemies logarithimically
            int ghostCount = (int)Mathf.Log(level, 2f);

            // Make a copy of a random amount of enemies based on min and max at random positions
            LayoutObjectAtRandom(ghostTiles, ghostCount, ghostCount);

            // Instantiate the next dungeon tile in the upper right corner with no rotation
            Instantiate(nextDungeon, new Vector3(cols - 1, rows - 8, 0f), Quaternion.Euler(0, 0, 180));
        }

        // Sets up the barrier and floor
        void DungeonBoardSetup ()
		{
			// Create a game object called "DungeonBoard" and set boardHolder to the game object's transform
			dungeonBoardHolder = new GameObject ("DungeonBoard").transform;
			
			// Go along the x axis starting from -1 which fills the corners with either floor or barrier tiles
			for(int x = -1; x < cols + 1; x++)
			{
				// Loop along the y axis, starting from -1 to fill board with either floor or barrier tiles
				for(int y = -1; y < rows + 1; y++)
				{
					// Select a random tile from the array of floor prefabs and make a copy
					GameObject randomTileInstantiation = floorTiles[Random.Range (0,floorTiles.Length)];
					
					// Check if the current pos is at a corner, if it is choose a random barrier prefab from the array of barrier tiles
					if(x == -1 || x == cols || y == -1 || y == rows)
                    {
                        randomTileInstantiation = barrierTiles[Random.Range(0, barrierTiles.Length)];
                    }

                    // Make a GameObject (copy) using the prefab chosen for randomTileInstantiation at the Vector3 position in the loop
                    GameObject instance = Instantiate (randomTileInstantiation, 
                        new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

                    // Set the parent of the instantiated object instance to dungeonBoardHolder
                    instance.transform.SetParent (dungeonBoardHolder);
				}
			}
		}

        // Clear the dungeonTilePositions list and then make a new board
        void InitialiseDungeonTilesList()
        {
            // Clear the list
            dungeonTilesPositions.Clear();

            // Go through the x axis
            for (int x = 1; x < cols - 1; x++)
            {
                // Within each x axis loop through the y 
                for (int y = 1; y < rows - 1; y++)
                {
                    // At each index, add a new Vector3 the list using the x and y
                    dungeonTilesPositions.Add(new Vector3(x, y, 0f));
                }
            }
        }


        // Returns a random pos from the dungeonTilesPositions list
        Vector3 RandomDungeonTilePosition ()
		{
            // Define randomIndex, set the value to a random number between 0 and the no. of items in the list dungeonTilesPositions
            int randomIndex = Random.Range (0, dungeonTilesPositions.Count);

            // This variable's value is set to the entry at randomIndex from the List dungeonTilesPositions
            Vector3 randomPos = dungeonTilesPositions[randomIndex];

            // Get rid of the entry at randomIndex from the list so that it cannot be used again
            dungeonTilesPositions.RemoveAt (randomIndex);
			
			// Return the randomly selected Vector3 position
			return randomPos;
		}
		
		
		// Takes in an array of GameObjects to choose from alongside a min and max range for the number of objects to create
		void LayoutObjectAtRandom (GameObject[] dungeonTileArray, int min, int max)
		{
			// Choose a random amount of objects to create copies of within the min and max bounds
			int objectCount = Random.Range (min, max + 1);
			
			// Make copies of objects until the randomly chosen limit objectCount is reached
			for(int i = 0; i < objectCount; i++)
			{
                // Choose a pos for randomPosition by getting a random pos from the list of Vector3 types available that is stored in dungeonTilesPositions
                Vector3 randomPos = RandomDungeonTilePosition();

                // Get a random tile from dungeonTileArray and assign it to dungeonTileChosen
                GameObject dungeonTileChosen = dungeonTileArray[Random.Range (0, dungeonTileArray.Length)];

                // Make a copy of dungeonTileChosen at the position returned by randomPos (no rotation)
                Instantiate(dungeonTileChosen, randomPos, Quaternion.identity);
			}
		}
	}
}