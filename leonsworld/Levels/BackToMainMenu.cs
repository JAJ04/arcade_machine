using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LeonsWorld
{
    public class BackToMainMenu : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            // If "LEFT ALT" is pressed then return to the god main menu
            if (Input.GetKeyDown(KeyCode.LeftAlt))
            {
                SceneManager.LoadScene("godMenu");
            }
        }
    }
}
