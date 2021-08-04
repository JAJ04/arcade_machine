using UnityEngine;
using System.Collections;

public class HealthMove: MonoBehaviour
{
    // This will start the health sprite at one destination and will move it to another destination
    public Transform originSpot;
    public Transform destinationSpot;

    // Move the sprite at a certain speed
    public float speed;

    // This is a bool that allows the health kit to move in a different direction
    private bool switchBool = false;

    // Needs to be "FixedUpdate()" is Time.timeScale will pause this
    void FixedUpdate() 
    {
        if (transform.position == destinationSpot.position) 
	{
            switchBool = true;
        }
	
        if (transform.position == originSpot.position) 
	{
            switchBool = false;
        }

        if (switchBool) 
	{
            transform.position = Vector3.MoveTowards(transform.position, originSpot.position, speed);
        } 
	else 
	{
            transform.position = Vector3.MoveTowards(transform.position, destinationSpot.position, speed);
        }
    }
}
