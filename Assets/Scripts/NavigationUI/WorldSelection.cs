using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSelection : MonoBehaviour
{
    [SerializeField] private string worldName;
    [SerializeField] private RectTransform stageSelectionTransform;
    [SerializeField] private float worldRotationSpeed;
    [SerializeField] private bool isUnlocked, isSelected = false;

    private void Update()
    {
        float speed = worldRotationSpeed * Time.deltaTime;
        transform.Rotate(0, 1 * speed, 0);
    }

    public void EnterWorld()
    {
        stageSelectionTransform.DOAnchorPosY(0, 0.2f);
    }
    public string Selected()
    {
        return worldName;
    }
}
