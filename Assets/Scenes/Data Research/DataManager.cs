using UnityEngine;
using System;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private DataLoader dataLoader;
    private PlayerSaveData currentData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dataLoader = DataLoader.instance;

        if (dataLoader == null)
        {
            Debug.LogWarning("⚠️ DataLoader instance not found. DataManager will not load.");
            Destroy(gameObject);
            return;
        }

        string currentUser = dataLoader.GetCurrentUser();

        if (string.IsNullOrEmpty(currentUser) || currentUser == "No User")
        {
            Debug.Log("ℹ No logged-in user found. Waiting for new player creation...");
            return;
        }

        // Always pull fresh data from Google Sheets
        Debug.Log($"Downloading cloud data for {currentUser}...");
        SyncCloudData(currentUser);
    }

    /// <summary>
    /// Downloads cloud data for the player and handles a scene-specific loading image
    /// </summary>
    public void SyncCloudData(string playerName, GameObject sceneLoadingImage = null, Action<PlayerSaveData> onComplete = null)
    {
        if (sceneLoadingImage != null)
            sceneLoadingImage.SetActive(true);

        dataLoader.Sync.DownloadFromGoogleSheet(playerName, cloudData =>
        {
            if (this == null) return;

            if (sceneLoadingImage != null)
                sceneLoadingImage.SetActive(false);

            if (cloudData != null)
            {
                currentData = cloudData;
                Debug.Log($"Loaded data from Google Sheets for {currentData.playerName}");
            }

            onComplete?.Invoke(cloudData);
        });
    }

    // ------------------ SAVE + UPLOAD ------------------
    public void SaveData(int stageLevel, int stageScore, float playTime, GameObject sceneLoadingImage = null)
    {
        if (currentData == null || string.IsNullOrEmpty(currentData.playerName))
        {
            Debug.LogWarning("No valid current user to save.");
            return;
        }

        currentData.stageLevel = stageLevel;
        currentData.stageScore = stageScore;
        currentData.playTime = playTime;

        UploadToGoogleSheets(sceneLoadingImage);
    }

    public void UploadToGoogleSheets(GameObject sceneLoadingImage = null)
    {
        if (currentData == null || string.IsNullOrEmpty(currentData.playerName))
        {
            Debug.LogWarning("No data to upload.");
            return;
        }

        if (sceneLoadingImage != null)
            sceneLoadingImage.SetActive(true);

        Debug.Log($"⬆️ Uploading {currentData.playerName} to Google Sheets...");
        dataLoader.Sync.UploadToGoogleSheet(currentData, success =>
        {
            if (sceneLoadingImage != null)
                sceneLoadingImage.SetActive(false);

            if (success)
                Debug.Log($"Successfully uploaded {currentData.playerName} data.");
            else
                Debug.LogError($"Failed to upload {currentData.playerName} data.");
        });
    }

    // ------------------ TEST METHODS ------------------
    // [ContextMenu("🧪 Test Update and Upload")]
    // public void TestUpdateCloudData(GameObject sceneLoadingImage = null)
    // {
    //     if (currentData == null || string.IsNullOrEmpty(currentData.playerName))
    //     {
    //         Debug.LogWarning("No current user to test update.");
    //         return;
    //     }

    //     // Randomly update current data
    //     currentData.stageScore += UnityEngine.Random.Range(10, 50);
    //     currentData.stageLevel = Mathf.Min(currentData.stageLevel + 1, 10);
    //     currentData.playTime += UnityEngine.Random.Range(60f, 300f);

    //     Debug.Log($"Testing update for {currentData.playerName}...");
    //     UploadToGoogleSheets(sceneLoadingImage);
    // }

    // ------------------ GETTER ------------------
    public PlayerSaveData GetCurrentData() => currentData;
}
