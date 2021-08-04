using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipCredits: MonoBehaviour 
{
    // Reference to the audio to play
    public AudioSource backSound;

    // Bool to only play the sound once
    bool playSoundOnce = true;

    // Update is called once per frame
    void Update()
    {
        // If the "LEFT ALT" key is pressed then jump straight to the menu
        if (Input.GetKeyDown(KeyCode.LeftAlt)) 
	{
            StartCoroutine(GoBackToMenu());
        }
    }

    // Play a sound and go to the quit menu
    IEnumerator GoBackToMenu() 
    {
        // Play the sound only once
        if (playSoundOnce) 
	{
            backSound.Play();
            playSoundOnce = false;
        }

        yield return new WaitForSeconds(0.25 f);

        SceneManager.LoadScene("menu");
    }
}
