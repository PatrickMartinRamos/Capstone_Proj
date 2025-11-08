using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class BombMechanics : MonoBehaviour
{
    [SerializeField] ProblemType problemType;
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject gameScene;
    private Vector3 gameSceneSpawnPt;
    private bool canTargetBomb = true, isNeutralized = false;

    public static UnityEvent OpenGameAreaEvent = new UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StageManager.Instance.StageBombs.Add(gameObject);
        gameSceneSpawnPt = StageManager.Instance.gameplaySpawnPt.transform.position;
        OpenGameAreaEvent.AddListener(DisableTargetting);
        StageManager.CloseGameAreaEvent.AddListener(EnableTargetting);
    }

    // Update is called once per frame
    void Update()
    {
        if ((isNeutralized && !explosionFX.activeInHierarchy))
        {
            gameObject.SetActive(false);
        }
    }
    void DisableTargetting()
    {
        canTargetBomb = false;
    }
    void EnableTargetting()
    {
        canTargetBomb = true;
    }
    public void SelectBomb()
    {
        if (!canTargetBomb) return;
        Debug.Log("Bomb has been selected.");
        StageManager.Instance.targetBomb = gameObject;
        OpenGameAreaEvent.Invoke();
        OpenGameScene();
    }
    void OpenGameScene()
    {
        GameObject prefab = GetCraftBox();
        if (prefab != null)
        {
            gameScene = Instantiate(prefab, gameSceneSpawnPt, Quaternion.identity);
            gameScene.transform.SetParent(StageManager.Instance.gameplaySpawnPt.transform, worldPositionStays: false);
            gameScene.transform.localScale = Vector3.zero;
            gameScene.SetActive(true);
            StageManager.Instance.ActiveGameArea = gameScene;

            // Create sequence properly
            Sequence seq = DOTween.Sequence();
            seq.Append(gameScene.transform.DOScale(1.4f, 0.3f))
               .Append(gameScene.transform.DOScale(1.1f, 1f))
               .OnComplete(() => gameScene.GetComponent<ProblemLoader>().LoadProblem());
        }
    }

    GameObject GetCraftBox()
    {
        switch(problemType)
        {
            case ProblemType.squaringBinomial:
                return StageManager.Instance.squaringBinomialBox;
            case ProblemType.radicals:
                return StageManager.Instance.radicalsBox;
            case ProblemType.completingSquare:
                return StageManager.Instance.completingSquareBox;
        }
        return null;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Something Hit Bomb.");
        if (collision != null && collision.gameObject.name == "Bullet")
        {
            explosionFX.SetActive(true);
            isNeutralized = true;
            collision.gameObject.SetActive(false);
        }
    }
}
