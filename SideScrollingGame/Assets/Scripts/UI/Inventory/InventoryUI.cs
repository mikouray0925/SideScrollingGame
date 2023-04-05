using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Page
{
    [SerializeField] ItemSlotUI[] wearableItemSlots = new ItemSlotUI[10];
    [SerializeField] ItemSlotUI[] quickSlots = new ItemSlotUI[4];
    
    [Header ("Scroller")]
    [SerializeField] GameObject slotPrefeb_Scroller;
    [SerializeField] Transform slotHolder_Scroller;
    
    public static InventoryUI instance {get; private set;}

    public Inventory bindingInventory {get; private set;} = null;

    private void Awake() {
        instance = this;
    }

    public void Bind(Inventory inventory) { 
        Unbind();

        for (int i = 0; i < wearableItemSlots.Length && i < inventory.wearableItems.Length; i++) {
            wearableItemSlots[i].displayingSlot = inventory.slots[i];
        }

        for (int i = 0; i < quickSlots.Length && i < inventory.QuickSlotNum; i++) {
            quickSlots[i].displayingSlot = inventory.slots[inventory.QuickSlotsStartIndex + i];
        }

        // Bind scroller side
        for (int i = inventory.ScrollerStartIndex; i < inventory.slots.Count; i++) {
            GameObject slotObj = Instantiate(slotPrefeb_Scroller, slotHolder_Scroller);
            ItemSlotUI slotUI = slotObj.GetComponent<ItemSlotUI>();
            slotUI.displayingSlot = inventory.slots[i];
        }

        bindingInventory = inventory;
    }

    public void Unbind() {
        if (bindingInventory != null) {
            for (int i = 0; i < wearableItemSlots.Length; i++) {
                wearableItemSlots[i].displayingSlot = null;
            }

            for (int i = 0; i < quickSlots.Length; i++) {
                quickSlots[i].displayingSlot = null;
            }

            // Unbind scroller side
            foreach (Transform child in slotHolder_Scroller) {
                Destroy(child.gameObject);
            }
        }
        bindingInventory = null;
    }

    public void DropCursorItem() {
        if (CursorItemSlot.instance.Slot.Empty()) return;
        if (AppManager.instance.LocalHero &&
            AppManager.instance.LocalHero.inventory == bindingInventory) {
            AppManager.instance.LocalHero.dropper.Drop(CursorItemSlot.instance.Slot.TakeOutItem());
        }
    }
}
