using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PageMarkGroup : MonoBehaviour
{
    public PageMark[] pageMarks;

    public void CloseOtherPages(PageMark markOnCurrent) {
        foreach(PageMark mark in pageMarks) {
            if (mark != markOnCurrent) {
                mark.DeactivatePage();
            } 
        }
    }
}
