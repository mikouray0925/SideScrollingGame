using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageablePart : MonoBehaviour
{
    public Health health;
    [SerializeField] private Transform centerPoint;

    public static HashSet<Health> GetHealthComponents(Collider2D[] colliders) {
        HashSet<Health> healthSet = new HashSet<Health>();
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent(out DamageablePart damageable)) {
                if (damageable.enabled) healthSet.Add(damageable.health);
            }
        }
        return healthSet;
    }

    public Vector2 Center {
        get {
            if (centerPoint) {
                return centerPoint.position;
            } else {
                return transform.position;
            }
        }
        private set {}
    }
}
