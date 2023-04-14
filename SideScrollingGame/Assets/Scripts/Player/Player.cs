using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] PlayerInput input;
    public HeroController heroController;

    public PlayerData playerData;
    
    private void Awake() {
        input.actions["Pause"].started += cxt => PauseGame(); 
        input.actions["ShowInfoPages"].started += cxt => ShowInfoPages();
    }

    private void PauseGame() {
        if(AppManager.instance) {
            if (AppManager.instance.playerInfoPages.IsActive) {
                AppManager.instance.playerInfoPages.Hide();
            } else {
                AppManager.instance.TriggerGamePause();
            }
        }
    }

    private void ShowInfoPages() {
        if (heroController.bindingHero != null) {
            AppManager.instance.playerInfoPages.Show();
        }
    }

    private void BeforeBackToMainMenu() {
        heroController.Unbind();
    }

    public void LoadData() {
        if (PlayerData.Exist()) {
            playerData = PlayerData.Load();
            if (GlobalSettings.heroDict.TryGetValue(playerData.heroName,
            out GameObject heroPrefab)) {
                GameObject heroObj = Instantiate(heroPrefab);
                HeroBrain hero = heroObj.GetComponent<HeroBrain>();
                if (AppManager.instance.localPlayer == this) {
                    AppManager.instance.LocalHero = hero;
                } else {
                    
                }
                hero.ReadSave(playerData);

                AppManager.instance.PlayGameLevel(playerData.inSceneName);
            } 
        } else {
            Debug.LogError("No data can be load for the player.");
        }

    }

    public void SaveData() {
        if (playerData == null) playerData = new PlayerData();
        if (heroController.bindingHero != null) heroController.bindingHero.WriteSave(playerData);
        playerData.Save();
    }
}
