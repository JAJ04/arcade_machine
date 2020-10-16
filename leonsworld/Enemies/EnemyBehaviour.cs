using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class EnemyBehaviour : MonoBehaviour {
	
        // Get the start and end sights for the enemy to "see" the player
	    public Transform sightStart, sightEnd;
	
        // Bools to trigger when the player has been spotted
	    public bool spotted = false;
	    public bool facingLeft = true;
	
        // Variable that holds the exclamation mark
	    public GameObject arrow;
	
	    void Start()
	    {
            // Starts the method "Patrol" and starts it between 2 and 6 seconds
		    InvokeRepeating("Patrol", 0f, Random.Range(2f, 6f));
	    }
	
	    // Update is called once per frame
	    void Update () 
	    {
            // Keep calling "Raycasting" and "Behaviours"
		    Raycasting();
		    Behaviours();
	    }
	
        // Draw a line between the start and end point and make it green
	    void Raycasting()
	    {
		    Debug.DrawLine(sightStart.position, sightEnd.position, Color.green);
            // Uses bitshifting to only target the "Player" layer
		    spotted = Physics.Linecast(sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer("Player"));
	    }
	
	    void Behaviours()
	    {
            // Show the exclamation mark if the player has been spotted, otherwise, don't
		    if(spotted == true)
		    {
			    arrow.SetActive(true);
		    }
		    else
		    {
			    arrow.SetActive(false);
		    }
	    }
	
	    void Patrol()
	    {
            // Keep flipping/unflipping the player sprite according to the
            // facingLeft variable
		    facingLeft =! facingLeft;
		
		    if(facingLeft == true)
		    {
			    transform.eulerAngles = new Vector2(0, 0);
		    }
		    else
		    {
			    transform.eulerAngles = new Vector2(0, 180);
		    }
	    }
	
        // If the enemy has taken damage then flash the enemy
	    public IEnumerator TakenDamage()
	    {
		    GetComponent<Renderer>().enabled = false;
		    yield return new WaitForSeconds(1);
		    GetComponent<Renderer>().enabled = true;
		    yield return new WaitForSeconds(1);
		    GetComponent<Renderer>().enabled = false;
		    yield return new WaitForSeconds(1);
		    GetComponent<Renderer>().enabled = true;
	    }
    }
}