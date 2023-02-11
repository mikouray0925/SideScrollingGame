using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageablePart : MonoBehaviour
{
    public Health health;

    public static HashSet<Health> GetHealthComponents(Collider[] colliders) {
        HashSet<Health> healthSet = new HashSet<Health>();
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent(out DamageablePart damageable)) {
                healthSet.Add(damageable.health);
            }
        }
        return healthSet;
    }
}
