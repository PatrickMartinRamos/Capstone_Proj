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
/*        Debug.Log($"{name} pressed");
*/    }

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
            uiShape.InteractWithDraggedObject(overlap.gameObject);
        }
        else
        {
            // No overlap → revert to start
            uiShape.RevertPosition();
        }
    }

    // Find overlapping draggable shapes in same parent
    UIShapeDraggableButton GetOverlappingShape()
    {
        var others = transform.parent.parent.GetComponentsInChildren<UIShapeDraggableButton>();
        Rect myRect = GetScreenRect(rectTransform);

        UIShapeDraggableButton bestMatch = null;
        float largestArea = 0f;

        foreach (var other in others)
        {
            if (other == this)
                continue;

            Rect otherRect = GetScreenRect(other.rectTransform);

            // If no overlap, skip
            if (!myRect.Overlaps(otherRect))
                continue;

            // Compute intersection rectangle
            float xMin = Mathf.Max(myRect.xMin, otherRect.xMin);
            float xMax = Mathf.Min(myRect.xMax, otherRect.xMax);
            float yMin = Mathf.Max(myRect.yMin, otherRect.yMin);
            float yMax = Mathf.Min(myRect.yMax, otherRect.yMax);

            float overlapWidth = Mathf.Max(0, xMax - xMin);
            float overlapHeight = Mathf.Max(0, yMax - yMin);

            float area = overlapWidth * overlapHeight;

            // Pick the one with the biggest area
            if (area > largestArea)
            {
                largestArea = area;
                bestMatch = other;
            }
        }

        return bestMatch;
    }


    Rect GetScreenRect(RectTransform rt)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);

        return new Rect(
            corners[0].x,
            corners[0].y,
            corners[2].x - corners[0].x,
            corners[2].y - corners[0].y
        );
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
