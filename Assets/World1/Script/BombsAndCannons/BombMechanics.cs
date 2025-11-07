using UnityEngine;
using DG.Tweening;

public class BombMechanics : MonoBehaviour
{
    [SerializeField] ProblemType problemType;
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject gameScene;
    private Vector3 gameSceneSpawnPt;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameSceneSpawnPt = StageManager.Instance.gameplaySpawnPt.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SelectBomb()
    {
        Debug.Log("Bomb has been selected.");
        StageManager.Instance.targetBomb = gameObject;
        OpenGameScene();
        this.gameObject.GetComponent<Collider2D>().enabled = false;
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
               .Append(gameScene.transform.DOScale(1.2f, 1f))
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
            gameObject.SetActive(false);
        }
    }
}
