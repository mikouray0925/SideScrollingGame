using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject core;
    public LoadingScreen loadingScreen;
    public List<GameObject> objNeedToKeep = new List<GameObject>();

    public static SceneController instance {get; private set;}
    public bool isChangingScene {get; private set;}

    private void Awake() {
        instance = this;
    }

    public void ChangeScene(string sceneName) {
        ChangeScene(sceneName, new List<GameObject>(), true);
    }

    public void ChangeScene(string sceneName, bool showLoadingScreen) {
        ChangeScene(sceneName, new List<GameObject>(), showLoadingScreen);
    }

    public void ChangeScene(string sceneName, List<GameObject> moreObjNeedToMove, bool showLoadingScreen = true) {
        if (isChangingScene) return;
        StartCoroutine(ChangingSceneCoroutine(sceneName, moreObjNeedToMove, showLoadingScreen));
    }

    //|=======================================================
    //| I think a scene is enough for this game.
    //| Multiple scene for big map is a challenge.
    //| 
    //| 
    //|=======================================================
    IEnumerator ChangingSceneCoroutine(string sceneName, List<GameObject> moreObjNeedToMove, bool showLoadingScreen = true) {
        isChangingScene = true;
        if (showLoadingScreen) loadingScreen.Show();

        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            loadingScreen.progress = asyncLoad.progress;
            yield return null;
        }
        loadingScreen.progress = 1f;

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
        if (showLoadingScreen) {
            while (!loadingScreen.SliderReachProgress) yield return null;
            loadingScreen.Hide();
        }
    }

    public static string ActiveSceneName {
        get {
            return SceneManager.GetActiveScene().name;
        }
        private set {}
    }
}
