using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetMuteToggleIcon : MonoBehaviour 
{
    // Update is called once per frame

    // Allows the mute icon to be used properly outside of the Leon's World
    // gameplay scenes again
    void Update() 
    {
        MuteSystem.muteIconToggle = true;
    }
}
