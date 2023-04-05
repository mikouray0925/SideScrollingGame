using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGlobalSetting", menuName = "GlobalSetting")]
public class GlobalSettings : ScriptableObject
{
    public static LayerMask groundLayers {get; private set;}
    public static LayerMask obstacleLayers {get; private set;}
    public static LayerMask creatureLayers {get; private set;}
    public static LayerMask enemyLayers {get; private set;}
    public static LayerMask itemDropLayers {get; private set;}
    public static LayerMask projectileLayers {get; private set;}
    [Header ("LayerMasks")]
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private LayerMask _creatureLayers;
    [SerializeField] private LayerMask _enemyLayers;
    [SerializeField] private LayerMask _itemDropLayers;
    [SerializeField] private LayerMask _projectileLayers;

    public static GameObject itemDropPrefab {get; private set;}
    [Header ("Prefabs")]
    [SerializeField] private GameObject _itemDropPrefab;


    public void SetThis() {
        groundLayers = _groundLayers;
        obstacleLayers = _obstacleLayers;
        creatureLayers = _creatureLayers;
        enemyLayers = _enemyLayers;
        itemDropLayers = _itemDropLayers;
        projectileLayers = _projectileLayers;

        itemDropPrefab = _itemDropPrefab;
    }
}
