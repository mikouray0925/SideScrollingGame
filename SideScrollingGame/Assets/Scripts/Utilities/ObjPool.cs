using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool<T> where T: MonoBehaviour
{
    Transform pool;
    GameObject objPrefab;
    List<T> availableObjects = new List<T>();

    public delegate void ObjOperation(T obj);
    public ObjOperation onSpawn;
    public ObjOperation onGet;
    public ObjOperation onRecycle;
    
    public delegate bool ObjJudgement(T obj);
    public ObjJudgement isAvailable;

    public HashSet<T> notYetRecycled = new HashSet<T>();
    private bool keepTrackOfObj = true;
    public bool KeepTrackOfObj {
        get {
            return keepTrackOfObj;
        }
        set {
            if (value) {
                keepTrackOfObj = true;
            } else {
                if (notYetRecycled.Count == 0) {
                    keepTrackOfObj = false;
                } else {
                    Debug.LogError("Cannot turn off tracking obj when there are obj not yet recycled.");
                }
            }
        }
    }

    public ObjPool(GameObject _objPrefab, Transform _pool, int initNum) {
        onSpawn += Deactivate;
        onGet += Activate;
        onRecycle += Deactivate;
        isAvailable = IsAvailableDefault;

        objPrefab = _objPrefab;
        pool = _pool;
        AvailableNum = initNum;
    }

    public T Get() {
        T result = null;
        Queue<T> unavailableObj = new Queue<T>();
        while (availableObjects.Count > 0) {
            T obj = availableObjects[0];
            availableObjects.RemoveAt(0);
            if (isAvailable(obj)) {
                result = obj;
                break;
            } else {
                unavailableObj.Enqueue(obj);
            }
        }
        while (unavailableObj.Count > 0) {
            availableObjects.Add(unavailableObj.Dequeue());
        }

        if (!result) {
            if (Spawn()) {
                result = availableObjects[0];
                availableObjects.RemoveAt(0);
            } else {
                Debug.LogError("Spawn obj failed.");
            }
        }

        if (result) {
            if (keepTrackOfObj) notYetRecycled.Add(result);
            onGet(result);
        }
        return result;
    }

    public void Recycle(T obj) {
        if (keepTrackOfObj) {
            if (notYetRecycled.Remove(obj)) {
                onRecycle(obj);
                availableObjects.Insert(0, obj);
            }
        } else {
            onRecycle(obj);
            availableObjects.Insert(0, obj);
        }
        
    }

    public IEnumerator RecycleAfterSeconds(T obj, float waitTime) {
        yield return new WaitForSeconds(waitTime);
        Recycle(obj);
    }

    public int AvailableNum {
        get {
            return availableObjects.Count;
        }
        set {
            Spawn(value - availableObjects.Count);
        }
    }

    private void Spawn(int num) {
        for (int i = 0; i < num; i++) {
            Spawn();
        }
    }  

    private bool Spawn() {
        GameObject gameObj;
        if (pool) {
            gameObj = GameObject.Instantiate(objPrefab, pool);
        } else {
            Debug.LogError("Object pool is not set.");
            return false;
        }
        
        if (gameObj.TryGetComponent<T>(out T newObj)) {
            onSpawn(newObj);
            availableObjects.Insert(0, newObj);
            return true;
        } else {
            Debug.LogError(typeof(T).ToString() + " cannot be found in " + gameObj.name);
            return false;
        }
    }

    public static bool IsAvailableDefault(T obj) {
        if (obj) {
            return !obj.gameObject.activeSelf;
        } else {
            return false;
        }
    }

    public static void Activate(T obj) {
        obj.gameObject.SetActive(true);
    }

    public static void Deactivate(T obj) {
        obj.gameObject.SetActive(false);
    }
}
