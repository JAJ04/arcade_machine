using UnityEngine;
using System.Collections;

public class PositionGUITopLeft : MonoBehaviour 
{
    // Use this for initialization
    void Update()
    {
        // Keeps the player data text clamped to the screen despite resolution
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.028 f, 0.028 f);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
