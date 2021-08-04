using UnityEngine;
using System.Collections;

namespace LeonsWorld 
{
    public class CameraFollow : MonoBehaviour 
    {
        // Gets player position
        public Transform Player;

        // Vector to keep the camera on the target of the player
        private Vector3 Direction;

        // Use this for initialization
        // This subtracts the world position and the player position to get the camera position
        void Start() 
	{
            Direction = transform.position - Player.transform.position;
        }

        // This changes the world position by getting the player direction and adding where the camera position is
        void LateUpdate()
	{
            transform.position = Player.transform.position + Direction;
        }
    }
}
