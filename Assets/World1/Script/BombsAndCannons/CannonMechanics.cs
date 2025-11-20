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
    public void GetShell()
    {

    }

}
