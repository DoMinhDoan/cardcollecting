using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class LocalSaveManager : SingletonAsComponent<LocalSaveManager>
{
    public static LocalSaveManager Instance
    {
        get { return ((LocalSaveManager)_Instance); }
        set { _Instance = value; }
    }

    GameSave save = new GameSave();

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Game Saved");
    }

    public void UpdateSaveGame(int score)
    {
        save.score = score;
    }

    private GameSave CreateSaveGameObject()
    {
        save.score = 0;
        return save;
    }

    public GameSave GetGameSave()
    {
        return save;
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            GameSave gameSave = (GameSave)bf.Deserialize(file);

            save = gameSave;

            file.Close();

            Debug.Log("Game Loaded");
        }
        else
        {
            Debug.Log("No game saved!");
        }
    }

    public void SaveAsJSON()
    {
        GameSave save = CreateSaveGameObject();
        string json = JsonUtility.ToJson(save);

        Debug.Log("Saving as JSON: " + json);
    }

}
