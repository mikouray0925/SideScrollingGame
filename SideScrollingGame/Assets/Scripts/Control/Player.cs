using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header ("References")]
    [SerializeField] PlayerInput input;
    public HeroController heroController;
    
    private void Awake() {
        input.actions["Pause"].started += cxt => PauseGame(); 
    }

    private void PauseGame() {
        if(AppManager.instance) {
            AppManager.instance.TriggerGamePause();
        }
    }
}