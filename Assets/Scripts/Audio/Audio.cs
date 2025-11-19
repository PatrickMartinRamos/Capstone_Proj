using UnityEngine;

public class Audio : MonoBehaviour
{
    public enum AudioType { Music, SFX }
    public AudioType audioType;

    private AudioSource source;

    private void Awake()
    {
        source = GetComponent<AudioSource>();
        ApplyVolume();
    }

    // Called when volumes change
    public void ApplyVolume()
    {
        if (AudioManager.Instance == null)
            return;

        float master = AudioManager.Instance.GetMasterVolume();

        switch (audioType)
        {
            case AudioType.Music:
                source.volume = master * AudioManager.Instance.GetMusicVolume();
                break;

            case AudioType.SFX:
                source.volume = master * AudioManager.Instance.GetSFXVolume();
                break;
        }
    }
}
