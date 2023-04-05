using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Potion/HealPotion")]
public class HealPotion : Item {
    protected override void OnUsedByHero(HeroBrain user, out Item usedItem) {
        if (user.health.Hp < user.health.MaxHp) {
            user.health.Heal(floatData["healAmount"]);
            usedItem = null;
        } else {
            usedItem = this;
        }
    }
}
