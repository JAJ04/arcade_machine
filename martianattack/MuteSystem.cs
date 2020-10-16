using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MuteSystem : MonoBehaviour
{
    // Bool for toggling the mute
    public static bool muteToggle = false;

    // Bool to set whether to mess with the mute icon in Leon's World 
    // when pausing/unpausing
    public static bool muteIconToggle = true;

    // Variable to hold the mute icon to switch it on/off
    [SerializeField]
    private GameObject _muteIcon;

    private void Awake()
    {
        DontDestroyOnLoad(this);

        // This is used to prevent duplicates from being created
        if (GameObject.FindGameObjectsWithTag("Mute").Length > 1)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        // If "M" key is pressed then mute/unmute game
        if (Input.GetKeyDown(KeyCode.M))
        {
            muteToggle = !muteToggle;
        }

        if (muteToggle == true)
        {
            // Show mute icon
            _muteIcon.SetActive(true);

            // Mute game
            AudioListener.volume = 0;
            muteToggle = true;
        }

        if (muteToggle == false)
        {
            // Disable mute icon
            _muteIcon.SetActive(false);

            // Unmute game
            AudioListener.volume = 1f;
            muteToggle = false;
        }
    }
}