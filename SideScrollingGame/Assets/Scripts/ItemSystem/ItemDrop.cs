using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] Item item;
    [SerializeField] Image img;
    public Rigidbody2D rb {get; private set;}

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    public static ItemDrop SpawnItemDrop(Item _item, Vector3 pos) {
        GameObject dropObj = Instantiate(
            GlobalSettings.itemDropPrefab, 
            pos, Quaternion.identity,
            AppManager.instance.currentGame.itemDropHolder
        );
        ItemDrop drop = dropObj.GetComponent<ItemDrop>();
        drop.item = _item;
        drop.img.sprite = _item.icon;
        return drop;
    }

    public bool SetItem(Item _item) {
        if (item == null) {
            item = _item;
            return true;
        } else {
            return false;
        }
    }

    public Item PickItem() {
        Invoke(nameof(DestroyThisObj), 0.05f);
        return item;
    }

    private void FixedUpdate() {
        if (item) {
            img.sprite = item.icon;
        }
    }

    private void DestroyThisObj() {
        Destroy(gameObject);
    }
}
