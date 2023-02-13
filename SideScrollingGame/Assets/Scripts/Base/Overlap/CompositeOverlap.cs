using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompositeOverlap : Overlap
{
    [SerializeField] Overlap[] overlaps;

    public override Collider2D[] GetOverlapColliders() {
        HashSet<Collider2D> colliderSet = new HashSet<Collider2D>();
        foreach (Overlap overlap in overlaps) {
            foreach (Collider2D collider in overlap.GetOverlapColliders()) {
                colliderSet.Add(collider);
            }
        }
        Collider2D[] colliders = new Collider2D[colliderSet.Count];
        colliderSet.CopyTo(colliders);
        return colliders;
    }

    public override void OnDrawGizmosShape() { 
        foreach (Overlap overlap in overlaps) {
            overlap.OnDrawGizmosShape();
        }
    }
}
