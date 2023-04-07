using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Base")]
public class Item : ScriptableObject
{
    public enum Type {
        Unspecified,
        Potion,
        Helmet,
        ChestPlate,
        Leggings,
        Shoes,
        Ring,
        Necklace,
        Wristband
    }
    
    [Header ("Base")]
    public int id;
    public string itemName;
    public string description;
    public Sprite icon;
    public bool usable;
    public bool wearable;
    public Type type;

    [Header ("Data")]
    [SerializeField][SerializedDictionary("name", "bool")] 
    protected SerializedDictionary<string, bool> boolData = 
          new SerializedDictionary<string, bool>();

    [SerializeField][SerializedDictionary("name", "float")] 
    protected SerializedDictionary<string, float> floatData = 
          new SerializedDictionary<string, float>();

    [SerializeField][SerializedDictionary("name", "int")] 
    protected SerializedDictionary<string, int> intData = 
          new SerializedDictionary<string, int>();
    
    [SerializeField][SerializedDictionary("name", "string")] 
    protected SerializedDictionary<string, string> stringData = 
          new SerializedDictionary<string, string>();

    public static void LocalHeroUseItem(Inventory.ItemSlot slot) {
        if (AppManager.instance.LocalHero != null) 
            HeroUseItem(AppManager.instance.LocalHero, slot);
    }

    public static void HeroUseItem(HeroBrain user, Inventory.ItemSlot slot) {
        if (!slot.Empty()) {
            Item item = slot.TakeOutItem();
            if (item.usable) {
                item.OnUsedByHero(user, out Item usedItem);
                if (slot.Add(usedItem)) {

                } else {
                    
                }
            } else {
                slot.Add(item);
            }
        }
    }

    protected virtual void OnUsedByHero(HeroBrain user, out Item usedItem) {
        usedItem = this;
    }

    public virtual void OnPutOnByHero(HeroBrain wearer) {}

    public virtual void BeingWornByHeroUpdate(HeroBrain wearer, float deltaTime) {}

    public virtual void OnTakenOffFromHero(HeroBrain wearer) {}

    public static bool IsUsable(Item item) {
        return item != null && item.usable;
    }
}
