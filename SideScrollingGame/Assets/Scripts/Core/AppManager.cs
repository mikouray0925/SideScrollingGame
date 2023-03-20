using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//|=======================================================
//| The global manager of this app.
//| There should be only one <AppManager> which is in the
//| core obj. This is different from <GameManager> which
//| manages a game level scene.
//|=======================================================
public class AppManager : MonoBehaviour
{
    [Header ("Core Objects")]
    [SerializeField] GameObject core;
    public AudioManager audioManager;
    public InterfaceUI optionMenu;

    [Header ("Player")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform playerHolder;
    [SerializeField] public Player localPlayer;

    [Header ("Game Objects")]
    [SerializeField] public GameManager currentGame;
    [SerializeField] public List<GameObject> objNeedToKeep = new List<GameObject>();

    [Header ("SceneNames")]
    [SerializeField] string chooseHeroSceneName;

    public static AppManager instance {get; private set;}

    public bool gamePaused {get; private set;}
    public bool isChangingScene {get; private set;}

    private void Awake() {
        instance = this;
    }

    public void TriggerGamePause() {
        if (gamePaused) {
            Time.timeScale = 1f;
            optionMenu.Hide();
            gamePaused = false;
        } else {
            Time.timeScale = 0f;
            optionMenu.Show();
            gamePaused = true;
        }
    }

    public void AddLocalPlayer() {
        if (playerPrefab && playerHolder) {
            localPlayer = Instantiate(playerPrefab, playerHolder).GetComponent<Player>();
        }
        else {
            Debug.LogError("playerPrefab or playerHolder refernece is not set.");
        }
    }

    public void StartNewGame() {
        if (localPlayer) {
            ChangeScene(chooseHeroSceneName);
        }
    }

    public void PlayGameLevel(string levelName) {
        PlayGameLevel(levelName, new List<GameObject>());
    }

    public void PlayGameLevel(string levelName, List<GameObject> moreObjNeedToMove) {
        if (isChangingScene) return;
        StartCoroutine(ChangingSceneCoroutine(levelName, moreObjNeedToMove));
    }

    public void ChangeScene(string sceneName) {
        ChangeScene(sceneName, new List<GameObject>());
    }

    public void ChangeScene(string sceneName, List<GameObject> moreObjNeedToMove) {
        if (isChangingScene) return;
        StartCoroutine(ChangingSceneCoroutine(sceneName, moreObjNeedToMove));
    }

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
