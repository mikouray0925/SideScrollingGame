using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class DamageData : MonoBehaviour
{
    [Header ("Basic parameters")]
    [SerializeField] private float damage;
    [SerializeField] private float damageMultiplier;
    [SerializeField][SerializedDictionary("Tag", "Multiplier")] 
    private SerializedDictionary<string, float> damageMultipliers;

    public float Damage {
        get {
            return damage * damageMultiplier;
        }
        private set {}
    }

    public float DamageMultiplier {
        get {
            return damageMultiplier;
        }
        private set {}
    }
    
    public void AddTempDamageMultiplier(float multiplier, float duration) {
        StartCoroutine(DamageMultiplierCoroutine(multiplier, duration));
    }

    private IEnumerator DamageMultiplierCoroutine(float multiplier, float duration) {
        damageMultiplier *= multiplier;
        yield return new WaitForSeconds(duration);
        damageMultiplier /= multiplier;
    }

    public bool AddDamageMultiplier(float multiplier, string tag) {
        if (damageMultipliers.TryAdd(tag, multiplier)) {
            damageMultiplier *= multiplier;
            return true;
        } else {
            Debug.LogError("Try to add an damage multiplier with existed tag: " + tag);
            return false;
        }
        
    }

    public bool RemoveDamageMultiplier(string tag) {
        if (damageMultipliers.Remove(tag, out float multiplier)) {
            damageMultiplier /= multiplier;
            return true;
        } else {
            Debug.LogError("Try to remove an damage multiplier with nonexistent tag: " + tag);
            return false;
        }
    }
}
