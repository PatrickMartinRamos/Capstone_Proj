using DG.Tweening;
using UnityEngine;

public class BulletMechanics : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Vector3 initLocalPos;
    private Tween moveTween;

    private void Awake()
    {
        initLocalPos = transform.localPosition;
    }
    private void Start()
    {
        StageManager.Instance.bullet = this.gameObject;
    }

    private void OnEnable()
    {
        target = StageManager.Instance.targetBomb;
        MoveToTarget();
    }

    private void OnDisable()
    {
        moveTween?.Kill(); // Stop any ongoing tween
        transform.localPosition = initLocalPos; // Reset to start
        transform.rotation = Quaternion.identity; // Optional: reset rotation
    }

    private void MoveToTarget()
    {
        if (target == null) return;

        Vector3 targetPos = target.transform.position;

        // Rotate to face target
        Vector3 direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Move once (not every frame)
        moveTween = transform.DOMove(targetPos, 2f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }
}
