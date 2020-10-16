using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class ResetCreditsEntry : MonoBehaviour
    {
        // Use this for initialization
        void Start()
        {
            // Reset the entry cheat codes when the scenes switch because
            // Start() is the first thing executed on the scene switch
            CheatCodes.cArrayIndex = 0;
            CheatCodes.arrayIndex = 0;
        }
    }
}
