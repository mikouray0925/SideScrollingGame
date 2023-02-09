using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static LayerMask groundLayers {get; private set;}
    public static LayerMask obstacleLayers {get; private set;}

    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private LayerMask _obstacleLayers;

    void Awake() {
        groundLayers = _groundLayers;
        obstacleLayers = _obstacleLayers;
    }

    bool InLayerMask(GameObject obj, LayerMask layerMask) {
        return ((1 << obj.layer & layerMask) != 0);
    }
}
