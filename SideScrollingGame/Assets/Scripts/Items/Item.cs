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

    public void BeUsedByHero(HeroBrain user) {
        if (usable) OnUsedByHero(user);
    }

    protected virtual void OnUsedByHero(HeroBrain user) {}
}
