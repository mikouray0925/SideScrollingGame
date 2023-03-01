using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class DamageData : MonoBehaviour
{
    [Header ("Parameters")]
    [SerializeField] private float damage;

    [Header ("Value Adjuster")]
    [SerializeField] private CombinedMultiplier multiplier;

    public float Damage {
        get {
            if (multiplier) {
                return damage * multiplier.Multiplier;
            } else {
                return damage;
            }
        }
        private set {}
    }
}
