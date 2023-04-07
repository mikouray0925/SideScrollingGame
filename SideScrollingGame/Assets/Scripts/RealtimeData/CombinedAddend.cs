using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

public class CombinedAddend : MonoBehaviour
{
    [Header ("Addend")]
    [SerializeField] private float currentAddend = 0f;
    [SerializeField][SerializedDictionary("Tag", "Addend")] 
    private SerializedDictionary<string, float> addendDict = new SerializedDictionary<string, float>();

    public float Addend {
        get {
            return currentAddend;
        }
        private set {}
    }

    public void AddTempAddend(float addend, float duration) {
        StartCoroutine(MultiplierCoroutine(addend, duration));
    }

    private IEnumerator MultiplierCoroutine(float addend, float duration) {
        currentAddend += addend;
        yield return new WaitForSeconds(duration);
        currentAddend -= addend;
    }

    public bool AddAddend(float addend, string tag) {
        if (addendDict.TryAdd(tag, addend)) {
            currentAddend += addend;
            return true;
        } else {
            Debug.LogWarning("Try to add an addend with existed tag: " + tag);
            return false;
        }
        
    }

    public bool RemoveAddend(string tag) {
        if (addendDict.Remove(tag, out float addend)) {
            currentAddend -= addend;
            return true;
        } else {
            Debug.LogWarning("Try to remove an addend with nonexistent tag: " + tag);
            return false;
        }
    }
}
