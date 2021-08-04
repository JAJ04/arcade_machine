using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeIn : MonoBehaviour
{
    // REFERENCE: https://forum.unity.com/threads/fading-in-out-gui-text-with-c-solved.380822/

    // can ignore the update, it's just to make the coroutines get called for example
    void Update()
    {
        StartCoroutine(FadeTextToFullAlpha(2f, GetComponent<Text>()));
    }

    // This increases the opacity of the text over time i.e. Time.deltaTime which is
    // divided by a value "t" (2f) to make it so the opacity gets reached in 2 seconds
    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 0);
        
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }
    }
}
