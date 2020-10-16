using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MartianAttack
{
    public class BossScoreInfo : MonoBehaviour
    {
        // Variable to make the game object get destroyed after a while
        private float _timerToDestroyObject;

        // Variable to flash the object
        private float _flashObject;

        // Bool variables to enable/disable flashing
        private bool _firstFlash = true;

        private void Update()
        {
            // Keep increasing _flashObject in-order to flash it
            _flashObject += Time.deltaTime;

            if(_flashObject >= 1f && _firstFlash)
            {
                gameObject.GetComponent<TextMesh>().text = UIController.bossScore.ToString();
                _firstFlash = false;
                _flashObject = 0;
            }

            if (_flashObject >= 1f && _firstFlash == false)
            {
                gameObject.GetComponent<TextMesh>().text = UIController.bossScore.ToString();
                _firstFlash = true;
                _flashObject = 0;
            }

            // Increase this as time goes on
            _timerToDestroyObject += Time.deltaTime;

            // After 6 secs destroy this object
            if(_timerToDestroyObject >= 4f)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
