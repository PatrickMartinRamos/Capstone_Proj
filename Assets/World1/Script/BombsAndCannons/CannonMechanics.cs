using DG.Tweening;
using UnityEngine;

public class CannonMechanics : MonoBehaviour
{
    [SerializeField] GameObject cannon;
    [SerializeField] GameObject cannonShell;
    [SerializeField] GameObject cannonShellCase;
    [SerializeField] Sprite STBshell, Rshell, CTSshell;

    [Header("Aiming Settings")]
    [SerializeField] private float rotationDuration = 0.5f; // time it takes to rotate
    [SerializeField] private Ease rotationEase = Ease.OutBack; // optional tween style

    private Tween rotationTween;
    public void Start()
    {
        StageManager.Instance.cannon = this.gameObject;
        cannonShell = this.gameObject.transform.GetChild(0).gameObject;
    }
    public void AimAt(Vector3 targetPos)
    {
        // Stop any current rotation tween
        rotationTween?.Kill();

        // Calculate direction and angle
        Vector2 direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        // Create rotation tween
        rotationTween = transform
            .DORotate(new Vector3(0, 0, angle), rotationDuration)
            .SetEase(rotationEase);
    }

    /// <summary>
    /// Instantly reset the cannon to its default rotation.
    /// </summary>
    public void ResetAim()
    {
        rotationTween?.Kill();
        transform.rotation = Quaternion.identity;
    }

    public void LauchCannon()
    {

    }
    public void SwitchShell(ProblemType p)
    {
        switch (p)
        {
            case ProblemType.squaringBinomial:
                cannonShell.GetComponentInChildren<SpriteRenderer>().sprite = STBshell;
                break;
            case ProblemType.radicals:
                cannonShell.GetComponentInChildren<SpriteRenderer>().sprite = Rshell;
                break;
            case ProblemType.completingSquare:
                cannonShell.GetComponentInChildren<SpriteRenderer>().sprite = CTSshell;
                break;
            default:
                break;
        }
    }

}
