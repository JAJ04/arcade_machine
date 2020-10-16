using UnityEngine;
using System.Collections;

namespace LeonsWorld
{
    public class BossBehaviour : MonoBehaviour
    {
	    // Used to draw an exclamation mark and I can enable it at a later point if need be
	    public Transform sightStart, sightEnd;
	
        // Detects if the player was spotted
	    public bool spotted = false;
        // The boss faces left by default
	    public bool facingLeft = true;
	
        // This property holds the exclamation mark texture
	    public GameObject arrow;
	
	    void Start()
	    {
            // Keep calling the "Patrol" method every x second between 2 and 6
		    InvokeRepeating("Patrol", 0f, Random.Range(2f, 6f));
	    }
	
	    // Update is called once per frame
	    void Update () 
	    {
            // Keep calling these methods
		    Raycasting();
		    Behaviours();
	    }
	
	    void Raycasting()
	    {
            // Check to see when the raycast line is hitting the "Player" layer
		    Debug.DrawLine(sightStart.position, sightEnd.position, Color.green);
		    spotted = Physics.Linecast(sightStart.position, sightEnd.position, 1 << LayerMask.NameToLayer("Player"));
	    }
	
	    void Behaviours()
	    {
            // Show the exclamation mark if the player is spotted
		    if(spotted == true)
		    {
			    arrow.SetActive(true);
		    }
		    else
		    {
			    arrow.SetActive(false);
		    }
	    }
	
        // Keep moving one way (to the left)
	    void Patrol()
	    {
		    transform.eulerAngles = new Vector2(0, 0);
	    }
	
        // Flash the sprite that this script is attached to
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