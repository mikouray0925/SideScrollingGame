using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickableObject : MonoBehaviour, IPointerClickHandler
{
    [Header ("Base Events")]
    public UnityEvent onLeftClick;
    public UnityEvent onMidClick;
    public UnityEvent onRightClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) {
            onLeftClick.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle) {
            onMidClick.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right) {
            onRightClick.Invoke();
        }
    }
}
