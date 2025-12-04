using DG.Tweening;
using UnityEngine;

public class BulletMechanics : MonoBehaviour
{
    [SerializeField] private GameObject target;
    private Vector3 initLocalPos;
    private Tween roTween, moveTween;

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
        target = StageManager.Instance.bombsManager.targetBomb;
        MoveToTarget();
    }

    private void OnDisable()
    {
        roTween?.Kill();
        moveTween?.Kill(); // Stop any ongoing tween
        transform.localPosition = initLocalPos; // Reset to start
        transform.rotation = Quaternion.identity; // Optional: reset rotation
    }
    [SerializeField] private Transform rotatingChild;

    private void MoveToTarget()
    {
        rotatingChild = GetComponentInChildren<Transform>();
        if (target == null) return;

        Vector3 targetPos = target.transform.position;

        // Face target
        Vector3 direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 🔁 Endless spin (clockwise)
        roTween = rotatingChild
            .DOLocalRotate(new Vector3(0, 0, -360), 0.5f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1);

        // 📌 Move the shell
        moveTween = transform.DOMove(targetPos, 1f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
    }

}
