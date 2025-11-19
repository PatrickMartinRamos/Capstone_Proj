using UnityEngine;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioManager audioManager;

    public void OnMusicSliderChanged(float value)
    {
        AudioManager.Instance.SetMusicVolume(value);
    }

    // public void OnSFXSliderChanged(float value)
    // {
    //     AudioManager.Instance.SetSFXVolume(value);
    // }
}
