using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorItemSlot : MonoBehaviour
{
    [SerializeField] ItemSlotUI slotUI;
    
    public static CursorItemSlot instance;
    Inventory.ItemSlot slot = new Inventory.ItemSlot(-1);

    private void Awake() {
        slotUI.displayingSlot = slot;
        instance = this;
    }
    
    public Inventory.ItemSlot Slot {
        get {
            if (InventoryUI.instance) {    
                return slot;
            } else {
                Debug.LogError("The instance of CursorItemSlot is not set yet.");
                return null;
            }
        }
        private set {}
    }
}
