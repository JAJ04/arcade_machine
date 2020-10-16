using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LeonsWorld
{
    public class EndCredits : MonoBehaviour
    {
        // Holds the property for the camera
	    public GameObject mainCamera;

        // The camera will move down at this speed
	    public int speed = 1;

        // This is the variable used to change the level
	    public string level;
	
	    // Update is called once per frame
	    void Update () {

            // Move the camera down every update at a certain speed
            mainCamera.transform.Translate(Vector3.down * Time.deltaTime * speed);
		
		    StartCoroutine(waitFor());
	    }

        // Load the specified level after 37 seconds
        IEnumerator waitFor()
	    {
		    yield return new WaitForSeconds(37);
		    SceneManager.LoadScene(level);
	    }
    }
}
