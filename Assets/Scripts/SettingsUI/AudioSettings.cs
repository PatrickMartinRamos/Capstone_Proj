using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    void Start()
    {
        if(audioManager == null)
            audioManager = AudioManager.Instance;
    }

    public void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    // public void OnSFXSliderChanged(float value)
    // {
    //     AudioManager.Instance.SetSFXVolume(value);
    // }
}
