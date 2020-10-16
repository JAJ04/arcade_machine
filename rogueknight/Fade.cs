using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// REFERENCE: https://forum.unity.com/threads/simple-ui-animation-fade-in-fade-out-c.439825/

public class Fade : MonoBehaviour
{
    // the image you want to fade, assign in inspector
    public Image image;

    public void Start()
    {
        // Fades the image out on start
        StartCoroutine(FadeImage(false));
    }

    IEnumerator FadeImage(bool fadeOut)
    {
        // Fade to transparent
        if (fadeOut)
        {
            // Do the code in the "else" part but backwards with the "for loop" going backwards
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                image.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
        // This will fade so that the colour is solid
        else
        {
            // This will happen over a second
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                // Transition to opaque with this code
                image.color = new Color(1, 1, 1, i);
                yield return null;
            }
        }
    }
}