using UnityEngine;

[CreateAssetMenu(fileName = "AudioSO", menuName = "Audio Settings")]
public class AudioSO : ScriptableObject
{
    [Range(0f, 1f)]
    public float masterVolume = 1f;

    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [Range(0f, 1f)]
    public float sfxVolume = 1f;

}
