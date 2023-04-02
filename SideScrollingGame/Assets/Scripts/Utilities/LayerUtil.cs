using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LayerUtil
{
    public static ObjInterface Operate(GameObject obj) {
        return new ObjInterface(obj);
    }

    public static ObjInterface Judge(GameObject obj) {
        return new ObjInterface(obj);
    }

    public static ObjInterface Operate(Collider2D col) {
        return new ObjInterface(col.gameObject);
    }

    public static ObjInterface Judge(Collider2D col) {
        return new ObjInterface(col.gameObject);
    }
    
    public class ObjInterface {
        private GameObject operatingObj;

        public ObjInterface(GameObject obj) {
            operatingObj = obj;
        }

        public bool IsInMask(LayerMask mask) {
            return ((1 << operatingObj.layer & mask) != 0);
        }
    }
}
