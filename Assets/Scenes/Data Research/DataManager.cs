using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    private DataLoader dataLoader;
    private PlayerSaveData currentData; 
    [SerializeField] private GameObject loadingImage;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
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
        if (loadingImage != null)
            loadingImage.SetActive(true);
        // Always pull fresh data from Google Sheets
        Debug.Log($"Downloading cloud data for {currentUser}...");
        dataLoader.Sync.DownloadFromGoogleSheet(currentUser, cloudData =>
        {
            // If this MonoBehaviour was destroyed while the async request was in-flight, bail out.
            if (this == null) return;

            if (cloudData != null)
            {
                currentData = cloudData;
                if (loadingImage != null)
                    loadingImage.SetActive(false);
                Debug.Log($" Loaded data from Google Sheets for {currentData.playerName}");
                Debug.Log($" Level: {currentData.stageLevel}, \nScore: {currentData.stageScore}, \nTime: {currentData.playTime:F1}s");
            }
            else
            {
                if (loadingImage != null)
                    loadingImage.SetActive(false);
            }

        });
    }

    // ------------------ SAVE + UPLOAD ------------------
    public void SaveData(int stageLevel, int stageScore, float playTime)
    {
        if (currentData == null || string.IsNullOrEmpty(currentData.playerName))
        {
            Debug.LogWarning("No valid current user to save.");
            return;
        }

        currentData.stageLevel = stageLevel;
        currentData.stageScore = stageScore;
        currentData.playTime = playTime;

        Debug.Log($"Updated data for {currentData.playerName}: Level {stageLevel}, Score {stageScore}, Time {playTime:F1}s");
        UploadToGoogleSheets();
    }

    // ------------------ UPLOAD ------------------
    public void UploadToGoogleSheets()
    {
        if (currentData == null || string.IsNullOrEmpty(currentData.playerName))
        {
            Debug.LogWarning("No data to upload.");
            return;
        }

        Debug.Log($"⬆️ Uploading {currentData.playerName} to Google Sheets...");
        dataLoader.Sync.UploadToGoogleSheet(currentData, success =>
        {
            if (success)
                Debug.Log($"Successfully uploaded {currentData.playerName} data.");
            else
                Debug.LogError($"Failed to upload {currentData.playerName} data.");
        });
    }

    // ------------------ TEST BUTTON ------------------
    [ContextMenu("🧪 Test Update and Upload to Cloud Data")]
    public void TestUpdateCloudData()
    {
        
        if (currentData == null || string.IsNullOrEmpty(currentData.playerName))
        {
            Debug.LogWarning("No current user to test update.");
            return;
        }
        //give random data for  the current log in user
        currentData.stageScore += Random.Range(10, 50);
        currentData.stageLevel = Mathf.Min(currentData.stageLevel + 1, 10);
        currentData.playTime += Random.Range(60f, 300f);
        
        Debug.Log($"Testing update for {currentData.playerName}...");
        UploadToGoogleSheets();
    }

    // ------------------ GETTER ------------------
    public PlayerSaveData GetCurrentData() => currentData;
}
