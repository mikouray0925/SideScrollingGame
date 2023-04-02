using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterfaceUI : MonoBehaviour
{
    [Header ("Base Events")]
    public UnityEvent onShow;
    public UnityEvent onHide;

    public bool IsActive {
        get {
            return gameObject.activeSelf;
        }
        set {
            if (!IsActive && value) {
                Show();
            }
            if (IsActive && !value) {
                Hide();
            }
        }
    }
    
    public void Show() {
        gameObject.SetActive(true);
        onShow.Invoke();
    }

    public void Hide() {
        gameObject.SetActive(false);
        onHide.Invoke();
    }
}
