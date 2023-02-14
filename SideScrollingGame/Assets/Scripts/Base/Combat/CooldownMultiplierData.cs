using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class CooldownMultiplierData : MonoBehaviour
{
    [Header ("Basic parameters")]
    [SerializeField] private float cdMultiplier;
    [SerializeField][SerializedDictionary("Tag", "Multiplier")] 
    private SerializedDictionary<string, float> cdMultipliers;

    public float CooldownMultiplier {
        get {
            return cdMultiplier;
        }
        private set {}
    }

    public bool AddCooldownMultiplier(float multiplier, string tag) {
        if (cdMultipliers.TryAdd(tag, multiplier)) {
            cdMultiplier *= multiplier;
            return true;
        } else {
            Debug.LogError("Try to add an CD multiplier with existed tag: " + tag);
            return false;
        }
        
    }

    public bool RemoveCooldownMultiplier(string tag) {
        if (cdMultipliers.Remove(tag, out float multiplier)) {
            cdMultiplier /= multiplier;
            return true;
        } else {
            Debug.LogError("Try to remove an CD multiplier with nonexistent tag: " + tag);
            return false;
        }
    }
}
