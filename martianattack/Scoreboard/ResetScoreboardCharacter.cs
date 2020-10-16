using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class ResetScoreboardCharacter : MonoBehaviour
    {
        // Use this for initialization and reset of the currentIndex in ScoreManager
        // on the scoreboard scene load
        void Start()
        {
            ScrollManager._currentIndex = 0;
        }
    }
}
