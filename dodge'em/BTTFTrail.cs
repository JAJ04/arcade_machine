using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DodgeEm
{
    public class BTTFTrail : MonoBehaviour
    {
        // Fire trail elements to display
        public GameObject fireTrailLeft1;
        public GameObject fireTrailLeft2;
        public GameObject fireTrailLeft3;
        public GameObject fireTrailLeft4;
        public GameObject fireTrailLeft5;
        public GameObject fireTrailLeft6;
        public GameObject fireTrailLeft7;
        public GameObject fireTrailLeft8;
        public GameObject fireTrailLeft9;
        public GameObject fireTrailLeft10;
        public GameObject fireTrailLeft11;
        public GameObject fireTrailLeft12;
        public GameObject fireTrailLeft13;
        public GameObject fireTrailRight1;
        public GameObject fireTrailRight2;
        public GameObject fireTrailRight3;
        public GameObject fireTrailRight4;
        public GameObject fireTrailRight5;
        public GameObject fireTrailRight6;
        public GameObject fireTrailRight7;
        public GameObject fireTrailRight8;
        public GameObject fireTrailRight9;
        public GameObject fireTrailRight10;
        public GameObject fireTrailRight11;
        public GameObject fireTrailRight12;
        public GameObject fireTrailRight13;

        // Use this for initialization
        void Start()
        {
            StartCoroutine(FlamesDelay());

            // Destroy this game object after 4 seconds
            Destroy(gameObject, 4f);
        }

        // Delays the flame trays a little
        IEnumerator FlamesDelay()
        {
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft1.SetActive(true);
            fireTrailRight1.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft2.SetActive(true);
            fireTrailRight2.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft3.SetActive(true);
            fireTrailRight3.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft4.SetActive(true);
            fireTrailRight4.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft5.SetActive(true);
            fireTrailRight5.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft6.SetActive(true);
            fireTrailRight6.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft7.SetActive(true);
            fireTrailRight7.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft8.SetActive(true);
            fireTrailRight8.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft9.SetActive(true);
            fireTrailRight9.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft10.SetActive(true);
            fireTrailRight10.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft11.SetActive(true);
            fireTrailRight11.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft12.SetActive(true);
            fireTrailRight12.SetActive(true);
            yield return new WaitForSeconds(0.08f);
            fireTrailLeft13.SetActive(true);
            fireTrailRight13.SetActive(true);
        }
    }
}
