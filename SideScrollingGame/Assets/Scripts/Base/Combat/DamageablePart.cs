using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageablePart : MonoBehaviour
{
    public Health health;
    [SerializeField] private Transform centerPoint;

    public static List<DamageablePart> GetDamageableParts(Collider2D[] colliders) {
        List<DamageablePart> damageableList = new List<DamageablePart>();
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent(out DamageablePart damageable)) {
                if (damageable.enabled) damageableList.Add(damageable);
            }
        }
        return damageableList;
    }

    public static HashSet<Health> GetHealthComponents(List<DamageablePart> damageableList) {
        HashSet<Health> healthSet = new HashSet<Health>();
        foreach (DamageablePart damageable in damageableList) {
            if (damageable.enabled) healthSet.Add(damageable.health);
        }
        return healthSet;
    }

    public static HashSet<Health> GetHealthComponents(Collider2D[] colliders) {
        return GetHealthComponents(GetDamageableParts(colliders));

        /* Maybe this is better
        HashSet<Health> healthSet = new HashSet<Health>();
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent(out DamageablePart damageable)) {
                if (damageable.enabled) healthSet.Add(damageable.health);
            }
        }
        return healthSet;
        */
    }

    public Vector2 Center {
        get {
            if (centerPoint) {
                return centerPoint.position;
            } else {
                return transform.position;
            }
        }
        set {
            if (centerPoint) {
                Vector3 offset = transform.position - centerPoint.position;
                // transform.position = centerPoint.position + offset
                Vector3 newCenterPos = value;
                transform.position = newCenterPos + offset;
            } else {
                transform.position = value;
            }
        }
    }
}
