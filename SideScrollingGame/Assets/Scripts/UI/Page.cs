using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Page : MonoBehaviour
{
    [SerializeField] UnityEvent onActivation;

    public void Activate() {
        gameObject.SetActive(true);
    }

    private void OnEnable() {
        onActivation.Invoke();
    }

    public void Deactivate() {
        gameObject.SetActive(false);
    }
}
