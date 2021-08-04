using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LeonsWorld 
{
    public class ChangeSceneLevelTwo: MonoBehaviour 
    {
        // Normal click sound 
        public AudioSource normalClick;

        // Negative sound so it lets the player know that they cannot select
        // the level
        public AudioSource secondLevelNegative;

        public void ChangeToScene(string sceneToChangeTo) 
	{
            // If the level is unlocked in the PlayerPrefs then you can change to the second level
            if (PlayerPrefs.GetInt("LevelUnlock", 0) == 3) 
	    {
                normalClick.Play();
		StartCoroutine(ChangeSceneAfterHalfSecond(sceneToChangeTo));
            }

            if (PlayerPrefs.GetInt("LevelUnlock", 0) < 3) 
	    {
                // Play the negative sound
                secondLevelNegative.Play();
            }
        }

        // This will change the scene after half of a second
        IEnumerator ChangeSceneAfterHalfSecond(string sceneToChangeTo) 
	{
            yield return new WaitForSeconds(0.5 f);
            SceneManager.LoadScene(sceneToChangeTo);
        }
    }
}
