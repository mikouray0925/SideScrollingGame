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
            if (heroController.bindingHero != null) heroController.bindingHero.ReadSave(playerData);
        }

    }

    public void SaveData() {
        if (playerData == null) playerData = new PlayerData();
        if (heroController.bindingHero != null) heroController.bindingHero.WriteSave(playerData);
        // playerData.Save();
    }
}
