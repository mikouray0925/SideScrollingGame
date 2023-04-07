using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlotUI : MonoBehaviour
{
    [SerializeField] Image iconImg;
    [SerializeField] Image slotImg;
    [SerializeField] bool hideSlotImgIfNotEmpty;
    public Inventory.ItemSlot displayingSlot = null;

    private void LateUpdate() {
        if (displayingSlot != null) {
            if (!displayingSlot.Empty()) {
                iconImg.enabled = true;
                iconImg.sprite = displayingSlot.ItemIcon;

                if (slotImg && hideSlotImgIfNotEmpty) slotImg.enabled = false;
                else if (slotImg) slotImg.enabled = true;
            } else {
                iconImg.enabled = false;
                
                if (slotImg && hideSlotImgIfNotEmpty) slotImg.enabled = true;
                else if (slotImg) slotImg.enabled = true;
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
