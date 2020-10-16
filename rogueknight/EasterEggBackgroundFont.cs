using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace RogueKnight
{
    // This class will change the font and background images over a few seconds
    public class EasterEggBackgroundFont : MonoBehaviour
    {
        // References to the background images, text (different fonts) and snowmen
        public GameObject rogueKnightBackgroundImage;
        public GameObject dodgeEmBackgroundImage;
        public GameObject leonsWorldBackgroundImage;
        public GameObject kitchenKhaosBackgroundImage;
        public GameObject martianAttackBackgroundImage;

        public GameObject snowman1;
        public GameObject snowman2;

        public GameObject easterEggRogueText;
        public GameObject easterEggDodgeEmText;
        public GameObject easterEggMartianAttackText;
        public GameObject easterEggLeonsWorldText;
        public GameObject easterEggKitchenKhaosText;

        public GameObject thankYouText;

        // Start the coroutine at once when the game object is enabled
        private void Start()
        {
            StartCoroutine(ChangeBackgroundFont());
        }

        IEnumerator ChangeBackgroundFont()
        {
            yield return new WaitForSeconds(0.25f);

            rogueKnightBackgroundImage.SetActive(false);
            dodgeEmBackgroundImage.SetActive(true);
            leonsWorldBackgroundImage.SetActive(false);
            kitchenKhaosBackgroundImage.SetActive(false);
            martianAttackBackgroundImage.SetActive(false);

            // Change it to the "Rogue Knight" font
            easterEggRogueText.GetComponent<Text>().enabled = true;
            easterEggDodgeEmText.GetComponent<Text>().enabled = false;
            easterEggMartianAttackText.GetComponent<Text>().enabled = false;
            easterEggLeonsWorldText.GetComponent<Text>().enabled = false;
            easterEggKitchenKhaosText.GetComponent<Text>().enabled = false;

            yield return new WaitForSeconds(0.25f);

            rogueKnightBackgroundImage.SetActive(false);
            dodgeEmBackgroundImage.SetActive(true);
            leonsWorldBackgroundImage.SetActive(false);
            kitchenKhaosBackgroundImage.SetActive(false);
            martianAttackBackgroundImage.SetActive(false);

            // Change it to the "Dodge 'Em" font
            easterEggRogueText.GetComponent<Text>().enabled = false;
            easterEggDodgeEmText.GetComponent<Text>().enabled = true;
            easterEggMartianAttackText.GetComponent<Text>().enabled = false;
            easterEggLeonsWorldText.GetComponent<Text>().enabled = false;
            easterEggKitchenKhaosText.GetComponent<Text>().enabled = false;

            yield return new WaitForSeconds(0.25f);

            rogueKnightBackgroundImage.SetActive(false);
            dodgeEmBackgroundImage.SetActive(false);
            leonsWorldBackgroundImage.SetActive(false);
            kitchenKhaosBackgroundImage.SetActive(false);
            martianAttackBackgroundImage.SetActive(true);

            // Change it to the "Martian Attack" font
            easterEggRogueText.GetComponent<Text>().enabled = false;
            easterEggDodgeEmText.GetComponent<Text>().enabled = false;
            easterEggMartianAttackText.GetComponent<Text>().enabled = true;
            easterEggLeonsWorldText.GetComponent<Text>().enabled = false;
            easterEggKitchenKhaosText.GetComponent<Text>().enabled = false;

            yield return new WaitForSeconds(0.25f);

            rogueKnightBackgroundImage.SetActive(false);
            dodgeEmBackgroundImage.SetActive(false);
            leonsWorldBackgroundImage.SetActive(true);
            kitchenKhaosBackgroundImage.SetActive(false);
            martianAttackBackgroundImage.SetActive(false);

            // Change it to the "Leon's World" font
            easterEggRogueText.GetComponent<Text>().enabled = false;
            easterEggDodgeEmText.GetComponent<Text>().enabled = false;
            easterEggMartianAttackText.GetComponent<Text>().enabled = false;
            easterEggLeonsWorldText.GetComponent<Text>().enabled = true;
            easterEggKitchenKhaosText.GetComponent<Text>().enabled = false;

            yield return new WaitForSeconds(0.25f);

            rogueKnightBackgroundImage.SetActive(false);
            dodgeEmBackgroundImage.SetActive(false);
            leonsWorldBackgroundImage.SetActive(false);
            kitchenKhaosBackgroundImage.SetActive(true);
            martianAttackBackgroundImage.SetActive(false);

            // Change it to the "Kitchen Khaos" font
            easterEggRogueText.GetComponent<Text>().enabled = false;
            easterEggDodgeEmText.GetComponent<Text>().enabled = false;
            easterEggMartianAttackText.GetComponent<Text>().enabled = false;
            easterEggLeonsWorldText.GetComponent<Text>().enabled = false;
            easterEggKitchenKhaosText.GetComponent<Text>().enabled = true;

            yield return new WaitForSeconds(0.25f);

            rogueKnightBackgroundImage.SetActive(false);
            dodgeEmBackgroundImage.SetActive(false);
            leonsWorldBackgroundImage.SetActive(false);
            kitchenKhaosBackgroundImage.SetActive(false);
            martianAttackBackgroundImage.SetActive(true);

            // Change it to the "Martian Attack" font
            easterEggRogueText.GetComponent<Text>().enabled = false;
            easterEggDodgeEmText.GetComponent<Text>().enabled = false;
            easterEggMartianAttackText.GetComponent<Text>().enabled = true;
            easterEggLeonsWorldText.GetComponent<Text>().enabled = false;
            easterEggKitchenKhaosText.GetComponent<Text>().enabled = false;

            yield return new WaitForSeconds(1.95f);

            // Disable the snowmen
            snowman1.SetActive(false);
            snowman2.SetActive(false);

            // Show the thank you text
            thankYouText.gameObject.GetComponent<Text>().enabled = true;

            yield return new WaitForSeconds(2f);

            // Load the god menu after a second
            SceneManager.LoadScene("godMenu");
        }
    }
}