using UnityEngine;
using System.Collections;

public class PositionGUIWinFace : MonoBehaviour
{
     // Use this for initialization
     void Update ()
	 {
        // Keeps the DOOM face clamped to the screen despite the resolution
         Vector3 pos = Camera.main.WorldToViewportPoint (transform.position);
         pos.x = Mathf.Clamp(pos.x, 0.95f, 0.95f);
         pos.y = Mathf.Clamp(pos.y, 0.92f, 0.92f);
         transform.position = Camera.main.ViewportToWorldPoint(pos);
	 }
}