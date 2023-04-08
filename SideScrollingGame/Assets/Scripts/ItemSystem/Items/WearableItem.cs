using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Wearable")]
public class WearableItem : Item
{
    float appliedHealthScaler = 1f;
    bool damageMultiplierAdded = false;
    bool speedMultiplierAdded = false;
    bool protectionAdded = false;
    bool cooldownSpeedMultiplierAdded = false;

    int frameCountWhenPutOn;
    
    public override void OnPutOnByHero(HeroBrain wearer) {
        frameCountWhenPutOn = Time.frameCount;
        if (floatData.TryGetValue("healthScaler", out float healthScaler)) {
            wearer.health.ScaleHealth(healthScaler);
            appliedHealthScaler = healthScaler;
        }
        if (floatData.TryGetValue("damageMultiplier", out float damageMultiplier)) {
            wearer.damageData.multiplier.AddMultiplier(damageMultiplier, itemName + frameCountWhenPutOn.ToString());
            damageMultiplierAdded = true;
        }
        if (floatData.TryGetValue("speedMultiplier", out float speedMultiplier)) {
            wearer.speedData.multiplier.AddMultiplier(speedMultiplier, itemName + frameCountWhenPutOn.ToString());
            speedMultiplierAdded = true;
        }
        if (floatData.TryGetValue("protection", out float protection)) {
            wearer.protection.AddAddend(protection, itemName + frameCountWhenPutOn.ToString());
            protectionAdded = true;
        }
        if (floatData.TryGetValue("cooldownSpeedMultiplier", out float cooldownSpeedMultiplier)) {
            wearer.cooldownSpeedMultiplier.AddMultiplier(cooldownSpeedMultiplier, itemName + frameCountWhenPutOn.ToString());
            cooldownSpeedMultiplierAdded = true;
        }
    }

    public override void OnTakenOffFromHero(HeroBrain wearer) {
        if (appliedHealthScaler != 1f) {
            wearer.health.ScaleHealth(1f / appliedHealthScaler);
            appliedHealthScaler = 1f;
        }        
        if (damageMultiplierAdded) {
            wearer.damageData.multiplier.RemoveMultiplier(itemName + frameCountWhenPutOn.ToString());
            damageMultiplierAdded = false;
        }
        if (speedMultiplierAdded) {
            wearer.speedData.multiplier.RemoveMultiplier(itemName + frameCountWhenPutOn.ToString());
            speedMultiplierAdded = false;
        }
        if (protectionAdded) {
            wearer.protection.RemoveAddend(itemName + frameCountWhenPutOn.ToString());
            protectionAdded = false;
        }
        if (cooldownSpeedMultiplierAdded) {
            wearer.cooldownSpeedMultiplier.RemoveMultiplier(itemName + frameCountWhenPutOn.ToString());
            cooldownSpeedMultiplierAdded = false;
        }
    }
}
