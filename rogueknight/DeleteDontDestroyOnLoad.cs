using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RogueKnight
{
    public class DeleteDontDestroyOnLoad : MonoBehaviour
    {
        // Destroy both "GameManager" and "SoundManager" instances to prevent errors
        void Awake()
        {
            // Reset the easter egg count also
            GameManager.easterEggCount = 0;

            if (GameObject.Find("GameManager(Clone)") && GameObject.Find("SoundPrefab(Clone)"))
            {
                Destroy(GameObject.Find("GameManager(Clone)").gameObject);
                Destroy(GameObject.Find("SoundPrefab(Clone)").gameObject);
            }
        }
    }
}