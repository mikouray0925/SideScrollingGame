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
}
