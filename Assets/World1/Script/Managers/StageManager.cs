using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [SerializeField] private int stageNumber;
    [SerializeField] private AlgebraicFoundationPlayerStatus playerStatus;

    [Header("CraftBoxes")]
    [SerializeField] public GameObject squaringBinomialBox;
    [SerializeField] public GameObject radicalsBox;
    [SerializeField] public GameObject completingSquareBox;

    [Header("GameObjects")]
    public GameObject craftingBox;
    public GameObject initialHiddenItems;
    public GameObject cannon;
    public GameObject star;
    public GameObject bullet;
    public GameObject targetBomb;

    [Header("ShapesList")]
    public GameObject variable;
    public GameObject constant;
    public GameObject squaredVariable;
    public GameObject squaredConstant;
    public GameObject product;


    [Header("UI")]
    public float timeLimit;
    private float time; public float currentTime => time;
    public Slider timer;
    public TextMeshProUGUI NotificationText;
    public GameObject CorrectAnswerPanel, WrongAnswerPanel;
    public GameObject HelpPanel;

    public bool ShowCraftBox, craftBoxIsHidden=false, startGame = false;

    [Header("JSON Manager")]
    [SerializeField] private PlayerStatusJSONManager playerStatusJSONManager;

    public static StageManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (initialHiddenItems.activeSelf) initialHiddenItems.SetActive(false);

    }
/*    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Setting Player Data Current Stage
        playerStatus.currentStage = stageNumber;
        craftingBox.transform.position = hiddenPos;
        time = timeLimit;
        timer.maxValue = timeLimit;
        timer.value = time;
        timer.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (ShowCraftBox)
        {
            if (craftingBox.transform.position != shownUpPos)
            {
                craftingBox.transform.position = Vector3.MoveTowards(craftingBox.transform.position, shownUpPos, introTransSpeed * Time.deltaTime);
            }
            if (craftingBox.transform.position == shownUpPos && !initialHiddenItems.gameObject.activeSelf)
            {
                timer.gameObject.SetActive(true);
                initialHiddenItems.SetActive(true);
            }
            if (craftingBox.transform.position == shownUpPos && initialHiddenItems.gameObject.activeSelf && !WrongAnswerPanel.activeSelf)
            {
                time = Mathf.Clamp(time - Time.deltaTime, 0, timeLimit);
            }
            timer.value = time;
        }
        else
        {
            if (timer.gameObject.activeSelf) timer.gameObject.SetActive(false);
            if (initialHiddenItems.activeSelf) initialHiddenItems.SetActive(false);
            if (craftingBox.transform.position != hiddenPos)
            {
                craftingBox.transform.position = Vector3.MoveTowards(craftingBox.transform.position, hiddenPos, 4 * Time.deltaTime);
            }
            if (craftingBox.transform.position == hiddenPos) craftBoxIsHidden = true;
        }

        if (craftBoxIsHidden && startGame)
        {
            craftingBox.transform.position = Vector3.MoveTowards(craftingBox.transform.position, cannon.transform.position, 3 * Time.deltaTime);
            if (star.transform.position == cannon.transform.position)
            {
                bullet.SetActive(true); 
                bullet.transform.position = Vector3.MoveTowards(bullet.transform.position, targetBomb.transform.position, 5*Time.deltaTime);
                if(bullet.transform.position == targetBomb.transform.position)
                {
                    CorrectAnswerPanel.SetActive(true) ;
                }
            }

        } 
            

    }*/
    public void StartGame()
    {
        ShowCraftBox = true;
        craftBoxIsHidden = false;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        playerStatusJSONManager.UpdatePlayerStatus();
    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
}
