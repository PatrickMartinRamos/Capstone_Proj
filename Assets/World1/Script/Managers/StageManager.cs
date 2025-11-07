using DG.Tweening;
using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    public GameObject star;
    public GameObject bullet;
    public GameObject targetBomb;
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
    private float time; public float currentTime => time;
    public Slider timer;
    public TextMeshProUGUI NotificationText;
    public GameObject CorrectAnswerPanel, WrongAnswerPanel;
    public GameObject HelpPanel;

    public bool ShowCraftBox, craftBoxIsHidden=false, startGame = false;

    public static StageManager Instance { get; private set; }
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
        targetBomb.GetComponent<Collider2D>().enabled = true;
        ActiveGameArea.SetActive(false);
        bullet.SetActive(true);
    }
    public void OpenBulletCase()
    {
        bulletCase.transform.DOLocalMoveX(-6.6f, 0.2f, true);
        bullet.transform.SetParent(cannon.transform);
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
