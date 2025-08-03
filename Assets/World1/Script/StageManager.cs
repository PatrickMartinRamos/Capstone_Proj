using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    [Header("CraftBox")]
    [SerializeField] Vector3 hiddenPos, shownUpPos;
    [Header("GameObjects")]
    public GameObject CraftingBox;
    public GameObject InitialHiddenItems;
    public GameObject Cannon;
    public GameObject Star;
    public GameObject Bullet;
    public GameObject Bomb;

    [Header("UI")]
    public float timeLimit;
    private float time; public float currentTime => time;
    public Slider timer;
    public GameObject CorrectAnswerPanel, WrongAnswerPanel;
    public GameObject HelpPanel;

    public bool ShowCraftBox, craftBoxIsHidden=false;

    public static StageManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (InitialHiddenItems.activeSelf) InitialHiddenItems.SetActive(false);

    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CraftingBox.transform.position = hiddenPos;
        time = timeLimit;
        timer.maxValue = timeLimit;
        timer.value = time;
        timer.gameObject.SetActive(false);
        ShowCraftBox = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (ShowCraftBox)
        {
            if (CraftingBox.transform.position != shownUpPos)
            {
                CraftingBox.transform.position = Vector3.MoveTowards(CraftingBox.transform.position, shownUpPos, 4 * Time.deltaTime);
            }
            if (CraftingBox.transform.position == shownUpPos && !InitialHiddenItems.gameObject.activeSelf)
            {
                timer.gameObject.SetActive(true);
                InitialHiddenItems.SetActive(true);
            }
            if (CraftingBox.transform.position == shownUpPos && InitialHiddenItems.gameObject.activeSelf && !WrongAnswerPanel.activeSelf)
            {
                time = Mathf.Clamp(time - Time.deltaTime, 0, timeLimit);
            }
            timer.value = time;
        }
        else
        {
            if (timer.gameObject.activeSelf) timer.gameObject.SetActive(false);
            if (InitialHiddenItems.activeSelf) InitialHiddenItems.SetActive(false);
            if (CraftingBox.transform.position != hiddenPos)
            {
                CraftingBox.transform.position = Vector3.MoveTowards(CraftingBox.transform.position, hiddenPos, 4 * Time.deltaTime);
            }
            if (CraftingBox.transform.position == hiddenPos) craftBoxIsHidden = true;
        }
        if (craftBoxIsHidden)
        {
            Star.transform.position = Vector3.MoveTowards(Star.transform.position, Cannon.transform.position, 3 * Time.deltaTime);
            if (Star.transform.position == Cannon.transform.position)
            {
                Bullet.SetActive(true); 
                Bullet.transform.position = Vector3.MoveTowards(Bullet.transform.position, Bomb.transform.position, 5*Time.deltaTime);
                if(Bullet.transform.position == Bomb.transform.position)
                {
                    CorrectAnswerPanel.SetActive(true) ;
                }
            }

        } 
            

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
        shapeCombination.Instance.ClearShapeList();
        WrongAnswerPanel.SetActive(false);
    }
    public void RestartStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void NextStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void Home()
    {
        SceneManager.LoadScene(0);

    }
}
