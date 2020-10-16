using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UFO
{
    // Gives the text component on the game object this script is attached to
    [RequireComponent(typeof(Text))]
    public class HiscoreText : MonoBehaviour
    {
        // Reference to the text hiscore
        Text hiScore;

        // Text needs to be reset when the is started again
        private void OnEnable()
        {
            // Sets the text to what is in PlayerPrefs
            hiScore = GetComponent<Text>();
            // Concatenate "Hiscore: "
            hiScore.text = "Hiscore: " + PlayerPrefs.GetInt("UFO Hiscore").ToString();
        }
    }
}