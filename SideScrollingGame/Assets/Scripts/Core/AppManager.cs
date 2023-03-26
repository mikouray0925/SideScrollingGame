using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] GameObject core;
    public AudioManager audioManager;
    public InterfaceUI joystick;
    public InterfaceUI optionMenu;

    [Header ("Player")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerHolder;
    [SerializeField] public Player localPlayer;

    [Header ("Game Objects")]
    [SerializeField] public GameManager currentGame;
    [SerializeField] public List<GameObject> objNeedToKeep = new List<GameObject>();

    [Header ("Common Scene Names")]
    [SerializeField] string mainMenuSceneName;
    [SerializeField] string chooseHeroSceneName;

    public static AppManager instance {get; private set;}

    public bool gamePaused {get; private set;}
    public bool isChangingScene {get; private set;}

    private void Awake() {
        instance = this;
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

    //|=======================================================
    //| When the "NewGame" button is hitted, this will be called.
    //| Set up everything for a new game.
    //| 
    //| 
    //|=======================================================
    public void StartNewGame() {
        if (localPlayer) {
            ChangeScene(chooseHeroSceneName);
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
        if (isChangingScene) return;
        MessageCenter.SpreadGlobalMsg("BeforeChangingLevel", levelName);
        ChangeScene(levelName, moreObjNeedToMove);
        MessageCenter.SpreadGlobalMsg("AfterChangingLevel",  levelName);
    }

    private void AfterChangingLevel(string levelName) {
        joystick.IsActive = true;
    }

    public void GoBackToMainMenu() {
        if (isChangingScene) return;
        joystick.IsActive = false;
        objNeedToKeep.Clear();
        MessageCenter.SpreadGlobalMsg("BeforeBackToMainMenu");
        ChangeScene(mainMenuSceneName);
        if (gamePaused) TriggerGamePause();
    }

    public void ChangeScene(string sceneName) {
        ChangeScene(sceneName, new List<GameObject>());
    }

    public void ChangeScene(string sceneName, List<GameObject> moreObjNeedToMove) {
        if (isChangingScene) return;
        StartCoroutine(ChangingSceneCoroutine(sceneName, moreObjNeedToMove));
    }

    //|=======================================================
    //| I think a scene is enough for this game.
    //| Multiple scene for big map is a challenge.
    //| 
    //| 
    //|=======================================================
    IEnumerator ChangingSceneCoroutine(string sceneName, List<GameObject> moreObjNeedToMove) {
        isChangingScene = true;

        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        Scene nextScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.MoveGameObjectToScene(core, nextScene);
        foreach (GameObject obj in objNeedToKeep) {
            SceneManager.MoveGameObjectToScene(obj, nextScene);
        }
        foreach (GameObject obj in moreObjNeedToMove) {
            SceneManager.MoveGameObjectToScene(obj, nextScene);
        }

        SceneManager.UnloadSceneAsync(currentScene);
        isChangingScene = false;
    }

    public static void Quit() {   
        Application.Quit();
    }
}
