using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Settings")]
    public AudioSO audioData;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("Destroying "+this.gameObject.name);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public float GetMasterVolume() => audioData.masterVolume;
    public float GetMusicVolume() => audioData.musicVolume;
    public float GetSFXVolume() => audioData.sfxVolume;

    public void SetMasterVolume(float value)
    {
        audioData.masterVolume = value;
        UpdateAllAudio();
    }

    public void SetMusicVolume(float value)
    {
        audioData.musicVolume = value;
        UpdateAllAudio();
    }

    public void SetSFXVolume(float value)
    {
        audioData.sfxVolume = value;
        UpdateAllAudio();
    }

    public void UpdateAllAudio()
    {
        Audio[] audioObjects = FindObjectsByType<Audio>(FindObjectsSortMode.None);


        foreach (Audio a in audioObjects)
            a.ApplyVolume();
    }
}
