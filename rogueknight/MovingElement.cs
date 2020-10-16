using UnityEngine;
using System.Collections;

namespace RogueKnight
{
	public abstract class MovingElement : MonoBehaviour
	{
        // BoxCollider2D, Rigidbody2D and float variable (which is used to make the movement more refined)
        internal BoxCollider2D boxCollider2D;
        // "internal" was added to prevent an error from happening
        internal new Rigidbody2D rigidbody2D;

        // Used for smooth movement
        private float oppositeMoveTime;

        // This is the time it will take for an element to move
        public float movingElementTime;
        // Layer for collision detection
		public LayerMask blockingLayerMask;        

        //Protected, virtual functions can be overridden by inheriting classes.
        protected virtual void Start ()
		{
            // Reference to "THIS" element's "BoxCollider2D" component
            boxCollider2D = GetComponent <BoxCollider2D> ();

            // Reference to "THIS" element's "Rigidbody2D" component
            rigidbody2D = GetComponent <Rigidbody2D> ();

            // Stores the reciprocal of movingElementTime we can use it by doing multiplication 
            oppositeMoveTime = 1f / movingElementTime;
        }
		
		
		// This function returns a "true" if it is available to move, otherwise it's false
		// Takes an x and y for direction and a RaycastHit2D to check for a collision
		protected bool MoveElement(float x, float y, out RaycastHit2D raycastHit)
		{
			// Use this variable to assign a start position to move from
			Vector2 startPos = transform.position;
			
			// This is used to calculate the end position based on direction params passed into this function
			Vector2 endPos = startPos + new Vector2 (x, y);

            // Casts a line from a start point to an end point whilst checking collision on blockingLayerMask
            raycastHit = Physics2D.Linecast (startPos, endPos, blockingLayerMask);
			
			// Check if something had been hit
			if(raycastHit.transform == null)
			{
				// If there was nothing to hit then start the "SmoothMovement" coroutine whilst passing the "Vector2" end as the destination
				StartCoroutine (SmoothElementMovement(endPos));
				
				// If there was a "Move" then return true
				return true;
			}
			
			// If there was a collision, the "Move" was not successful
			return false;
		}

        // The virtual keyword makes sure that "TryMove" can be overridden by inheriting classes using "override"
        // "TryMove" takes a generic parameter to specify the type of the component we expect our entity to interact with if the entity is blocked
        protected virtual void TryMove<T>(float x, float y)
            where T : Component
        {
            // Prevents the sprite from flipping back left when you are only moving up or down
            // Retains the last flipped state

            if (!Mathf.Approximately(transform.position.x, (transform.position.x + x)))
            {
                // Flip the sprites dependent on this condition
                if (Mathf.Round(transform.position.x) < Mathf.Round(transform.position.x + x))
                {
                    GetComponent<SpriteRenderer>().flipX = false;
                }
                else
                {
                    GetComponent<SpriteRenderer>().flipX = true;
                }
            }

            // Hit will hold what the linecast hits when "MoveElement" is invoked
            RaycastHit2D hit;
            
            // canMove is set to true if "MoveElement" was successful
            bool moveAllowed = MoveElement(x, y, out hit);

            // This checks to see if nothing was hit by the linecast
            if (hit.transform == null)
            {
                // If nothing was hit at all then get out of this function
                return;
            }

            // Gets a "component" reference to the type T (a generic) that is attached to the element that was hit
            T hitComponent = hit.transform.GetComponent<T>();

            // If canMove is false and also hitComponent is not equal to null
            if (!moveAllowed && hitComponent != null)
            {
                // Call the IfCantMove function and pass hitComponent as the param
                IfCantMove(hitComponent);
            }
        }

        // This is a coroutine for moving entities from one space to the next space
        protected IEnumerator SmoothElementMovement(Vector3 end)
		{
            // Ensure we move whole units
            end.x = Mathf.CeilToInt(end.x);
            end.y = Mathf.CeilToInt(end.y);

			// This variable is used to calculate the leftover distance to move along by using the square magnitude of the difference between the current pos and end param
			// Square magnitude is used instead of magnitude because it's computationally cheaper.
			float sqredRemainingDistance = (transform.position - end).sqrMagnitude;

			// While sqredRemainingDistance is greater than a miniscule amount:
			while(sqredRemainingDistance > float.Epsilon)
			{
				// Find a new position that is closer to the end based on the oppositeMoveTime
				Vector3 newPos = Vector3.MoveTowards(rigidbody2D.position, end, oppositeMoveTime * Time.deltaTime);
				
				// Call the MovePosition on the attached "Rigidbody2D" and move it to the calculated pos
				rigidbody2D.MovePosition (newPos);

                // Calculate the leftover distance again after an element
                sqredRemainingDistance = (transform.position - end).sqrMagnitude;

                // Return and keep repeating until sqredRemainingDistance is approximately zero to end this function
                yield return null;
			}
		}        

        // This function in an inherited class
        protected abstract void IfCantMove <T> (T component)
            where T : Component;			
	}
}