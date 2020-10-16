using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MartianAttack
{
    public class MainMenuAnimation : MonoBehaviour
    {
        // Game Objects to trigger the separate keyframes
        [SerializeField]
        private GameObject[] _aliens;

        // Variable to start the animation again
        public bool startAnim = true;

        // Use this for initialization
        void Update ()
        {
            if(startAnim == true)
            {
                // Start the couroutine below for the animation
                StartCoroutine(Animation());
            }
        }

        // This coroutine acts like an animation, it allows the Main_Screen 
        // to stretch on a variety of screen sizes whilst giving the illusion
        // of an animation
        public IEnumerator Animation()
        {
            startAnim = false;
            _aliens[0].gameObject.SetActive(true);
            _aliens[1].gameObject.SetActive(false);
            _aliens[2].gameObject.SetActive(false);
            _aliens[3].gameObject.SetActive(false);
            _aliens[4].gameObject.SetActive(false);
            _aliens[5].gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _aliens[0].gameObject.SetActive(false);
            _aliens[1].gameObject.SetActive(true);
            _aliens[2].gameObject.SetActive(false);
            _aliens[3].gameObject.SetActive(false);
            _aliens[4].gameObject.SetActive(false);
            _aliens[5].gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _aliens[0].gameObject.SetActive(false);
            _aliens[1].gameObject.SetActive(false);
            _aliens[2].gameObject.SetActive(true);
            _aliens[3].gameObject.SetActive(false);
            _aliens[4].gameObject.SetActive(false);
            _aliens[5].gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _aliens[0].gameObject.SetActive(false);
            _aliens[1].gameObject.SetActive(false);
            _aliens[2].gameObject.SetActive(false);
            _aliens[3].gameObject.SetActive(true);
            _aliens[4].gameObject.SetActive(false);
            _aliens[5].gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _aliens[0].gameObject.SetActive(false);
            _aliens[1].gameObject.SetActive(false);
            _aliens[2].gameObject.SetActive(false);
            _aliens[3].gameObject.SetActive(false);
            _aliens[4].gameObject.SetActive(true);
            _aliens[5].gameObject.SetActive(false);
            yield return new WaitForSeconds(1);
            _aliens[0].gameObject.SetActive(false);
            _aliens[1].gameObject.SetActive(false);
            _aliens[2].gameObject.SetActive(false);
            _aliens[3].gameObject.SetActive(false);
            _aliens[4].gameObject.SetActive(false);
            _aliens[5].gameObject.SetActive(true);
        }
    }
}