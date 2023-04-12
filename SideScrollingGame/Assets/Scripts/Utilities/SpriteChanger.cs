using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpriteChanger : MonoBehaviour
{
    public SpriteRenderer tergetRenderer;
    public Sprite toSprite;
    public UnityEvent onChange;
    public bool isChanged;

    void Change() {
        if (!isChanged && tergetRenderer && toSprite) {
            tergetRenderer.sprite = toSprite;
            onChange.Invoke();
            isChanged = true;
        }
    }
}
