using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MartianAttack 
{
    public class ChangeBossColor: MonoBehaviour 
    {
        // Used to slow down the changing of the sprite color
        private float _timer = 0;

        // Update is called once per frame
        void Update() 
	{
            // Keep changing the sprite color after 2 seconds
            _timer += Time.deltaTime;

            if (_timer > 1 f) 
	    {
                // Keep changing colors over and over again
                gameObject.GetComponent < Image > ().color = Random.ColorHSV();
                _timer = 0 f;
            }
        }
    }
}
