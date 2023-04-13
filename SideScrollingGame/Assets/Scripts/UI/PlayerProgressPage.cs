using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProgressPage : Page
{
    [SerializeField] Image[] completeLevelSigns;

    public void UpdateDisplay() {
        if (AppManager.instance.localPlayer && 
            AppManager.instance.localPlayer.playerData != null) {
            for (int i = 0; i < completeLevelSigns.Length; i++) {
                completeLevelSigns[i].enabled = i < AppManager.instance.localPlayer.playerData.completeLevelNum;
            }
        }
    }
}
