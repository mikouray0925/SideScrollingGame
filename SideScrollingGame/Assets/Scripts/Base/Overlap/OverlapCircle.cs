using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircle : Overlap
{
    [SerializeField] float radius;
    
    override public Collider2D[] GetOverlapColliders() {
        return Physics2D.OverlapCircleAll(transform.position, radius, targetLayers);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = gizmosColor;    
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
