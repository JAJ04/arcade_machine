using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipToLevel1 : MonoBehaviour 
{
    // Reference to the sound to play
    public AudioSource backSound;

    // Bool to only play the sound once
    bool playSoundOnce = true;

    // Update is called once per frame
    void Update() 
    {
        // Change to level 1 when this is attached to a game object during the cutscenes for level 1
        if (Input.GetKeyDown(KeyCode.LeftAlt)) 
	{
            StartCoroutine(GoBackToLevel1());
        }
    }

    // Play a sound and go to the quit menu
    IEnumerator GoBackToLevel1() 
    {
        // Play the sound only once
        if (playSoundOnce)
	{
            backSound.Play();
            playSoundOnce = false;
        }

        yield return new WaitForSeconds(0.25 f);

        SceneManager.LoadScene("level1");
    }
}
