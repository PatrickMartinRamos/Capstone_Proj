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
    public float currentTime;
    public Slider timer;
    public TextMeshProUGUI NotificationText;
    public GameObject correctAnswerPanel, WrongAnswerPanel;
    public GameObject notificationPanel;
    public GameObject stageIndicatorLabel;
    public GameObject astronaut;

    [Header("Managers")]
    [SerializeField] public BombsManager bombsManager;
    [SerializeField] public UIManager uiManager;

    [Header("Audio")]
    [SerializeField] public AudioSource audioSrc;

    public bool isPlaying = false;
    public float playTimer;


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

        bombsManager = bombsManager == null ? gameObject.AddComponent<BombsManager>() : bombsManager;
        uiManager = uiManager == null ? gameObject.AddComponent<UIManager>() : uiManager;
        audioSrc = GameObject.FindFirstObjectByType<AudioSource>();

        currentTime = timeLimit;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartSuccessSequence()
    {
        currentTime = timer.GetComponent<TimerMechanics>().GetCurrentTime();
        CloseGameAreaEvent.Invoke();

        Sequence seq = DOTween.Sequence();
        seq.Append(ActiveGameArea.transform.DOLocalMoveY(1.5f, 1f));
        seq.Append(ActiveGameArea.transform.DOLocalMoveY(-15, 0.5f));
        seq.Append(cannon.transform.DOPunchPosition(Vector3.down, 0.3f, 2));
        seq.OnComplete(() => { bullet.SetActive(true); ActiveGameArea.SetActive(false);
        });
    }

    public void StartGame()
    {
        isPlaying = true;
    }
    public void DeductTime(float deduction)
    {
        timer.GetComponent<TimerMechanics>().DeductTime(deduction);
    }

}
