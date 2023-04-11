using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] HeroBrain onHero;
    [Range (0, 50)]
    [SerializeField] int initSize = 41;
    public Item.Type[] wearableItems = new Item.Type[10];
    [Range (0, 50)]
    [SerializeField] int quickSlotNum = 4;
    public List<ItemSlot> slots;

    private void Awake() {
        slots = new List<ItemSlot>(initSize);

        for (int i = 0; i < wearableItems.Length; i++) {
            slots.Add(new WearableItemSlot(i, wearableItems[i], onHero));
        }
        for (int i = QuickSlotsStartIndex; i < QuickSlotsStartIndex + quickSlotNum; i++) {
            ItemSlot newSlot = new ItemSlot(i);
            newSlot.isValidItem = Item.IsUsable;
            slots.Add(newSlot);
        }
        for (int i = slots.Count; i < initSize; i++) {
            slots.Add(new ItemSlot(i));
        }

        InventoryUI.instance.Bind(this);
    }

    private void Update() {
        for (int i = 0; i < wearableItems.Length; i++) {
            slots[i].UpdateWearingItem(Time.deltaTime);
        }
    }

    public int QuickSlotsStartIndex {
        get {
            return wearableItems.Length;
        }
        private set {}
    }

    public int QuickSlotNum {
        get {
            return quickSlotNum;
        }
        private set {}
    }

    public int ScrollerStartIndex {
        get {
            return wearableItems.Length + quickSlotNum;
        }
        private set {}
    }

    public bool AddItemToScroller(Item item) {
        for (int i = ScrollerStartIndex; i < slots.Count; i++) {
            if (slots[i].Add(item)) return true;
        }
        return false;
    }

    public void UseQuickSlotItem(int index) {
        if (index < 0 || index >= QuickSlotNum) return;
        if (onHero && !slots[QuickSlotsStartIndex + index].Empty()) {
            Item.HeroUseItem(onHero, slots[QuickSlotsStartIndex + index]);
        }
    }

    private void OnDestroy() {
        if (InventoryUI.instance.bindingInventory == this) {
            InventoryUI.instance.Unbind();
        }
    }

    public class ItemSlot {
        public int index {get; private set;}
        protected Item item = null;
        public delegate void ItemOperation(Item i);
        public ItemOperation onItemEnter;
        public ItemOperation onItemLeave;
        public delegate bool ItemJudgement(Item i);
        public ItemJudgement isValidItem;

        public ItemSlot(int _index) {
            index = _index;
        }

        public Sprite ItemIcon {
            get {
                if (item != null) {
                    return item.icon;
                }
                return null;
            }
            private set {}
        }

        public Item ItemCopy {
            get {
                if (item != null) {
                    return Instantiate(item);
                }
                return null;
            }
            private set {}
        }

        public bool Empty() {
            return item == null;
        }

        public Item TakeOutItem() {
            if (item != null) {
                Item _item = item;
                item = null;
                if (onItemLeave != null) onItemLeave(_item);
                return _item;
            } else {
                return null;
            }
        }

        public bool AbleToAdd(Item _item) {
            if (isValidItem != null) {
                return isValidItem(_item);
            } else {
                return true;
            }
        }

        public bool Add(Item _item) {
            if (_item == null) return false;
            if (item == null) {
                if (isValidItem != null && !isValidItem(_item)) {
                    return false;
                } 
                item = _item;
                if (onItemEnter != null) onItemEnter(item);
                return true;
            } else {
                return false;
            }
        }

        public virtual void UpdateWearingItem(float deltaTime) {}

        public static bool MoveItem(ItemSlot src, ItemSlot dest) {
            if (src.Empty() || !dest.Empty()) return false;
            if (dest.AbleToAdd(src.item)) {
                dest.Add(src.TakeOutItem());
                return true;
            } else {
                return false;
            }
        }

        public static bool SwapItem(ItemSlot slot1, ItemSlot slot2) {
            if ( slot1.Empty() &&  slot2.Empty()) return true;
            if (!slot1.Empty() && !slot2.AbleToAdd(slot1.item)) return false;
            if (!slot2.Empty() && !slot1.AbleToAdd(slot2.item)) return false;
            Item toSlot1 = slot2.TakeOutItem();
            Item toSlot2 = slot1.TakeOutItem();
            slot1.Add(toSlot1);
            slot2.Add(toSlot2);
            return true;
        }
    }

    public class WearableItemSlot : ItemSlot {
        Item.Type type;
        HeroBrain onHero;

        public WearableItemSlot(int _index, Item.Type _type, HeroBrain _onHero) : base(_index) {
            type = _type;
            onHero = _onHero;
            isValidItem = IsWearableAndRightType;
            onItemEnter += CallItemPutOnEvent;
            onItemLeave += CallItemTakenOffEvent;
        }

        public bool IsWearableAndRightType(Item item) {
            return item.wearable && item.type == type;
        }

        private void CallItemPutOnEvent(Item item) {
            item.OnPutOnByHero(onHero);
        }

        private void CallItemTakenOffEvent(Item item) {
            item.OnTakenOffFromHero(onHero);
        }

        public override void UpdateWearingItem(float deltaTime) {
            if (Empty()) return;
            item.BeingWornByHeroUpdate(onHero, deltaTime);
        }
    }
}
