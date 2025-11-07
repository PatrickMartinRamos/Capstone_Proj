using System.IO;
using UnityEngine;


/// <summary>
/// wag na muna gamitin to for local save/load nlng to use datamanagesr for load and uploading data to google sheets
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    private string saveFilePath;
    public PlayerSaveData CurrentData { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "player_save.json");
        LoadGame();
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(saveFilePath, json);
#if UNITY_EDITOR
        Debug.Log("Game Saved to: " + saveFilePath);
#endif
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            CurrentData = JsonUtility.FromJson<PlayerSaveData>(json);
        }
        else
        {
            CurrentData = new PlayerSaveData();
            SaveGame(); // initialize with default
        }
    }

    // Use this for autosaving coins/exp
    public void UpdateProgress(int stageLVL = 0, int stageScore = 0)
    {
        //TODO: refactor to cater to stage level and score
        CurrentData.stageLevel += stageLVL;
        CurrentData.stageScore += stageScore;
        SaveGame();
    }

    public void SetName(string name)
    {
        CurrentData.playerName = name;
        SaveGame();
    }

    public void SetLevel(int level)
    {
        CurrentData.stageLevel = level;
        SaveGame();
    }

    // You can call this anywhere to get the current saved data
    public PlayerSaveData GetSaveData()
    {
        return CurrentData;
    }
}
