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

    public void LeftClickEvent() {
        if (displayingSlot == null) return;
        Inventory.ItemSlot.SwapItem(CursorItemSlot.instance.Slot, displayingSlot);
    }

    public void RightClickEvent() {
        if (displayingSlot == null) return;
        Item.LocalHeroUseItem(displayingSlot);
    }
}
