using UnityEngine;

public class FPSSettings : MonoBehaviour
{
    [SerializeField] private int fps;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = fps;
    }
    private void Update() {
        Debug.Log($"FrameRate: {fps}");    
    }
}
