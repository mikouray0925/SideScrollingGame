using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterfaceUI : MonoBehaviour
{
    [SerializeField] UnityEvent onShow;
    [SerializeField] UnityEvent onHide;
    
    public void Show() {
        gameObject.SetActive(true);
        onShow.Invoke();
    }

    public void Hide() {
        gameObject.SetActive(false);
        onHide.Invoke();
    }
}
