using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class WebGLVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public string videoFileName = "sample BG.mp4";

    void Start()
    {
        string videoPath = Path.Combine(Application.streamingAssetsPath, videoFileName);
        videoPlayer.url = videoPath;
        videoPlayer.playOnAwake = false;

        videoPlayer.Play();
    }
}
