using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LeonsWorld
{
    public class Checker : MonoBehaviour
    {
        // Get properties of the game manager (reference)
        public GameManager gameManager;

        // Get a reference to the "angel"/"death" sound
        public AudioSource angel;
	
	    void Update()
	    {
            // If the game manager is deleted in other words
		    if(gameManager == null)
		    {
			    StartCoroutine("WaitForSeconds");
		    }
	    }
	
	    // Update is called once per frame
	    void RestartScene ()
	    {
            // Reload the scene
		    if(gameManager == null)
		    {
			    StartCoroutine("WaitForSeconds");
			    SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
			    Time.timeScale = 1f;
		    }
	    }
	
        // After a certain amount of seconds this will return
	    public static class CoroutineUtil
	    {
		    public static IEnumerator WaitForRealSeconds(float time)
		    {
			    float start = Time.realtimeSinceStartup;
			    while (Time.realtimeSinceStartup < start + time)
			    {
				    yield return null;
			    }
		    }
	    }

        // Reloads the scene after 5 seconds
	    IEnumerator WaitForSeconds()
	    {
		    yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(5));
		    RestartScene ();
	    }
    }
}
