using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RawImgScroller : MonoBehaviour
{
    public RawImage targetImg;
    public Vector2 velocity;

    private void Update() {
        if (targetImg) {
            Rect rect = targetImg.uvRect;
            rect.x += velocity.x * Time.deltaTime;
            rect.y += velocity.y * Time.deltaTime;
            targetImg.uvRect = rect;
        }
    }

}
