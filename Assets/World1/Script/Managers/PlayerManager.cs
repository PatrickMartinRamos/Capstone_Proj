using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Selected Tool")]
    [SerializeField] private Tool chosenTool;
    public static PlayerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
}
