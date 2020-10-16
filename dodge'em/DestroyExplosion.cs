using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DodgeEm
{
    public class DestroyExplosion : MonoBehaviour
    {
        // After instantiation destroy the object after 0.5f seconds
        void Start()
        {
            Destroy(gameObject, 0.5f);
        }
    }
}