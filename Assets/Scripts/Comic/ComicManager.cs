using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class ComicManager : MonoBehaviour
{
    [Header("World 1 Comics")]
    [SerializeField] private VideoClip[] world1Comics;

    [Header("World 2 Comics")]
    [SerializeField] private VideoClip[] world2Comics;

    [Header("UI")]
    [SerializeField] private GameObject comicCanvas;  // Canvas that holds the comic UI
    [SerializeField] private VideoPlayer videoPlayer;

    private string currentWorld;
    private int currentStage;

    private void Start()
    {
        // Make sure the comic UI starts hidden
        if (comicCanvas != null)
            comicCanvas.SetActive(false);
    }

    public void PlayComicFor(string worldName, int stageID)
    {
        currentWorld = worldName;
        currentStage = stageID;

        VideoClip clip = GetComicClip(worldName, stageID);

        if (clip == null)
        {
            Debug.LogWarning($"No comic clip found for {worldName} - Stage {stageID}");
            SceneManager.LoadScene(worldName);
            return;
        }

        // Show the comic UI and play the video
        comicCanvas.SetActive(true);
        videoPlayer.clip = clip;
        videoPlayer.loopPointReached += OnComicFinished;
        videoPlayer.Play();

        Debug.Log($"Playing comic for {worldName} - Stage {stageID}");
    }

    private VideoClip GetComicClip(string worldName, int stageID)
    {
        int index = Mathf.Clamp(stageID - 1, 0, 14);

        switch (worldName)
        {
            case "World1_GameScene":
                return (world1Comics.Length > index) ? world1Comics[index] : null;
            case "World2_GameScene":
                return (world2Comics.Length > index) ? world2Comics[index] : null;
            default:
                return null;
        }
    }

    private void OnComicFinished(VideoPlayer vp)
    {
        vp.loopPointReached -= OnComicFinished;
        Debug.Log($"Comic finished for {currentWorld} - Loading gameplay scene...");
        comicCanvas.SetActive(false);
        SceneManager.LoadScene(currentWorld);
    }
}
