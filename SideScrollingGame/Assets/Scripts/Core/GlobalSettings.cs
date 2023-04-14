using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[CreateAssetMenu(fileName = "NewGlobalSetting", menuName = "GlobalSetting")]
public class GlobalSettings : ScriptableObject
{
    public static LayerMask groundLayers {get; private set;}
    public static LayerMask obstacleLayers {get; private set;}
    public static LayerMask creatureLayers {get; private set;}
    public static LayerMask playerLayers {get; private set;}
    public static LayerMask enemyLayers {get; private set;}
    public static LayerMask itemDropLayers {get; private set;}
    public static LayerMask projectileLayers {get; private set;}
    [Header ("LayerMasks")]
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private LayerMask _creatureLayers;
    [SerializeField] private LayerMask _playerLayers;
    [SerializeField] private LayerMask _enemyLayers;
    [SerializeField] private LayerMask _itemDropLayers;
    [SerializeField] private LayerMask _projectileLayers;

    public static GameObject itemDropPrefab {get; private set;}
    public static Dictionary<string, GameObject> heroDict {get; private set;}
    [Header ("Prefabs")]
    [SerializeField] private GameObject _itemDropPrefab;
    [SerializeField][SerializedDictionary("name", "prefab")]
    public SerializedDictionary<string, GameObject> registeredHeros = new SerializedDictionary<string, GameObject>();

    public static Dictionary<int, Item> itemDict {get; private set;} = new Dictionary<int, Item>();
    [Header ("Items")]
    [SerializeField] private List<Item> registeredItems;

    public void SetThis() {
        groundLayers = _groundLayers;
        obstacleLayers = _obstacleLayers;
        creatureLayers = _creatureLayers;
        playerLayers = _playerLayers;
        enemyLayers = _enemyLayers;
        itemDropLayers = _itemDropLayers;
        projectileLayers = _projectileLayers;

        itemDropPrefab = _itemDropPrefab;

        heroDict = new Dictionary<string, GameObject>();
        foreach (KeyValuePair<string, GameObject> pair in registeredHeros) {
            heroDict.Add(pair.Key, pair.Value);
        }

        foreach (Item item in registeredItems) {
            if (!itemDict.TryAdd(item.id, item)) {
                Debug.LogError($"Item id{item.id} conflicts between {item.name} and {itemDict[item.id].name}");
            }
        }
    }
}
