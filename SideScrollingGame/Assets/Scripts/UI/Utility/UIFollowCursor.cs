using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIFollowCursor : MonoBehaviour
{
    [SerializeField] RectTransform rectTransform;
    
    void LateUpdate() {
        rectTransform.position = Mouse.current.position.ReadValue();
    }
}
