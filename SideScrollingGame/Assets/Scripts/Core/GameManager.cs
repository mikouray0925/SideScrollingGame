using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Transform objectPool {get; private set;}
    public static Transform impactEffectHolder {get; private set;}
    public Transform itemDropHolder {get; private set;}

    [Header ("References")]
    [SerializeField] private CameraFollow mainCamFollow;
    [SerializeField] private CameraFollow heroCloseupCamFollow;
    [SerializeField] private CameraFollow minimapCamFollow;
    [SerializeField] private Transform _objectPool;
    [SerializeField] private Transform _impactEffectHolder;
    [SerializeField] private Transform _itemDropHolder;

    [Header ("SceneSettings")]
    [SerializeField] public Vector3 heroSpawnPos;
    [SerializeField] public string nextLevelName;

    [Header ("Events")]
    [SerializeField] UnityEvent onAwake;

    void Awake() {
        print("GameManager awaked.");

        objectPool = _objectPool;
        impactEffectHolder = _impactEffectHolder;
        itemDropHolder = _itemDropHolder;

        if (AppManager.instance) {
            AppManager.instance.currentGame = this;
            if (AppManager.instance.localPlayer && 
                AppManager.instance.localPlayer.heroController.IsBinded()) {
                AppManager.instance.localPlayer.heroController.SetHeroPosTo(heroSpawnPos);
                AppManager.instance.localPlayer.heroController.MakeCameraFollowHero(mainCamFollow);
                AppManager.instance.localPlayer.heroController.MakeCameraFollowHero(heroCloseupCamFollow);
                AppManager.instance.localPlayer.heroController.MakeCameraFollowHero(minimapCamFollow);
                AppManager.instance.localPlayer.playerData.inSceneName = gameObject.scene.name;
                AppManager.instance.localPlayer.SaveData();
                print("Successfully spawn hero.");
                onAwake.Invoke();
            }
            else {
                Debug.LogError("Some instances of AppManager.localPlayer are not set.");
            }
        }
        else {
            Debug.LogError("The instance of AppManager is not set.");
        }
    }

    public void ChangeMusic(AudioClip music) {
        AudioManager.instance.musicPlayer.clip = music;
        if (!AudioManager.instance.musicPlayer.isPlaying) 
            AudioManager.instance.musicPlayer.Play();
    }

    public void DeactivateAllEmeny() {
        Message deactivateAllEmeny = Message.DeactivatingOrder;
        deactivateAllEmeny.filterByLayerMask = true;
        deactivateAllEmeny.layerMask = GlobalSettings.enemyLayers;
        MessageCenter.SpreadGlobalMsg(deactivateAllEmeny);
    }

    public static ImpactEffectSystem SpawnImpactEffect(GameObject impactPrefab) {
        if (!impactEffectHolder) return null;
        GameObject impact = Instantiate(impactPrefab, impactEffectHolder);
        return impact.GetComponent<ImpactEffectSystem>();
    }

    public void ToNextLevel() {
        AppManager.instance.localPlayer.playerData.completeLevelNum += 1;
        AppManager.instance.PlayGameLevel(nextLevelName);
    }

    public void ShowGameWin() {
        AppManager.instance.endGameWindow.Show();
        AppManager.instance.endGameWindow.text.text = "You Win";
    }
}
