using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AreaTrigger2D : MonoBehaviour
{
    [Header ("Settings")]
    [SerializeField] public LayerMask layerMask;
    
    [Header ("No arguments")]
    [SerializeField] public UnityEvent onEnter;
    [SerializeField] public UnityEvent onStay;
    [SerializeField] public UnityEvent onExit;

    public delegate void ColliderOperation(Collider2D col);
    [SerializeField] public ColliderOperation onEnter_collider;
    [SerializeField] public ColliderOperation onStay_collider;
    [SerializeField] public ColliderOperation onExit_collider;

    private void OnTriggerEnter2D(Collider2D other) {
        if (LayerUtil.Judge(other).IsInMask(layerMask)) {
            onEnter.Invoke();
            if (onEnter_collider != null) onEnter_collider(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (LayerUtil.Judge(other).IsInMask(layerMask)) {
            onStay.Invoke();
            if (onStay_collider != null) onStay_collider(other);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (LayerUtil.Judge(other).IsInMask(layerMask)) {
            onExit.Invoke();
            if (onExit_collider != null) onExit_collider(other);
        }
    }
}
