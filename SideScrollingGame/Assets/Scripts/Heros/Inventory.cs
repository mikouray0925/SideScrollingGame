using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Range (6, 42)]
    [SerializeField] int initSize = 33;
    [Range (0, 42)]
    [SerializeField] int fixedSlotNum;
    public List<ItemSlot> slots;

    private void Awake() {
        slots = new List<ItemSlot>(initSize);
        for (int i = 0; i < initSize; i++) {
            slots.Add(new ItemSlot(i));
        }
        InventoryUI.instance.Bind(this);
    }

    public int ScrollerStartIndex {
        get {
            return fixedSlotNum;
        }
        private set {}
    }

    public class ItemSlot {
        public int index {get; private set;}
        public Item item {get; private set;} = null;
        public delegate void ItemOperation(Item i);
        public ItemOperation onItemEnter;
        public ItemOperation onItemLeave;

        public ItemSlot(int _index) {
            index = _index;
        }

        public bool Empty() {
            return item == null;
        }

        public Item TakeItem() {
            if (item != null) {
                Item _item = item;
                item = null;
                onItemLeave(item);
                return _item;
            } else {
                return null;
            }
        }

        public bool Add(Item _item) {
            if (item == null) {
               item = _item;
               onItemEnter(item);
               return true;
           } else {
                return false;
            }
        }
    }
}
