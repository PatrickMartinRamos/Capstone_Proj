using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class BombMechanics : MonoBehaviour
{
    [SerializeField] public ProblemType problemType;
    [SerializeField] GameObject explosionFX, bigExplosionFX, inactiveFX;
    [SerializeField] GameObject gameScene;
    private BombsManager bombsManager;
    private Vector3 gameSceneSpawnPt;
    private bool canTargetBomb = true, isNeutralized = false, isRegistered;
    public bool bombExploded = false;

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
        if (!canTargetBomb || isNeutralized) return;

        // selection prompt
        Debug.Log("Bomb has been selected.");

        // targetBomb setting
        bombsManager.setTargetBomb(this.gameObject);
        StageManager.Instance.cannon.GetComponent<CannonMechanics>().AimAt(this.gameObject.transform.position);

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
               .Append(gameScene.transform.DOScale(1f, 1f))
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
            case ProblemType.advanceCompletingSquare:
                return StageManager.Instance.advCompletingSquareBox;
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
        StartCoroutine(WaitForExplosionToFinish(explosionFX.GetComponent<ParticleSystem>()));        
    }

    public void ExplodeBomb()
    {
        StageManager.Instance.ActiveGameArea.SetActive(false);
        bigExplosionFX.SetActive(true);
        StartCoroutine(WaitForBigExplosionToFinish(bigExplosionFX.GetComponent<ParticleSystem>()));
        bombExploded = true;

    }
    IEnumerator WaitForExplosionToFinish(ParticleSystem ps)
    {
        // Wait until the particle system is completely done
        yield return new WaitUntil(() => !ps.IsAlive(true));

        isNeutralized = true;
        bombsManager.AddNeutralized();
        gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
        inactiveFX.SetActive(true);
    }
    IEnumerator WaitForBigExplosionToFinish(ParticleSystem ps)
    {
        // Wait until the particle system is completely done
        yield return new WaitUntil(() => !ps.IsAlive(true));

        StageManager.Instance.WrongAnswerPanel.SetActive(true);
    }
}
