using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DodgeEm
{
    public class MoveRoad : MonoBehaviour
    {
        // Vector2 that controls the scale of the track moving
        private Vector2 roadOffset;

        // Speed at which the track moves
        public float roadSpeed = 1.5f;

        // Update is called once per frame
        void Update()
        {
            // Only touches the X axis and not the Y axis
            // Defines a vector that is updated according to the time
            // that the has ran and the roadSpeed
            
            // Apply this value (the offset) to the material on the Quad
            roadOffset = new Vector2(Time.time * roadSpeed, 0);
            GetComponent<Renderer>().material.mainTextureOffset = roadOffset;
        }
    }
}