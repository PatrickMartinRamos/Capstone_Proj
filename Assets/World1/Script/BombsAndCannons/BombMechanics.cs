using UnityEngine;
using DG.Tweening;

public class BombMechanics : MonoBehaviour
{
    [SerializeField] ProblemType problemType;
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject gameScene;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
    }
    void OpenGameScene()
    {
        gameScene = GetCraftBox();
        if(gameScene != null)
        {
            Instantiate(gameScene);
            gameScene.transform.DOScale(0, 0f);
            gameScene.SetActive(true);
            gameScene.transform.DOScale(1.3f, 0.5f);
            gameScene.transform.DOScale(1f, 1f);
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
