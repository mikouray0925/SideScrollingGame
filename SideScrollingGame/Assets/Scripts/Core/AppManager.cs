using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    [SerializeField] GameObject core;
    public AudioManager audioManager;
    public InterfaceUI optionMenu;

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

    public bool ChangeScene(string sceneName) {
        if (isChangingScene) return false;
        StartCoroutine(ChangingSceneCoroutine(sceneName, new List<GameObject>()));
        return true;
    }

    IEnumerator ChangingSceneCoroutine(string sceneName, List<GameObject> objThatNeedToMove) {
        isChangingScene = true;

        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        Scene nextScene = SceneManager.GetSceneByName(sceneName);
        SceneManager.MoveGameObjectToScene(core, nextScene);
        foreach (GameObject obj in objThatNeedToMove) {
            SceneManager.MoveGameObjectToScene(obj, nextScene);
        }

        SceneManager.UnloadSceneAsync(currentScene);
        isChangingScene = false;
    }

    public static void Quit() {   
        Application.Quit();
    }
}
