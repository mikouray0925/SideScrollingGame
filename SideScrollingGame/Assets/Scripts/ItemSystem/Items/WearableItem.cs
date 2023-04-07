using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Wearable")]
public class WearableItem : Item
{
    bool damageMultiplierAdded = false;
    bool speedMultiplierAdded = false;
    bool protectionAdded = false;
    bool cooldownSpeedMultiplierAdded = false;
    
    public override void OnPutOnByHero(HeroBrain wearer) {
        if (floatData.TryGetValue("damageMultiplier", out float damageMultiplier)) {
            wearer.damageData.multiplier.AddMultiplier(damageMultiplier, itemName);
            damageMultiplierAdded = true;
        }
        if (floatData.TryGetValue("speedMultiplier", out float speedMultiplier)) {
            wearer.speedData.multiplier.AddMultiplier(speedMultiplier, itemName);
            speedMultiplierAdded = true;
        }
        if (floatData.TryGetValue("protection", out float protection)) {
            wearer.protection.AddAddend(protection, itemName);
            protectionAdded = true;
        }
        if (floatData.TryGetValue("cooldownSpeedMultiplier", out float cooldownSpeedMultiplier)) {
            wearer.cooldownSpeedMultiplier.AddMultiplier(cooldownSpeedMultiplier, itemName);
            cooldownSpeedMultiplierAdded = true;
        }
    }

    public override void OnTakenOffFromHero(HeroBrain wearer) {
        if (damageMultiplierAdded) {
            wearer.damageData.multiplier.RemoveMultiplier(itemName);
            damageMultiplierAdded = false;
        }
        if (speedMultiplierAdded) {
            wearer.speedData.multiplier.RemoveMultiplier(itemName);
            speedMultiplierAdded = false;
        }
        if (protectionAdded) {
            wearer.protection.RemoveAddend(itemName);
            protectionAdded = false;
        }
        if (cooldownSpeedMultiplierAdded) {
            wearer.cooldownSpeedMultiplier.RemoveMultiplier(itemName);
            cooldownSpeedMultiplierAdded = false;
        }
    }
}
