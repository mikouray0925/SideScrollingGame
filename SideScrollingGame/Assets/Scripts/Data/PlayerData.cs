using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
    public const string fileName = "Player.data";


    public void Save() {
        SaveSystem.SaveData<PlayerData>(this, fileName);
    }

    public static PlayerData Load() {
        return SaveSystem.LoadData<PlayerData>(fileName);
    }

    public bool Exist() {
        return SaveSystem.HaveSaveFile(fileName);
    }
}
