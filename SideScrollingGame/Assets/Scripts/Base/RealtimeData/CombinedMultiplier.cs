using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class CombinedMultiplier : MonoBehaviour
{
    [Header ("Multiplier")]
    [SerializeField] private float currentMultiplier = 1f;
    // [SerializeField][SerializedDictionary("Tag", "Multiplier")] 
    private SerializedDictionary<string, float> multiplierDict;

    public float Multiplier {
        get {
            return currentMultiplier;
        }
        private set {}
    }

    public void AddTempMultiplier(float multiplier, float duration) {
        StartCoroutine(MultiplierCoroutine(multiplier, duration));
    }

    private IEnumerator MultiplierCoroutine(float multiplier, float duration) {
        currentMultiplier *= multiplier;
        yield return new WaitForSeconds(duration);
        currentMultiplier /= multiplier;
    }

    public bool AddMultiplier(float multiplier, string tag) {
        if (multiplierDict.TryAdd(tag, multiplier)) {
            currentMultiplier *= multiplier;
            return true;
        } else {
            Debug.LogError("Try to add a multiplier with existed tag: " + tag);
            return false;
        }
        
    }

    public bool RemoveMultiplier(string tag) {
        if (multiplierDict.Remove(tag, out float multiplier)) {
            currentMultiplier /= multiplier;
            return true;
        } else {
            Debug.LogError("Try to remove a multiplier with nonexistent tag: " + tag);
            return false;
        }
    }
}
