using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.IO;

public class ComicManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject comicCanvas;
    [SerializeField] private VideoPlayer videoPlayer;

    private string currentWorld;
    private int currentStage;

    private void Start()
    {
        if (comicCanvas != null)
            comicCanvas.SetActive(false);

        // Disable autoplay (browser restriction)
        videoPlayer.playOnAwake = false;
    }

    public void PlayComicFor(string worldName, int stageID)
    {
        currentWorld = worldName;
        currentStage = stageID;

        string fileName = $"{worldName}_Comic_{stageID}.mp4";
        string videoPath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (!FileExists(videoPath))
        {
            Debug.LogWarning($"Comic video not found: {videoPath}. Loading game scene instead...");
            SceneManager.LoadScene(worldName);
            return;
        }

        comicCanvas.SetActive(true);
        videoPlayer.url = videoPath;
        videoPlayer.loopPointReached += OnComicFinished;

        // AUTO-PLAY (no click wait)
        StartVideo();
    }

    private void StartVideo()
    {
        Debug.Log("Attempting to auto-play comic...");
        videoPlayer.Play();
    }


    private bool FileExists(string path)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL cannot use File.Exists, but StreamingAssets are always included if uploaded
        return true;
#else
        return File.Exists(path);
#endif
    }

    private void OnComicFinished(VideoPlayer vp)
    {
        vp.loopPointReached -= OnComicFinished;
        Debug.Log($"Comic finished for {currentWorld} - Loading gameplay scene...");
        comicCanvas.SetActive(false);
        SceneManager.LoadScene(currentWorld);
    }
}
