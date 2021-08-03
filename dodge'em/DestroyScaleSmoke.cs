using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScaleSmoke : MonoBehaviour
{
    // Use this for initialization
    void Start ()
    {
        // Destroy the smoke after a certain amount of time
        Destroy(gameObject, 1f);
        StartCoroutine(ScaleDown());
    }

    // Scale down the smoke over time (every 0.05f of a second)
    IEnumerator ScaleDown()
    {
        gameObject.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.50f, 0.50f, 0.50f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.40f, 0.40f, 0.40f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.35f, 0.35f, 0.35f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.30f, 0.30f, 0.30f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.20f, 0.20f, 0.20f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.10f, 0.10f, 0.10f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        yield return new WaitForSeconds(0.05f);
        gameObject.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
    }
}
