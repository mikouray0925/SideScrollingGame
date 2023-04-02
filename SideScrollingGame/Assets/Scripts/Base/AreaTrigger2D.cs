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

    [Header ("With Collider")]
    [SerializeField] public UnityEvent<Collider2D> onEnter_collider;
    [SerializeField] public UnityEvent<Collider2D> onStay_collider;
    [SerializeField] public UnityEvent<Collider2D> onExit_collider;

    private void OnTriggerEnter2D(Collider2D other) {
        if (LayerUtil.Judge(other).IsInMask(layerMask)) {
            onEnter.Invoke();
            onEnter_collider.Invoke(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (LayerUtil.Judge(other).IsInMask(layerMask)) {
            onStay.Invoke();
            onStay_collider.Invoke(other);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other) {
        if (LayerUtil.Judge(other).IsInMask(layerMask)) {
            onExit.Invoke();
            onExit_collider.Invoke(other);
        }
    }
}
