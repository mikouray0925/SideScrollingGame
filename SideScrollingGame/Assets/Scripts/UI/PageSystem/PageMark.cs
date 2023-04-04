using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageMark : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] public Page onPage;
    [SerializeField] Image img;

    public void ShowPage() {
        if (transform.parent && 
            transform.parent.TryGetComponent<PageMarkGroup>(out PageMarkGroup group)) {
            group.CloseOtherPages(this);
        }
        onPage.Show();
        img.color = new Color(1, 1, 1, 1);
    }

    public void HidePage() {
        onPage.Hide();
        img.color = new Color(0.7f, 0.7f, 0.7f, 1);
    }
}
