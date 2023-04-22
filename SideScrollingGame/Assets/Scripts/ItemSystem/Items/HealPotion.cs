using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Item/Potion/HealPotion")]
public class HealPotion : Item {
    [Header ("Prefabs")]
    [SerializeField] GameObject healEffectPrefab;

    [Header ("SFX")]
    [SerializeField] AudioClip drinkPotionSFX;

    protected override void OnUsedByHero(HeroBrain user, out Item usedItem) {
        if (user.health.Hp < user.health.MaxHp) {
            user.health.Heal(floatData["healAmount"]);
            if (healEffectPrefab) {
                ImpactEffectSystem impact = GameManager.SpawnImpactEffect(healEffectPrefab);
                impact.transform.position = user.mainCollider.bounds.center;
            }
            if (drinkPotionSFX) {
                user.audioSource.PlayOneShot(drinkPotionSFX, AudioManager.EffectVolume);
            }
            usedItem = null;
        } else {
            usedItem = this;
        }
    }
}
