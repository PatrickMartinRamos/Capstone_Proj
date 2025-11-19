using Unity.VisualScripting;
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
        audioSource = StageManager.Instance.audioSrc;
    }

    internal virtual void OnClick()
    {
        audioSource.clip = buttonClickSFX;
        audioSource.Play();
    }
}
