using UnityEngine;
using UnityEngine.UI;

public class ButtonEvent : MonoBehaviour
{
    internal Button button;
    [SerializeField] internal AudioSource audioSource;
    [SerializeField] internal AudioClip buttonClickSFX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
        audioSource = GetComponent<AudioSource>();

        // If no AudioSource exists, add one
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    internal virtual void OnClick()
    {
        audioSource.clip = buttonClickSFX;
        audioSource.Play();
    }
}
