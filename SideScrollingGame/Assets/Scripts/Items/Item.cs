using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Base")]
public class Item : ScriptableObject
{
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;
    public bool usable;

    public static void LocalHeroUseItem(Inventory.ItemSlot slot) {
        if (AppManager.instance.LocalHero != null) 
            HeroUseItem(AppManager.instance.LocalHero, slot);
    }

    public static void HeroUseItem(HeroBrain user, Inventory.ItemSlot slot) {
        if (!slot.Empty()) {
            Item item = slot.TakeOutItem();
            if (item.usable) {
                item.OnUsedByHero(user, out bool needToAddBackToSlot);
                if (needToAddBackToSlot) slot.Add(item);
            } else {
                slot.Add(item);
            }
        }
    }

    protected virtual void OnUsedByHero(HeroBrain user, out bool needToAddBackToSlot) {
        needToAddBackToSlot = true;
    }
}
