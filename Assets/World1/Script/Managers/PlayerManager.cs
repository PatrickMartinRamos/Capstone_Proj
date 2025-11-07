using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private AlgebraicFoundationPlayerStatus playerStatus;

    [Header("JSON Manager")]
    public PlayerStatusJSONManager playerStatusJSONManager;
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
    private void Start()
    {
        playerStatus.currentStage = StageManager.Instance.StageNumber;
    }
}
