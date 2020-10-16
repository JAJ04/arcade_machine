using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MartianAttack
{
    public class ScrollManager : MonoBehaviour
    {
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

        // This variable is used to increment and decrement through the alphabet
        // Static is used so that the other script can only increase this variable for the alphabet
        public static int _currentIndex = 0;
        // Used to check if space is pressed 3 times
        public static int _spacePressedCounter = 0;
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
            // If Space key is pressed enter a character
            if (Input.GetKeyDown(KeyCode.Space) && _spacePressedCounter <= 4 && SceneController.pauseMenuActive == false)
            {
                // Confirm selection quickly
                EnterCharacter();
                // Increase counter
                _spacePressedCounter++;
            }

            // If 3 characters are entered jump to main screen
            if (Input.GetKeyDown(KeyCode.Space) && _spacePressedCounter > 2)
            {
                // Turn off Selected_Character text and Username text
                GameObject.Find("Selected_Character").SetActive(false);
                GameObject.Find("Username").SetActive(false);
                // Switch back to the main scene (this actually delays the switching for some reason)
                SceneManager.LoadScene("game");
            }

            // Used to put characters into an InputField
            SelectCharacter();
        }

        private void SelectCharacter()
        {
            // Used to speed up character selection
            _aHoldTime += 1 * Time.deltaTime;

            // Reset the hold time to increase character selection
            if (!Input.GetKey(KeyCode.A) && SceneController.pauseMenuActive == false)
            {
                _aHoldTime = 0;
            }

            // Speed through the character selection
            if (_aHoldTime >= _fastSelectConfirmTime)
            {
                if (_currentIndex >= -1 && _aHoldTime >= _fastSelectConfirmTime + _secondsPerFastCharSelect)
                {
                    // Decrement through the alphabet and allow wrapping to Z
                    _currentIndex -= 1;
                    if (_currentIndex == -1)
                    {
                        _currentIndex = 25;
                    }
                    // Play key stroke
                    AudioSource.PlayClipAtPoint(_keyStroke, Camera.main.transform.position);
                    // Display the selected char on screen
                    GameObject.Find("Selected_Character").GetComponent<Text>().text = "Selected Char: " + _alphabet[_currentIndex].ToString() + ".";
                    _aHoldTime = _fastSelectConfirmTime;
                }
            }

            // Used to speed up character selection
            _dHoldTime += 1 * Time.deltaTime;
            if (!Input.GetKey(KeyCode.D) && SceneController.pauseMenuActive == false)
            {
                _dHoldTime = 0;
            }

            // Speed through the character selection
            if (_dHoldTime >= _fastSelectConfirmTime)
            {
                if (_currentIndex <= 26 && _dHoldTime >= _fastSelectConfirmTime + _secondsPerFastCharSelect) 
                {
                    // Increment through the alphabet and allow wrapping to A
                    _currentIndex += 1;
                    if (_currentIndex == 26)
                    {
                        _currentIndex = 0;
                    }
                    // Play key stroke
                    AudioSource.PlayClipAtPoint(_keyStroke, Camera.main.transform.position);
                    // Display the selected char on screen
                    GameObject.Find("Selected_Character").GetComponent<Text>().text = "Selected Char: " + _alphabet[_currentIndex].ToString() + ".";
                    _dHoldTime = _fastSelectConfirmTime;
                }
            }

            if (Input.GetKeyDown(KeyCode.A) && SceneController.pauseMenuActive == false)
            {
                // Play key stroke
                AudioSource.PlayClipAtPoint(_keyStroke, Camera.main.transform.position);
                // Decrement through the alphabet and allow wrapping to Z
                _currentIndex -= 1;
                if (_currentIndex == -1)
                {
                    _currentIndex = 25;
                }
                // Display the selected char on screen
                GameObject.Find("Selected_Character").GetComponent<Text>().text = "Selected Char: " + _alphabet[_currentIndex].ToString() + ".";
            }
            else if (Input.GetKeyDown(KeyCode.D) && SceneController.pauseMenuActive == false)
            {
                // Play key stroke
                AudioSource.PlayClipAtPoint(_keyStroke, Camera.main.transform.position);
                // Increment through the alphabet and allow wrapping to A
                _currentIndex += 1;
                if (_currentIndex == 26)
                {
                    _currentIndex = 0;
                }
                // Display the selected char on screen
                GameObject.Find("Selected_Character").GetComponent<Text>().text = "Selected Char: " + _alphabet[_currentIndex].ToString() + ".";
            }

            _characterConfirmTime += 1f * Time.deltaTime;
        }

        private void EnterCharacter()
        {
            // Play key stroke char entered sound
            AudioSource.PlayClipAtPoint(_keyStrokeInput, Camera.main.transform.position);

            // Add onto the username by getting alphabet characters
            _userChars.Add(_alphabet[_currentIndex]);
            // This allows the ability to get another character
            _characterConfirmTime = 0f;

            // This username derives from _userChars
            _username = new string(_userChars.ToArray());
            // Put the username into the InputField
            _playerName.text = _username;
        }
    }
}