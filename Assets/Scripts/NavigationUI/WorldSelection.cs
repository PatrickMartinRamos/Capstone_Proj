using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// Handles the selection and rotation of worlds in the navigation UI.
/// </summary>
public class WorldSelection : MonoBehaviour
{
    [SerializeField] private string worldName;
    [SerializeField] private RectTransform stageSelectionTransform;
    [SerializeField] private float worldRotationSpeed;

    public List<Transform> regions;

    public List<Transform> GetRegions()
    {
        return regions;
    }
    private void Update()
    {
        float speed = worldRotationSpeed * Time.deltaTime;
        transform.Rotate(0, 1 * speed, 0);
    }

    public void OpenWorldStage(RectTransform stageTransform)
    {
        stageSelectionTransform.DOAnchorPosY(945, 0.2f);
    }

    public void CloseWorldStage(RectTransform stageTransform)
    {
        stageSelectionTransform.DOAnchorPosY(-800, 0.2f);
    }

    public RectTransform OpenRectTransform()
    {
        return stageSelectionTransform;
    }

    public string worldNameGet()
    {
        return worldName;
    }
    public string Selected()
    {
        return worldName;
    }
}
