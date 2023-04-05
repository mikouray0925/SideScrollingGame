using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//|=======================================================
//| The global manager of this app.
//| There should be only one <AppManager> which is in the
//| core obj. This is different from <GameManager> which
//| manages a game level scene.
//| Maybe this is not the best way to manage game.
//| I've try my best to organize all.
//|=======================================================
public class AppManager : MonoBehaviour
{
    [Header ("Core Objects")]
    [SerializeField] AudioManager audioManager;
    [SerializeField] SceneController sceneController;
    [SerializeField] GlobalSettings globalSettings;
    public GameManager currentGame;

    [Header ("UI Canvas")]
    public PlayerHUD playerHUD;
    public InterfaceUI playerInfoPages;
    public InterfaceUI joystick;
    public InterfaceUI optionMenu;

    [Header ("Player")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerHolder;
    [SerializeField] public Player localPlayer;

    [Header ("Core Scene Names")]
    [SerializeField] string mainMenuSceneName;
    [SerializeField] string chooseHeroSceneName;

    public static AppManager instance {get; private set;}

    public bool gamePaused {get; private set;}

    private void Awake() {
        instance = this;
        globalSettings.SetThis();
    }

    //|=======================================================
    //| If "gamePaused" is true, try to continue the game.
    //| If false, try to pause the game.
    //| Very useful when ESC is hitted.
    //| 
    //|=======================================================
    public void TriggerGamePause() {
        if (gamePaused) {
            Time.timeScale = 1f;
            optionMenu.Hide();
            gamePaused = false;
        } else {
            Time.timeScale = 0f;
            MessageCenter.SpreadGlobalMsg("OnGameBeingPaused");
            optionMenu.Show();
            gamePaused = true;
        }
    }

    //|=======================================================
    //| Create a player obj and put it into playerHolder obj.
    //| Set "localPlayer" to this player, means the player of
    //| this computer is this one.
    //| 
    //|=======================================================
    public void AddLocalPlayer() {
        if (playerPrefab && playerHolder) {
            localPlayer = Instantiate(playerPrefab, playerHolder).GetComponent<Player>();
        }
        else {
            Debug.LogError("playerPrefab or playerHolder refernece is not set.");
        }
    }

    public HeroBrain LocalHero {
        get {
            if (localPlayer != null &&
                localPlayer.heroController.bindingHero != null) {
                    return localPlayer.heroController.bindingHero;
            } else {
                return null;
            }
        }
        set {
            if (value != null) {
                if (localPlayer.heroController.bindingHero == null) {
                    SceneController.instance.objNeedToKeep.Add(value.gameObject);
                    localPlayer.heroController.Bind(value);
                    // playerHUD.Bind(value);
                } else {
                    Debug.LogError("Local player has already binded a hero.");
                }        
            } else {
                Debug.LogError("Use UnbindLocalHero() to unbind instead of setting null.");
            }
        }
    }

    public void UnbindLocalHero() {
        SceneController.instance.objNeedToKeep.Remove(LocalHero.gameObject);
        if (playerHUD.bindingHero == LocalHero) playerHUD.Unbind();
        localPlayer.heroController.Unbind();
    }

    //|=======================================================
    //| When the "NewGame" button is hitted, this will be called.
    //| Set up everything for a new game.
    //| 
    //| 
    //|=======================================================
    public void StartNewGame() {
        if (localPlayer) {
            sceneController.ChangeScene(chooseHeroSceneName);
        }
    }

    //|=======================================================
    //| Load the game level scene. Set up everything for the 
    //| game level in this func. 
    //| Nothing is inplement yet.
    //| 
    //|=======================================================
    public void PlayGameLevel(string levelName) {
        PlayGameLevel(levelName, new List<GameObject>());
    }

    public void PlayGameLevel(string levelName, List<GameObject> moreObjNeedToMove) {
        if (sceneController.isChangingScene) return;
        MessageCenter.SpreadGlobalMsg("BeforeChangingLevel", levelName);
        sceneController.ChangeScene(levelName, moreObjNeedToMove);
        MessageCenter.SpreadGlobalMsg("AfterChangingLevel",  levelName);
    }

    private void AfterChangingLevel(string levelName) {
        joystick.Show();
        playerHUD.Show();
    }

    public void GoBackToMainMenu() {
        if (sceneController.isChangingScene) return;
        playerHUD.Unbind();
        playerHUD.Hide();
        joystick.Hide();
        sceneController.objNeedToKeep.Clear();
        MessageCenter.SpreadGlobalMsg("BeforeBackToMainMenu");
        sceneController.ChangeScene(mainMenuSceneName);
        if (gamePaused) TriggerGamePause();
    }

    
    public static void Quit() {   
        Application.Quit();
    }
}
