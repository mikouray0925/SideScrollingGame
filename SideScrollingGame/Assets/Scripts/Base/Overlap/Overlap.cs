using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overlap : MonoBehaviour
{
    [Header ("gizmos")]
    [SerializeField] protected bool drawGizmos;
    [SerializeField] protected Color gizmosColor;
    
    [Header ("Base parameters")]
    [SerializeField] protected LayerMask targetLayers;

    public virtual Collider2D[] GetOverlapColliders() {
        return new Collider2D[0];
    }

    public List<DamageablePart> GetOverlapDamageableParts() {
        return DamageablePart.GetDamageableParts(GetOverlapColliders());
    }

    public HashSet<Health> GetOverlapHealthComponents() {
        return DamageablePart.GetHealthComponents(GetOverlapColliders());
    }

    private void OnDrawGizmos() {
        if (!drawGizmos) return;
        Gizmos.color = gizmosColor; 
        OnDrawGizmosShape();
    }

    public virtual void OnDrawGizmosShape() {}
}
