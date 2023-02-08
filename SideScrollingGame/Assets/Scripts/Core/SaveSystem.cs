using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public const string gameSaveFolderName = "/MikouScrollerGame/";
    public const string playerDataFileName = "player.data";

    public static void SavePlayerData(PlayerData data) {
        SaveData<PlayerData>(data, playerDataFileName);
    }

    public static PlayerData LoadPlayerData() {
        return LoadData<PlayerData>(playerDataFileName);
    }

    public static bool HavePlayerData() {
        return HaveSaveFile(playerDataFileName);
    }

    #region UtilityFunctions
    // Do not modify them.

    public static string gameSaveFolderPath {
        get {
            return Application.persistentDataPath + gameSaveFolderName;
        }
        private set {}
    }

    public static void SaveData<T>(T data, string fileName) where T: class {
        if (!typeof(T).IsSerializable) {
            Debug.LogError("Try to save an unserializable class.");
            return;
        }

        string path = gameSaveFolderPath + fileName;
        FileStream fstream = new FileStream(path, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(fstream, data);
        fstream.Close();
    } 

    public static T LoadData<T>(string fileName) where T: class {
        if (!typeof(T).IsSerializable) {
            Debug.LogError("Try to load an unserializable class.");
            return null;
        }

        string path = gameSaveFolderPath + fileName;
        if (File.Exists(path)) {
            FileStream fstream = new FileStream(path, FileMode.Open);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(fstream) as T;
        }
        else {
            Debug.LogError("Save file was not found in " + path);
            return null;
        }
    }

    public static bool HaveSaveFile(string saveFileName) {
        string path = gameSaveFolderPath + saveFileName;
        return File.Exists(path);
    }

    #endregion
}
