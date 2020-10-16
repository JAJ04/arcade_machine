using UnityEngine;
using System.Collections;

namespace RogueKnight
{
	public class BreakableWall : MonoBehaviour
	{
        // How many hits the knight has to give in-order for the wall to break
        public int wallHealth = 4;

        // Audio clips that play when the knight attacks the wall
        public AudioClip hitSound1;				
		public AudioClip hitSound2;
        public AudioClip chestOpener;

        // This is called when the knight attacks a wall
        public void HitWall (int loss)
		{
			// RandomizeSfx plays one of two chop sounds
			SoundManager.instance.RandomSoundEffect(hitSound1, hitSound2);

            // Decrease wallHealth from the loss
            wallHealth -= loss;
			
			// If hit points are less than or equal to zero then
			if(wallHealth <= 0)
            {
                // Disable "THIS" game object
                gameObject.SetActive(false);
            }

            // If hit points are less than or equal to zero and it is the chest then
            if (wallHealth <= 0 && gameObject.name == "TileDamage1(Clone)")
            {
                // Disable "THIS" game object
                gameObject.SetActive(false);
                // Increase easter egg counter
                GameManager.easterEggCount++;
                // Play the chest open sound
                if(chestOpener != null)
                {
                    AudioSource.PlayClipAtPoint(chestOpener, Camera.main.transform.position);
                }    
            }
        }
	}
}
