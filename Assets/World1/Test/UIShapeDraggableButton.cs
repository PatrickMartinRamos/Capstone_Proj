using UnityEngine;
using UnityEngine.EventSystems;

public class UIShapeDraggableButton : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler,
    IBeginDragHandler, IEndDragHandler
{
    public bool isPressed = false;
    public bool isDragging = false;

    private Vector2 pointerOffset;

    RectTransform rectTransform;
    RectTransform parentRect;
    Canvas canvas;

    UIShape uiShape;      // reference to shape logic
    Vector2 dragStartPos; // store original pos for revert

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.parent.GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        uiShape = GetComponent<UIShape>();   // << IMPORTANT
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        Debug.Log($"{name} pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;

        // If it was dragged and released NOT over another shape → revert
        if (!isDragging)
        {
            uiShape.RevertPosition();
        }

        isDragging = false;
        //Debug.Log($"{name} released");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        // Save starting position for revert
        dragStartPos = rectTransform.anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pointerOffset
        );

        pointerOffset = rectTransform.anchoredPosition - pointerOffset;

        // Bring to front
        this.transform.SetAsLastSibling();

        //Debug.Log($"{name} started dragging");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPos;

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out worldPos))
        {
            rectTransform.position = worldPos;
            ClampToParent();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        //Debug.Log($"{name} stopped dragging");

        // Check for overlap
        UIShapeDraggableButton overlap = GetOverlappingShape();

        if (overlap != null)
        {
            //Debug.Log($"{name} overlapped {overlap.name}");

            // Combine shapes
            overlap.gameObject.GetComponent<UIShape>().InteractWithDraggedObject(this.gameObject);
        }
        else
        {
            // No overlap → revert to start
            uiShape.RevertPosition();
        }
    }

    public float radiusOffset = 0.2f;  // shrink the detection circle


    // Find overlapping draggable shapes in same parent
    UIShapeDraggableButton GetOverlappingShape()
    {
        var others = transform.parent.parent.GetComponentsInChildren<UIShapeDraggableButton>();

        Vector2 myCenter = GetScreenCenter(rectTransform);
        float myRadius = Mathf.Max(0, GetScreenRadius(rectTransform) - radiusOffset);

        UIShapeDraggableButton bestMatch = null;
        float bestOverlap = 0f;

        foreach (var other in others)
        {
            if (other == this)
                continue;

            Vector2 otherCenter = GetScreenCenter(other.rectTransform);
            float otherRadius = Mathf.Max(0, GetScreenRadius(other.rectTransform) - radiusOffset);

            float distance = Vector2.Distance(myCenter, otherCenter);
            float combined = myRadius + otherRadius;

            if (distance > combined)
                continue; // ❌ no circle overlap

            // Overlap amount (bigger = stronger match)
            float overlap = combined - distance;

            if (overlap > bestOverlap)
            {
                bestOverlap = overlap;
                bestMatch = other;
            }
        }

        return bestMatch;
    }



    Vector2 GetScreenCenter(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        // Convert to screen space
        Vector2 min = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 max = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        return (min + max) * 0.5f; // midpoint
    }

    float GetScreenRadius(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        Vector2 min = RectTransformUtility.WorldToScreenPoint(null, corners[0]);
        Vector2 max = RectTransformUtility.WorldToScreenPoint(null, corners[2]);

        // Use half of the smallest dimension to approximate circle
        float width = max.x - min.x;
        float height = max.y - min.y;

        return Mathf.Min(width, height) * 0.5f;
    }


    private void ClampToParent()
    {
        Vector3[] buttonCorners = new Vector3[4];
        rectTransform.GetWorldCorners(buttonCorners);

        Vector3[] parentCorners = new Vector3[4];
        parentRect.GetWorldCorners(parentCorners);

        Vector3 pos = rectTransform.position;

        if (buttonCorners[0].x < parentCorners[0].x)
            pos.x += parentCorners[0].x - buttonCorners[0].x;

        if (buttonCorners[2].x > parentCorners[2].x)
            pos.x -= buttonCorners[2].x - parentCorners[2].x;

        if (buttonCorners[0].y < parentCorners[0].y)
            pos.y += parentCorners[0].y - buttonCorners[0].y;

        if (buttonCorners[1].y > parentCorners[1].y)
            pos.y -= buttonCorners[1].y - parentCorners[1].y;

        rectTransform.position = pos;
    }
}
