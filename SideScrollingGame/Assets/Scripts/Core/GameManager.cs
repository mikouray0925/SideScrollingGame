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

    [Header ("LayerMasks")]
    [SerializeField] private LayerMask _groundLayers;
    [SerializeField] private LayerMask _obstacleLayers;
    [SerializeField] private LayerMask _creatureLayers;
    [SerializeField] private LayerMask _enemyLayers;

    [Header ("References")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform _impactEffectHolder;

    [Header ("SceneSettings")]
    [SerializeField] public Vector3 heroSpawnPos;

    void Awake() {
        print("GameManager awaked.");

        groundLayers = _groundLayers;
        obstacleLayers = _obstacleLayers;
        creatureLayers = _creatureLayers;
        enemyLayers = _enemyLayers;

        impactEffectHolder = _impactEffectHolder;

        if (AppManager.instance) {
            AppManager.instance.currentGame = this;
            if (AppManager.instance.localPlayer && 
                AppManager.instance.localPlayer.heroController.IsBinded()) {
                AppManager.instance.localPlayer.heroController.SetHeroPosTo(heroSpawnPos);
                AppManager.instance.localPlayer.heroController.MakeCameraFollowHero(mainCamera.GetComponent<CameraFollow>());
                print("Successfully spawn hero.");
            }
            else {
                Debug.LogError("Some instances of AppManager.localPlayer are not set.");
            }
        }
        else {
            Debug.LogError("The instance of AppManager is not set.");
        }
        /*
        Message deactivateAllEmeny = Message.DeactivatingOrder;
        deactivateAllEmeny.filterByLayerMask = true;
        deactivateAllEmeny.layerMask = enemyLayers;
        MessageCenter.SpreadGlobalMsg(deactivateAllEmeny);
        */
    }

    public static bool InLayerMask(GameObject obj, LayerMask layerMask) {
        return ((1 << obj.layer & layerMask) != 0);
    }

    public static ImpactEffectSystem SpawnImpactEffect(GameObject impactPrefab) {
        if (!impactEffectHolder) return null;
        GameObject impact = Instantiate(impactPrefab, impactEffectHolder);
        return impact.GetComponent<ImpactEffectSystem>();
    }
}
