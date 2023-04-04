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
            if (!displayingSlot.Empty()) {
                img.enabled = true;
                img.sprite = displayingSlot.ItemIcon;
            } else {
                img.enabled = false;
            }
        }
    }

    private void LeftClickEvent() {
        if (displayingSlot == null) return;
        if (InventoryUI.instance.cursorSlot.Empty()) {
            if (!displayingSlot.Empty()) {
                InventoryUI.instance.cursorSlot.Add(displayingSlot.TakeOutItem());
            }
        } else {
            if (displayingSlot.Empty()) {
                displayingSlot.Add(InventoryUI.instance.cursorSlot.TakeOutItem());
            }
        }
    }
}
