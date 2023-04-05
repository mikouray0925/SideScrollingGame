using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPicker : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] ItemDropper dropper;
    [SerializeField] Overlap pickableArea;

    public bool PickNearestItem() {
        ItemDrop nearest = FindNearestItemDrop();
        if (nearest != null) {
            Item pickedItem = nearest.PickItem();
            if (!inventory.AddItemToScroller(pickedItem)) {
                dropper.Drop(pickedItem);
                return false;
            }
            return true;
        } else return false;
    }

    public ItemDrop FindNearestItemDrop() {
        Collider2D[] colliders = pickableArea.GetOverlapColliders();
        ItemDrop nearest = null;
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent<ItemDrop>(out ItemDrop itemDrop)) {
                if (nearest != null) {
                    if ((itemDrop.transform.position - transform.position).magnitude < 
                        ( nearest.transform.position - transform.position).magnitude) {
                        nearest = itemDrop;
                    }
                } else {
                    nearest = itemDrop;
                }
            }
        }
        return nearest;
    }
}
