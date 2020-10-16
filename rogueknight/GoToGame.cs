using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RogueKnight
{
    // This class will transition to the main game in the "Welcome" scene in a second and reset the dungeon variable
    public class GoToGame : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(GoToGameCoroutine());
        }

        IEnumerator GoToGameCoroutine()
        {
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("rogue-knight");
        }
    }
}
