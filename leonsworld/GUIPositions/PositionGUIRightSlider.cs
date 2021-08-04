using UnityEngine;
using System.Collections;

public class PositionGUIRightSlider: MonoBehaviour
{
    // Use this for initialization
    void Update() 
    {
        // This keeps the bullet slider clamped on the bottom right at all times
        // No matter the resolution (scrapped but kept as a spare)
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x, 0.81 f, 0.81 f);
        pos.y = Mathf.Clamp(pos.y, 0.035 f, 0.035 f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
