using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    public ItemDrop Drop(Item item) {
        return ItemDrop.SpawnItemDrop(item, transform.position);
    }   

    public ItemDrop Drop(Item item, Vector2 force, ForceMode2D forceMode = ForceMode2D.Force) {
        ItemDrop drop = ItemDrop.SpawnItemDrop(item, transform.position);
        drop.rb.AddForce(force, forceMode);
        return drop;
    }  

}
