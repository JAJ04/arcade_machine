using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MartianAttack
{
    public class ScrollManager2 : MonoBehaviour
    {
        // This script is used so the player can use the UP/DOWN on the joystick

        // Variable to store the entire username at the end
        private string _username = "";

        // Used to change the InputField text
        public InputField _playerName;

        // AudioSource variable to play for the keystroke
        [SerializeField]
        AudioClip _keyStroke;

        // AudioSource variable to play for the keystroke when a char is put into the InputField
        [SerializeField]
        AudioClip _keyStrokeInput;

        // This has a list of all of the alphabet characters
        private char[] _alphabet = new char[] {'A', 'B', 'C','D', 'E', 'F', 'G', 'H', 'I', 'J',
            'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        // Used to store the entire username also as selection goes along
        private List<char> _userChars = new List<char>();

        // How long it takes to confirm a character
        private float _characterConfirmTime = 0f;
        // Used to determine when to speed up selection
        private float _aHoldTime = 0f;
        private float _dHoldTime = 0f;

        // The amount of seconds it should take to select the next/previous character while in fast select mode
        private float _secondsPerFastCharSelect = 0.10f;
        // The amount of seconds it should take to confirm to go in fast select mode
        private float _fastSelectConfirmTime = 0.5f; 

        private void Update()
        {
            // Used to put characters into an InputField
            SelectCharacter();
        }

        private void SelectCharacter()
        {
            // Used to speed up character selection
            _aHoldTime += 1 * Time.deltaTime;

            // Reset the hold time to increase character selection
            if (!Input.GetKey(KeyCode.S) && SceneController.pauseMenuActive == false)
            {
                _aHoldTime = 0;
            }

            // Speed through the character selection
            if (_aHoldTime >= _fastSelectConfirmTime)
            {
                if (ScrollManager._currentIndex >= -1 && _aHoldTime >= _fastSelectConfirmTime + _secondsPerFastCharSelect)
                {
                    // Decrement through the alphabet and allow wrapping to Z
                    ScrollManager._currentIndex -= 1;
                    if (ScrollManager._currentIndex == -1)
                    {
                        ScrollManager._currentIndex = 25;
                    }
                    // Play key stroke
                    AudioSource.PlayClipAtPoint(_keyStroke, Camera.main.transform.position);
                    // Display the selected char on screen
                    GameObject.Find("Selected_Character").GetComponent<Text>().text = "Selected Char: " + _alphabet[ScrollManager._currentIndex].ToString() + ".";
                    _aHoldTime = _fastSelectConfirmTime;
                }
            }

            // Used to speed up character selection
            _dHoldTime += 1 * Time.deltaTime;
            if (!Input.GetKey(KeyCode.W) && SceneController.pauseMenuActive == false)
            {
                _dHoldTime = 0;
            }

            // Speed through the character selection
            if (_dHoldTime >= _fastSelectConfirmTime)
            {
                if (ScrollManager._currentIndex <= 26 && _dHoldTime >= _fastSelectConfirmTime + _secondsPerFastCharSelect) 
                {
                    // Increment through the alphabet and allow wrapping to A
                    ScrollManager._currentIndex += 1;
                    if (ScrollManager._currentIndex == 26)
                    {
                        ScrollManager._currentIndex = 0;
                    }
                    // Play key stroke
                    AudioSource.PlayClipAtPoint(_keyStroke, Camera.main.transform.position);
                    // Display the selected char on screen
                    GameObject.Find("Selected_Character").GetComponent<Text>().text = "Selected Char: " + _alphabet[ScrollManager._currentIndex].ToString() + ".";
                    _dHoldTime = _fastSelectConfirmTime;
                }
            }

            if (Input.GetKeyDown(KeyCode.S) && SceneController.pauseMenuActive == false)
            {
                // Play key stroke
                AudioSource.PlayClipAtPoint(_keyStroke, Camera.main.transform.position);
                // Decrement through the alphabet and allow wrapping to Z
                ScrollManager._currentIndex -= 1;
                if (ScrollManager._currentIndex == -1)
                {
                    ScrollManager._currentIndex = 25;
                }
                // Display the selected char on screen
                GameObject.Find("Selected_Character").GetComponent<Text>().text = "Selected Char: " + _alphabet[ScrollManager._currentIndex].ToString() + ".";
            }
            else if (Input.GetKeyDown(KeyCode.W) && SceneController.pauseMenuActive == false)
            {
                // Play key stroke
                AudioSource.PlayClipAtPoint(_keyStroke, Camera.main.transform.position);
                // Increment through the alphabet and allow wrapping to A
                ScrollManager._currentIndex += 1;
                if (ScrollManager._currentIndex == 26)
                {
                    ScrollManager._currentIndex = 0;
                }
                // Display the selected char on screen
                GameObject.Find("Selected_Character").GetComponent<Text>().text = "Selected Char: " + _alphabet[ScrollManager._currentIndex].ToString() + ".";
            }

            _characterConfirmTime += 1f * Time.deltaTime;
        }

        private void EnterCharacter()
        {
            // Play key stroke char entered sound
            AudioSource.PlayClipAtPoint(_keyStrokeInput, Camera.main.transform.position);

            // Add onto the username by getting alphabet characters
            _userChars.Add(_alphabet[ScrollManager._currentIndex]);
            // This allows the ability to get another character
            _characterConfirmTime = 0f;

            // This username derives from _userChars
            _username = new string(_userChars.ToArray());
            // Put the username into the InputField
            _playerName.text = _username;
        }
    }
}