using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppManager : MonoBehaviour
{
    [SerializeField] GameObject core;
    public AudioManager audioManager;
    public OptionMenu optionMenu;

    public static AppManager instance {get; private set;}

    private void Awake() {
        instance = this;
    }

    public void ChangeScene(string sceneName) {
        StartCoroutine(ChangingSceneCoroutine(sceneName));
    }

    IEnumerator ChangingSceneCoroutine(string sceneName) {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone) {
            yield return null;
        }

        SceneManager.MoveGameObjectToScene(core, SceneManager.GetSceneByName(sceneName));
        SceneManager.UnloadSceneAsync(currentScene);
    }

    public static void Quit() {   
        Application.Quit();
    }
}
