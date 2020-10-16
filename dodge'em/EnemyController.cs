using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DodgeEm
{
    public class EnemyController : MonoBehaviour
    {
        // Variable to move the enemy at
        private float enemySpeed = 7f;

        // Update is called once per frame
        void Update()
        {
            // Keep updating the position with a Vector3
            transform.Translate(new Vector3(0f, 1, 0f) * enemySpeed * Time.deltaTime);
        }
    }
}