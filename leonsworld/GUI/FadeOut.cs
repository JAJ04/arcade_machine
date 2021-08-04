using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeOut : MonoBehaviour
{
    // REFERENCE: https://forum.unity.com/threads/fading-in-out-gui-text-with-c-solved.380822/

    // can ignore the update, it's just to make the coroutines get called for example
    void Update()
    {
        StartCoroutine(FadeTextToZeroAlpha(2f, GetComponent<Text>()));
    }

    // This increases the transparency of the text over time i.e. Time.deltaTime which is
    // divided by a value "t" (2f) to make it so the transparency gets reached in 2 seconds
    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}
