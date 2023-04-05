using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorItemSlot : MonoBehaviour
{
    public static CursorItemSlot instance;
    Inventory.ItemSlot slot = new Inventory.ItemSlot(-1);

    private void Awake() {
        instance = this;
    }
    
    public Inventory.ItemSlot Slot {
        get {
            if (InventoryUI.instance && InventoryUI.instance.IsActive) {    
                return slot;
            } else {
                Debug.LogError("Cannot access CursorItemSlot when InventoryUI is not active.");
                return null;
            }
        }
        private set {}
    }
}
