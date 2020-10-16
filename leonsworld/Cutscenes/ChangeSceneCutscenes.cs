using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeonsWorld
{
    public class ChangeSceneCutscenes : MonoBehaviour
    {
        // This will be used to store the value entered in the inspector
        public string sceneToChangeTo;
        // This will be used to specify how long to wait until the scene will change
        public int changeSceneAfterX;

        // This is ran when the script first executes
        void Start()
        {
            // Start the coroutine
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            // Wait X seconds before changing the scene
            yield return new WaitForSeconds(changeSceneAfterX);

            // Change to the scene specified in the inspector
            SceneManager.LoadScene(sceneToChangeTo);
        }
    }
}