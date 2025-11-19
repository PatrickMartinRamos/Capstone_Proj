using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class TutorialWebPlayer : MonoBehaviour
{
    [Header("Video Settings")]
    public VideoPlayer videoPlayer;
    public string videoFileName = "World1_page2_tutorial.mp4";

    private void Start()
    {
        string videoPath = Path.Combine(Application.streamingAssetsPath, videoFileName);
        //videoPlayer.playOnAwake = false;
        videoPlayer.url = videoPath;
        videoPlayer.Play();
    }
}
