using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class DamageData : DataWithMultiplier
{
    [Header ("Parameters")]
    [SerializeField] private float damage;

    public float Damage {
        get {
            return damage * Multiplier;
        }
        private set {}
    }
}
