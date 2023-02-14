using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlapCircle : Overlap
{
    [SerializeField] public float radius;
    
    public override Collider2D[] GetOverlapColliders() {
        return Physics2D.OverlapCircleAll(transform.position, radius, targetLayers);
    }

    public override void OnDrawGizmosShape() { 
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
