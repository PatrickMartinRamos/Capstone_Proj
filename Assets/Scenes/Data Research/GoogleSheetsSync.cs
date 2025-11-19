using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetsSync : MonoBehaviour
{
    [SerializeField] private GameObject loadIcon;
    [Header("Google Web App URL")]
    public string webAppUrl = "https://proxy-server-red-pi.vercel.app";

    private Queue<IEnumerator> requestQueue = new Queue<IEnumerator>();
    private bool isProcessingQueue = false;
    private const int maxRetries = 3;
    private const float retryDelay = 0.5f; // seconds between retries

    // -------------------------
    // PUBLIC METHODS
    // -------------------------

    public void UploadToGoogleSheet(PlayerSaveData data, Action<bool> onComplete = null)
    {
        string jsonData = JsonUtility.ToJson(data);
        EnqueueRequest(PostRequest(webAppUrl, jsonData, onComplete));
    }

    public void DownloadFromGoogleSheet(string playerName, Action<PlayerSaveData> onDataReceived)
    {
        string url = $"{webAppUrl}?playerName={UnityWebRequest.EscapeURL(playerName)}";
        EnqueueRequest(GetRequest(url, onDataReceived));
    }

    // -------------------------
    // QUEUE LOGIC
    // -------------------------

    private void EnqueueRequest(IEnumerator request)
    {
        requestQueue.Enqueue(request);

        if (!isProcessingQueue)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        isProcessingQueue = true;

        while (requestQueue.Count > 0)
        {
            yield return StartCoroutine(requestQueue.Dequeue());
            yield return new WaitForSeconds(0.2f); // small delay to prevent rate-limits
        }

        isProcessingQueue = false;
    }

    // -------------------------
    // POST REQUEST
    // -------------------------

    private IEnumerator PostRequest(string url, string json, Action<bool> onComplete = null, int attempt = 1)
    {
        if (loadIcon != null)
            loadIcon.SetActive(true);

        using UnityWebRequest www = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        www.uploadHandler = new UploadHandlerRaw(bodyRaw);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"Upload attempt {attempt} failed: {www.error}");

            if (attempt < maxRetries)
            {
                yield return new WaitForSeconds(retryDelay);
                yield return StartCoroutine(PostRequest(url, json, onComplete, attempt + 1));
            }
            else
            {
                Debug.LogError("Upload failed after max retries.");
                onComplete?.Invoke(false);
            }
        }
        else
        {
            Debug.Log("Upload success: " + www.downloadHandler.text);
            onComplete?.Invoke(true);
        }

        if (loadIcon != null)
            loadIcon.SetActive(false);
    }

    // -------------------------
    // GET REQUEST
    // -------------------------

    private IEnumerator GetRequest(string url, Action<PlayerSaveData> callback, int attempt = 1)
    {
        if (loadIcon != null)
            loadIcon.SetActive(true);

        using UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning($"Download attempt {attempt} failed: {www.error}");

            if (attempt < maxRetries)
            {
                yield return new WaitForSeconds(retryDelay);
                yield return StartCoroutine(GetRequest(url, callback, attempt + 1));
            }
            else
            {
                Debug.LogError("Download failed after max retries.");
                callback?.Invoke(null);
            }
        }
        else
        {
            string json = www.downloadHandler.text;
            PlayerSaveData data = JsonUtility.FromJson<PlayerSaveData>(json);
            callback?.Invoke(data);
        }

        if (loadIcon != null)
            loadIcon.SetActive(false);
    }
}
