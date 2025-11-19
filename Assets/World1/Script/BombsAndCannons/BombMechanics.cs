using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class BombMechanics : MonoBehaviour
{
    [SerializeField] ProblemType problemType;
    [SerializeField] GameObject explosionFX;
    [SerializeField] GameObject gameScene;
    private BombsManager bombsManager;
    private Vector3 gameSceneSpawnPt;
    private bool canTargetBomb = true, isNeutralized = false, isRegistered;

    public static UnityEvent OpenGameAreaEvent = new UnityEvent();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bombsManager = StageManager.Instance.bombsManager;
        if (bombsManager!=null) isRegistered = bombsManager.RegisterBomb(this.gameObject);
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
        // return if bomb cannot be targetted
        if (!canTargetBomb) return;

        // selection prompt
        Debug.Log("Bomb has been selected.");

        // targetBomb setting
        bombsManager.setTargetBomb(this.gameObject);
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
            DestroyBomb(collision.gameObject);
        }
    }
    private void DestroyBomb(GameObject bullet)
    {
        explosionFX.SetActive(true);
        bullet.SetActive(false);
        StartCoroutine(WaitForExplosionToFinish());        
    }
    IEnumerator WaitForExplosionToFinish()
    {
        ParticleSystem ps = explosionFX.GetComponent<ParticleSystem>();

        // Wait until the particle system is completely done
        yield return new WaitUntil(() => !ps.IsAlive(true));

        isNeutralized = true;
        bombsManager.AddNeutralized();
    }
}
