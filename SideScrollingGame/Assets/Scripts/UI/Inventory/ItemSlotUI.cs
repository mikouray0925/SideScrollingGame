using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image img;
    public Inventory.ItemSlot displayingSlot = null;

    private void LateUpdate() {
        if (displayingSlot != null) {
            if (displayingSlot.item != null) {
                img.enabled = true;
                img.sprite = displayingSlot.item.icon;
            } else {
                img.enabled = false;
            }
        }
        
    }
}
