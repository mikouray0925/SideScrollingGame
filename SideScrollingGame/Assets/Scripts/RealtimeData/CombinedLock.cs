using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombinedLock : MonoBehaviour
{
    [SerializeField] 
    private HashSet<string> lockTags = new HashSet<string>();

    public bool IsLocked {
        get {
            return lockTags.Count > 0;
        }
        private set {}
    }

    public bool AddLock(string tag) {
        if (lockTags.Add(tag)) {
            return true;
        } else {
            Debug.LogWarning("Try to add a lock with existed tag: " + tag);
            return false;
        }
    }

    public bool AddLock(string tag, float duration) {
        if (lockTags.Add(tag)) {
            StartCoroutine(RemoveLockAfterSec(tag, duration));
            return true;
        } else {
            Debug.LogWarning("Try to add a lock with existed tag: " + tag);
            return false;
        }
    }

    public bool RemoveLock(string tag) {
        if (lockTags.Remove(tag)) {
            return true;
        } else {
            // Debug.LogWarning("Try to remove a lock with nonexistent tag: " + tag);
            return false;
        }
    }

    private IEnumerator RemoveLockAfterSec(string tag, float duration) {
        yield return new WaitForSeconds(duration);
        RemoveLock(tag);
    }
}
