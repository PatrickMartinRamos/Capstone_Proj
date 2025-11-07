using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class DataLoader : MonoBehaviour
{
    public static DataLoader instance;
    private GoogleSheetsSync sync;
    public GoogleSheetsSync Sync => sync;
    string currentPlayerName;
    int currentStageLevel, currentScore;
    private float currentPlayTime;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        sync = GetComponent<GoogleSheetsSync>();
    }

    public void UploadData(string _playerName, int _stageLevel, int _stageScore, float _playTime, Action<bool> onComplete = null)
    {
        // Set player data immediately before any checking or uploading
        currentPlayerName = _playerName;
        currentStageLevel = _stageLevel;
        currentScore = _stageScore;
        currentPlayTime = _playTime;

        if (sync == null)
        {
            Debug.LogError(" GoogleSheetsSync is NULL in DataLoader!");
            onComplete?.Invoke(false);
            return;
        }

        Debug.Log($"Checking if {_playerName}'s data needs updating...");

        PlayerSaveData newData = new PlayerSaveData
        {
            playerName = _playerName,
            stageLevel = _stageLevel,
            stageScore = _stageScore,
            playTime = _playTime
        };

        // Continue with your normal check-download-upload flow
        sync.DownloadFromGoogleSheet(_playerName, existingData =>
        {
            bool hasExistingData = existingData != null && 
                                !string.IsNullOrEmpty(existingData.playerName) &&
                                existingData.playerName == _playerName;

            if (hasExistingData)
            {
                bool isSame =
                    existingData.stageLevel == newData.stageLevel &&
                    existingData.stageScore == newData.stageScore &&
                    Mathf.Approximately(existingData.playTime, newData.playTime);

                if (isSame)
                {
                    Debug.Log("No update needed. Data is the same.");
                    onComplete?.Invoke(true);
                    return;
                }
            }
            else
            {
                Debug.Log($"🆕 No existing data found for {_playerName}, creating new entry...");
            }

            // Upload new data if different or not found
            sync.UploadToGoogleSheet(newData, success =>
            {
                if (success)
                {
                    Debug.Log($"✅ Data uploaded for {_playerName}");
                    onComplete?.Invoke(true);
                }
                else
                {
                    Debug.LogWarning($"⚠️ Upload failed for {_playerName}");
                    onComplete?.Invoke(false);
                }
            });
        });

    }
        
    public string GetCurrentUser()
    {
        if(currentPlayerName == null) return "No User";
        return currentPlayerName;
    }

    public int GetCurrentUserStageLevel()
    {
        if(currentStageLevel == 0) return 0;
        return currentStageLevel;
    }

    public int GetCurrentUserScore()
    {
        if(currentScore == 0) return 0;
        return currentScore;
    }

    public float GetCurrentUserPlayTime()
    {
        if(currentPlayTime == 0f) return 0f;
        return currentPlayTime;
    }
}
