using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBrain : MonoBehaviour
{
    [Header ("Creature system")]
    public HeroHealth health;
    public HeroMovement movement;
    public HeroNormalAttack normalAttack;
    public HeroAbility1 ability1;
    public HeroAbility2 ability2;
    
    [Header ("Item system")]
    public Inventory inventory;
    public ItemPicker picker;
    public ItemDropper dropper;

    [Header ("Data")]
    public SpeedData speedData;
    public DamageData damageData;
    public CombinedAddend protection;
    public CombinedMultiplier cooldownSpeedMultiplier;

    [Header ("Sprites")]
    public Sprite ability1Icon;
    public Sprite ability2Icon;

    public void ReadSave(PlayerData playerData) {
        if (playerData != null) {
            BroadcastMessage("OnReadData", playerData, SendMessageOptions.DontRequireReceiver);
        }
    }

    public void WriteSave(PlayerData playerData) {
        if (playerData == null) playerData = new PlayerData();
        BroadcastMessage("OnWriteData", playerData, SendMessageOptions.DontRequireReceiver);
    }
}
