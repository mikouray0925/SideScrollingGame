using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjPool<T> where T: MonoBehaviour
{
    GameObject objPrefab;
    Stack<T> availableObjects;
    public delegate void ObjOperation(T obj);
    public ObjOperation onSpawn;
    public ObjOperation onGet;
    public ObjOperation onRelease;

    public ObjPool(GameObject _objPrefab, int initNum) {
        objPrefab = _objPrefab;
        AvailableNum = initNum;
    }

    T Get() {
        T obj;
        if (!availableObjects.TryPop(out obj)) {
            if (Spawn()) {
                obj = availableObjects.Pop();
            } else {
                Debug.LogError("Get obj failed.");
                return null;
            }
        } 

        obj.gameObject.SetActive(true);
        onGet(obj);
        return obj;
    }

    void Release(T obj) {
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
        if (GameManager.objectPool) {
            gameObj = GameObject.Instantiate(objPrefab, GameManager.objectPool);
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
}
