using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private int stageNumber;
    public int StageNumber => stageNumber;

    [Header("CraftBoxes")]
    [SerializeField] public GameObject squaringBinomialBox;
    [SerializeField] public GameObject radicalsBox;
    [SerializeField] public GameObject completingSquareBox;

    [Header("GameObjects")]
    public GameObject cannon;
    public GameObject bullet;
    public GameObject craftArea;
    public GameObject gameplaySpawnPt;
    public GameObject bulletCase;
    public GameObject ActiveGameArea;
    public GameObject targetBomb;
    public List<GameObject> StageBombs;


    [Header("ShapesList")]
    public GameObject variable;
    public GameObject constant;
    public GameObject squaredVariable;
    public GameObject squaredConstant;
    public GameObject product;

    [Header("Problem")]
    public ProblemLoader problem;

    [Header("UI")]
    public float timeLimit;
    private float time; public float currentTime => time;
    public Slider timer;
    public TextMeshProUGUI NotificationText;
    public GameObject correctAnswerPanel, WrongAnswerPanel;
    public GameObject helpPanel;
    public GameObject stageIndicatorLabel;

    public bool ShowCraftBox, craftBoxIsHidden=false, startGame = false;

    // Singleton Instance
    public static StageManager Instance { get; private set; }

    // Events 
    public static UnityEvent CloseGameAreaEvent = new();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      //  stageIndicatorLabel.GetComponent<TextMeshProUGUI>().text = $"Stage {stageNumber}";
    }

    // Update is called once per frame
    void Update()
    {
        if (ActiveGameArea != null)
        {
            time = timeLimit;
            timer.maxValue = timeLimit;
            timer.value = time;
            timer.gameObject.SetActive(false);
        }
    }
    public void StartSuccessSequence()
    {
        CloseGameAreaEvent.Invoke();
        Sequence seq = DOTween.Sequence();
        seq.Append(ActiveGameArea.transform.DOLocalMoveY(1, 1f));
        seq.Append(ActiveGameArea.transform.DOScale(Vector3.zero, 1f));
        seq.Append(cannon.transform.DOPunchPosition(Vector3.down, 0.3f, 2));
        seq.OnComplete(()=>bullet.SetActive(true));
    }
    public void StartGame()
    {
        startGame = true;
    }
    public void DeductTime(float deduction)
    {
        time = time - deduction;
    }
    public void OpenWrongAnswerPanel()
    {
        WrongAnswerPanel.SetActive(true);
    }
    public void CloseWrongAnswerPanel()
    {
        WrongAnswerPanel.SetActive(false);
    }
    public void RestartStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextStage()
    {
        PlayerManager.Instance.playerStatusJSONManager.UpdatePlayerStatus();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
}
