using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool<T> where T: MonoBehaviour
{
    Transform pool;
    GameObject objPrefab;
    Stack<T> availableObjects = new Stack<T>();
    public delegate void ObjOperation(T obj);
    public ObjOperation onSpawn;
    public ObjOperation onGet;
    public ObjOperation onRelease;

    public ObjPool(GameObject _objPrefab, Transform _pool, int initNum) {
        onSpawn += NullFunc;
        onGet += NullFunc;
        onRelease += NullFunc;

        objPrefab = _objPrefab;
        pool = _pool;
        AvailableNum = initNum;
    }

    public T Get() {
        T obj;
        if (!availableObjects.TryPop(out obj)) {
            if (Spawn()) {
                obj = availableObjects.Pop();
            } else {
                Debug.LogError("Spawn obj failed.");
                return null;
            }
        } 

        obj.gameObject.SetActive(true);
        onGet(obj);
        return obj;
    }

    public void Release(T obj) {
        onRelease(obj);
        obj.gameObject.SetActive(false);
        availableObjects.Push(obj);
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
            gameObj.SetActive(false);
            availableObjects.Push(newObj);
            return true;
        } else {
            Debug.LogError(typeof(T).ToString() + " cannot be found in " + gameObj.name);
            return false;
        }
    }

    void NullFunc(T obj) {}
}
