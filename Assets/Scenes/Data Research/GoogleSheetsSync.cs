using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GoogleSheetsSync : MonoBehaviour
{
    [SerializeField] GameObject loadIcon;
    [Header("Google Web App URL")]
    public string webAppUrl = "https://proxy-server-red-pi.vercel.app";

    //public string webAppUrl = "https://script.google.com/macros/s/AKfycbx7uNeu-MF949aQq9ttJXy8gCWcfmI9awdsYJTTqPXBqYOQkn4Mh0F9iq956zjvSzeezg/exec";

    // Save (POST)
    public void UploadToGoogleSheet(PlayerSaveData data, Action<bool> onComplete = null)
    {
        string jsonData = JsonUtility.ToJson(data);
        StartCoroutine(PostRequest(webAppUrl, jsonData, onComplete));
    }

    // Load (GET)
    public void DownloadFromGoogleSheet(string playerName, Action<PlayerSaveData> onDataReceived)
    {
        string url = $"{webAppUrl}?playerName={UnityWebRequest.EscapeURL(playerName)}";
        StartCoroutine(GetRequest(url, onDataReceived));
    }


    private IEnumerator PostRequest(string url, string json, Action<bool> onComplete = null)
    {
        loadIcon.SetActive(true);

        UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        loadIcon.SetActive(false);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(" Upload failed: " + www.error);
            onComplete?.Invoke(false);
        }
        else
        {
            Debug.Log("✅ Upload success: " + www.downloadHandler.text);
            onComplete?.Invoke(true);
        }
}
    private IEnumerator GetRequest(string url, Action<PlayerSaveData> callback)
    {
        loadIcon.SetActive(true);

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        loadIcon.SetActive(false);

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Download failed: " + www.error);
            callback?.Invoke(null);
        }
        else
        {
            string json = www.downloadHandler.text;
            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
            callback?.Invoke(data);
        }
    }
}
