using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public const string fileName = "Player.data";

    public string heroName;
    public int completeLevelNum = 0;
    public string inSceneName;
    public float healthProportion;
    public int[] IdsOfItemsInSlots = new int[100];


    public void Save() {
        SaveSystem.SaveData<PlayerData>(this, fileName);
    }

    public static PlayerData Load() {
        return SaveSystem.LoadData<PlayerData>(fileName);
    }

    public static bool Exist() {
        return SaveSystem.HaveSaveFile(fileName);
    }
}
