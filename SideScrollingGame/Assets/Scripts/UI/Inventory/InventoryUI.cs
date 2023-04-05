using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : Page
{
    [Header ("Item slot")]
    [SerializeField] GameObject slotPrefeb_Scroller;
    [SerializeField] Transform slotHolder_Scroller;
    
    public static InventoryUI instance {get; private set;}

    public Inventory bindingInventory {get; private set;} = null;

    private void Awake() {
        instance = this;
    }

    public void Bind(Inventory inventory) { 
        Unbind();

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
            // Unbind scroller side
            foreach (Transform child in slotHolder_Scroller) {
                Destroy(child.gameObject);
            }
        }
        bindingInventory = null;
    }
}
