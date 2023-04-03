using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelectionBoxRadio : MonoBehaviour
{
    [SerializeField] HeroSelectionBox[] boxes;

    public void UnselectOthers(HeroSelectionBox selected) {
        foreach (HeroSelectionBox box in boxes) {
            if (box != selected) box.ResetSelect();
        }
    }
}
