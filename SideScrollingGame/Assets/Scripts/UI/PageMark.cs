using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageMark : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] PageMarkGroup inGroup; 
    [SerializeField] public Page onPage;
    [SerializeField] Image img;

    public void ActivatePage() {
        if (inGroup) inGroup.CloseOtherPages(this);
        onPage.Activate();
        img.color = new Color(1, 1, 1, 1);
    }

    public void DeactivatePage() {
        onPage.Deactivate();
        img.color = new Color(0.7f, 0.7f, 0.7f, 1);
    }
}
