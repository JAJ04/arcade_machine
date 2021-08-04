using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    // Use this for initialization
    void Start() 
    {
        Destroy(gameObject, 5 f);
    }
}
