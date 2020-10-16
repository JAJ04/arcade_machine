using UnityEngine;
using System.Collections;

public class ParallaxBackground : MonoBehaviour
{
    // Move the background at a certain speed
	public float speed = 0.5f;
	
	// Use this for initialization
	void Start ()
    {
        // Change the sorting order of the sprite
		GetComponent<Renderer>().sortingOrder = 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Change the offset of the texture in accordance to the speed and time pushing
        // the speed forward
		Vector2 offset = new Vector2(Time.time * speed, 0);
		GetComponent<Renderer>().material.mainTextureOffset = offset;
	}
}