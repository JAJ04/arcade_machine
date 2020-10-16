using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UFO
{
    public class ResetCheatCodeUFO : MonoBehaviour
    {
        // On the scene load reset the array
        void Start()
        {
            CheatCodeUFO.UFOArrayIndex = 0;
        }
    }
}