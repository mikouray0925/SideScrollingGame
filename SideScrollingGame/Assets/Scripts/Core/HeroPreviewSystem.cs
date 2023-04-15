using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPreviewSystem : MonoBehaviour
{
    [SerializeField] public Vector3 heroPos;
    
    void Awake() {
        if (AppManager.instance) {
            if (AppManager.instance.localPlayer && 
                AppManager.instance.localPlayer.heroController.IsBinded()) {
                AppManager.instance.playerInfoPages.IsActive = true;
                AppManager.instance.playerInfoPages.EnterPreviewState();

                AppManager.instance.localPlayer.heroController.SetHeroPosTo(heroPos);
            }
            else {
                Debug.LogError("Some instances of AppManager.localPlayer are not set.");
            }
        }
        else {
            Debug.LogError("The instance of AppManager is not set.");
        }
    }

    private void LateUpdate() {
        if (AppManager.instance.playerInfoPages.inPreviewState) {
            AppManager.instance.playerInfoPages.IsActive = true;
        }
    }
}
