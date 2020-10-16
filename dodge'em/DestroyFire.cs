using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DodgeEm
{
    public class DestroyFire : MonoBehaviour
    {
        // After instantiation destroy the object after 3 seconds
        void Start()
        {
            Destroy(gameObject, 1.5f);
        }
    }
}