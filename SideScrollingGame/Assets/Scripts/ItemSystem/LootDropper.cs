using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class LootDropper : MonoBehaviour
{
    [Serializable]
    public struct LootData {
        public Item item;
        [Range (0f, 1f)]
        [SerializeField] public float chance;
    }
    public LootData[] possibleLoots;
    public bool normalizeOnAwake = true;
    public int lootNum = 1;
    public float upForce;
    public float honrizontalRange;
    public float dropPeriod;

    private void Awake() {
        if (normalizeOnAwake) NormalizeLootChances();
    }

    public void StartRandomDrop() {
        StartCoroutine(RandomDropCoroutine());
    }
    
    private IEnumerator RandomDropCoroutine() {
        for (int i = 0; i < lootNum; i++) {
            Vector3 offset = UnityEngine.Random.Range(-honrizontalRange, honrizontalRange) * Vector3.right;
            Item pickedItem = Pick();
            if (pickedItem != null) {
                ItemDrop drop = ItemDrop.SpawnItemDrop(pickedItem, transform.position + offset);
                drop.rb.AddForce(upForce * Vector2.up, ForceMode2D.Impulse);
                yield return new WaitForSeconds(dropPeriod);
            } 
        }
    }
    
    public Item Pick() {
        if (possibleLoots.Length == 0) return null; 

        float multiplier = 1f;
        foreach (LootData loot in possibleLoots) {
            if (UnityEngine.Random.Range(0f, 1f) < (loot.chance * multiplier)) {
                return loot.item;
            } else {
                multiplier /= (1f - loot.chance);
            }
        }

        NormalizeLootChances();

        multiplier = 1f;
        foreach (LootData loot in possibleLoots) {
            if (UnityEngine.Random.Range(0f, 1f) < (loot.chance * multiplier)) {
                return loot.item;
            } else {
                multiplier /= loot.chance;
            }
        }
        return null;
    }

    public void NormalizeLootChances() {
        float sumOfChance = 0;
        foreach (LootData loot in possibleLoots) {
            sumOfChance += loot.chance;
        }
        float multiplier = 1f / sumOfChance;
        for (int i = 0; i < possibleLoots.Length; i++) {
            possibleLoots[i].chance *= multiplier;
        }
    }
}
