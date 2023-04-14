using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void ContinueSavedGame() {
        if (AppManager.instance) {
            AppManager.instance.ContinueSavedGame();
        }
    }
    
    public void StartNewGame() {
        if (AppManager.instance) {
            AppManager.instance.StartNewGame();
        }
    }
}
