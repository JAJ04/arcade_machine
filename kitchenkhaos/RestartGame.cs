using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Catch
{
    public class RestartGame : MonoBehaviour
    {
        void Update()
        {
            // When the game is over it checks to see if
            // space is pressed, if it is, reload the scene
            // using SceneManager
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }
}
