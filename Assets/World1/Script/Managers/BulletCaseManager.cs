using DG.Tweening;
using UnityEngine;

public class BulletCaseManager : MonoBehaviour
{
    [SerializeField] GameObject bulletPlaceholder;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StageManager.CloseGameAreaEvent.AddListener(OpenCase);
    }

    void OpenCase()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(-6f, 0.5f, true));
        seq.Append(bulletPlaceholder.transform.DOMove(StageManager.Instance.bullet.transform.position, 1f));
        seq.Join(bulletPlaceholder.transform.DOScale(Vector3.zero, 1f));
        
    }
}
