using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header ("Basic")]
    [SerializeField] public DamageData damageData;
    [SerializeField] public CooldownMultiplierData cdData;
    [SerializeField] private float attackCD;

    public float AttackCD {
        get {
            if (cdData) {
                return attackCD * cdData.Multiplier;
            } else {
                return attackCD;
            }
        }
        private set {}
    }

    protected int ApplyDamage(Overlap overlap, Vector2 direction, float damageMultiplier = 1f, float damageAddend = 0) {
        HashSet<Health> healthSet = overlap.GetOverlapHealthComponents();
        foreach (Health health in healthSet) {
            health.TakeDamage(damageData.Damage * damageMultiplier + damageAddend, direction);
        }
        return healthSet.Count;
    }
}
