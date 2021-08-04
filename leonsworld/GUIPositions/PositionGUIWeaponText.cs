using UnityEngine;
using System.Collections;

public class PositionGUIWeaponText : MonoBehaviour 
{
    // Use this for initialization
    void Update() 
    {
        // Keeps the weapon selected text clamped to the screen despite resolution
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.49 f, 0.49 f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
