using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoUI : InterfaceUI
{
    [SerializeField] GameObject[] previewStateObjects;

    public bool inPreviewState {get; private set;}
    
    public void EnterPreviewState() {
        foreach (GameObject obj in previewStateObjects) {
            obj.SetActive(true);
        }

        if (AppManager.instance.LocalHero != null) {
            AppManager.instance.LocalHero.movement.enabled = false;
            AppManager.instance.LocalHero.normalAttack.enabled = false;
            AppManager.instance.LocalHero.ability1.enabled = false;
            AppManager.instance.LocalHero.ability2.enabled = false;
            AppManager.instance.LocalHero.inventory.enabled = false;
        }

        inPreviewState = true;
    }

    public void LeavePreviewState() {
        foreach (GameObject obj in previewStateObjects) {
            obj.SetActive(false);
        }

        if (AppManager.instance.LocalHero != null) {
            AppManager.instance.LocalHero.movement.enabled = true;
            AppManager.instance.LocalHero.normalAttack.enabled = true;
            AppManager.instance.LocalHero.ability1.enabled = true;
            AppManager.instance.LocalHero.ability2.enabled = true;
            AppManager.instance.LocalHero.inventory.enabled = true;
        }

        inPreviewState = false;
        Hide();
    }
}
