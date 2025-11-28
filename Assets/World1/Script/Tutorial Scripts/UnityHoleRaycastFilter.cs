using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIHoleRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    public Vector2 holeCenter = new Vector2(0.5f, 0.5f); // normalized 0–1
    public float holeRadius = 0.2f; // normalized 0–1

    private RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public bool IsRaycastLocationValid(Vector2 screenPos, Camera eventCamera)
    {
        Vector2 local;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPos, eventCamera, out local);
        Vector2 normalized = Rect.PointToNormalized(rect.rect, local);

        float dist = Vector2.Distance(normalized, holeCenter);

        // Inside hole → NOT a valid raycast → UI behind gets it
        if (dist < holeRadius)
            return false;

        // Outside hole → block
        return true;
    }
}
