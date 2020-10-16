using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Catch
{
    public class ScoreTracker : MonoBehaviour
    {
        // Variable that keeps track of the score
        public int score;

        // This is a variable that holds a reference to the UI text on the screen
        // to change it to show the most up to date score
        // The shadow will also change too for the nice backdrop
        public Text scoreText;
        public Text scoreTextShadow;

        // When the game first starts reset the score to 0
        void Start()
        {
            score = 0;
            UpdateScore();
        }

        // This updates the text on the reference above
        public void UpdateScore()
        {
            scoreText.text = score.ToString();
            scoreTextShadow.text = score.ToString();
        }

        // If this game object collided with the object in the tag, then
        // decrease the score by "1"
        void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Rotten")
            {
                score -= 2;
                UpdateScore();
            }
        }

        // Whenever the collider on this game object is entered by 
        // another object, increase the score and update the text
        void OnTriggerEnter2D(Collider2D other)
        {
            if(other.tag == "Fruit")
            {
                score += 2;
                UpdateScore();
            }
        }
    }
}