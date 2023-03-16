using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] PlayerInput input;
    
    private void Awake() {
        input.actions["Pause"].started += cxt => PauseGame(); 
    }

    private void PauseGame() {
        if(AppManager.instance) {
            AppManager.instance.TriggerGamePause();
        }
    }
}
