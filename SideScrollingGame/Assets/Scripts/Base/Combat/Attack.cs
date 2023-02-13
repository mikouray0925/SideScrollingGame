using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header ("Basic")]
    [SerializeField] public AttackData attackData;
    [SerializeField] private float attackCD;

    public float AttackCD {
        get {
            if (attackData) {
                return attackCD * attackData.CooldownMultiplier;
            } else {
                return attackCD;
            }
        }
        private set {}
    }

    protected int ApplyDamage(Overlap overlap, Vector2 direction, float damageMultiplier = 1f) {
        HashSet<Health> healthSet = overlap.GetOverlapHealthComponents();
        foreach (Health health in healthSet) {
            health.TakeDamage(attackData.Damage * damageMultiplier, direction);
        }
        return healthSet.Count;
    }
}
