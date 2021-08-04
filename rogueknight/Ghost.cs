using UnityEngine;
using System.Collections;

namespace RogueKnight 
{
    // This class inherits from MovingElement, the base class for objects that can move
    public class Ghost: MovingElement 
    {
        // The amount of food points to subtract from the knight when attacking
        public int knightDamage;

        // Audio clips to play when attacking the knight
        public AudioClip attackKnightSound1;
        public AudioClip attackKnightSound2;

        // Determines whether or not an ghost should move a turn or not
        private bool doMove;
        // Moves the ghost towards a target
        private Transform target;

        // Override the start function in the base class
        protected override void Start() 
	{
            // Adds "THIS" ghost that is instantiated to a list in the GameManager
            GameManager.gmInstance.AddGhostToList(this);

            // Find the Knight using the tag for it and get the transform component
            target = GameObject.FindGameObjectWithTag("Knight").transform;

            // Invokes the start function in the base class
            base.Start();
        }

        // MoveGhost is called by the GameManager each turn to tell each Ghost to try to move towards the knight
        public void MoveGhost() 
	{
            // These values allow us to change direction
            float xDir = 0;
            float yDir = 0;

            // Get angle from ghost to player
            Vector2 angle = (target.position - transform.position).normalized;

            // The further away from our facing up angle that the angle to the player is, the closer this value will be to 0, and it will go all the way to -1 if player is in opposite direction
            float dotY = Vector3.Dot(Vector3.up, angle);

            // Same thing here but for left/right
            float dotX = Vector3.Dot(Vector3.right, angle);

            //If dotY is -1 and absoluted we'd get +1, and if +1 it would remain +1, but if zero still be zero, meaning player is not above or below us.
            //We compare the absolute values for facing angle on X and Y and whichever is greater is the angle we're most facing the player on
            if (Mathf.Abs(dotY) > Mathf.Abs(dotX)) 
	    {
                xDir = 0;
                yDir = dotY; //Assign the non-absolute dot value for up direction, since it will have negative value if player is opposite direction, shooting ray in that direction instead
            } 
	    else 
	    {
                xDir = dotX;
                yDir = 0;
            }

            // Invoke the TryMove function and pass in the Knight
            TryMove<Knight> (xDir, yDir);
        }

        // Overrides the TryMove function in the base class to include functionality needed for the Ghost to move at certain times
        protected override void TryMove <T> (float x, float y) 
	{
            // If skipMove is true then set it to false and skip this move
            if (doMove) 
	    {
                doMove = false;
                return;
            }

            // Attempt a move by calling the appropriate function in the base class
            base.TryMove<T> (x, y);

            // Skip next move after the ghost has moved
            doMove = true;
        }

        // This is invoked if the ghost tries to go into a space which is occupied by the knight, it overrides the OnCantMove function of MovingObject 
        // Takes a generic parameter T which is the knight we may encounter
        protected override void IfCantMove <T> (T knightComponent) 
	{
            // Declare hitKnight and set it to equal the encountered component
            Knight hitKnight = knightComponent as Knight;

            // Decrease your food if the knight hits a ghost
            hitKnight.LoseEnergy(knightDamage);

            // RandomizeSfx function is called to play either of these sounds
            SoundManager.instance.RandomSoundEffect(attackKnightSound1, attackKnightSound2);
        }
    }
}
