using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageMarkGroup : MonoBehaviour
{
    public void CloseOtherPages(PageMark except) {
        foreach (Transform child in transform) {
            if (child.TryGetComponent<PageMark>(out PageMark pageMark) &&
                pageMark != except) {
                pageMark.HidePage();
            }
        }
    }
}
