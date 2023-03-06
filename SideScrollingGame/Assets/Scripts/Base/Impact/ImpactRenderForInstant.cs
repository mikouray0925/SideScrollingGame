using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpactRenderForInstant : MonoBehaviour
{
   SpriteRenderer rend;
    
    void Awake() {
        rend = GetComponent<SpriteRenderer>();
        rend.enabled = false;
    }

    public void RenderAt(Vector2 pos, float duration = 1f) {
        if (!rend.enabled) {
            transform.position = pos;
            rend.enabled = true;
            Invoke(nameof(DisableRenderer), duration);
        }
    }

    void DisableRenderer() {
        rend.enabled = false;
    }
}
