using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LayerMask groundLayers {get; private set;}
    public static LayerMask obstacleLayers {get; private set;}
    public static LayerMask creatureLayers {get; private set;}
    public static LayerMask enemyLayers {get; private set;}

    public static Transform impactEffectHolder {get; private set;}

    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private LayerMask _creatureLayers;
    [SerializeField] private LayerMask _enemyLayers;
    [SerializeField] private Transform _impactEffectHolder;

    void Awake() {
        groundLayers = _groundLayers;
        obstacleLayers = _obstacleLayers;
        creatureLayers = _creatureLayers;
        enemyLayers = _enemyLayers;

        impactEffectHolder = _impactEffectHolder;
    }

    public static bool InLayerMask(GameObject obj, LayerMask layerMask) {
        return ((1 << obj.layer & layerMask) != 0);
    }
}
