using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace LeonsWorld
{
    public class ChangeScene : MonoBehaviour
    {
        // Reference to the pause text in the scene to disable it
        public GameObject pauseText;

        // This script is used to change the scene and
        // plays a sound after 0.25f
        public void ChangeToScene(string sceneToChangeTo)
        {
            // Unpause the game and disable the pause menu only on the gameplay scenes
            if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("level1") ||
                SceneManager.GetActiveScene() == SceneManager.GetSceneByName("level1-boss") ||
                SceneManager.GetActiveScene() == SceneManager.GetSceneByName("level2"))
            {
                pauseText.SetActive(false);
                Time.timeScale = 1;
                GameManager.pauseMenu = false;
                GameManager.keysActive = true;

                // Don't allow the pause menu from being enabled whilst a scene transition is in progress
                GameManager.canEnableMenu = false;
            }

            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();

            // This makes it so the scene will switch after 0.25f seconds
            StartCoroutine(DelaySwitch(sceneToChangeTo));
        }

        IEnumerator DelaySwitch(string sceneToChangeTo)
        {
            yield return new WaitForSeconds(0.25f);
            SceneManager.LoadScene(sceneToChangeTo);
        }
    }
}
