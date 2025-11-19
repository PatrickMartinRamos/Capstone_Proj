using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPressDragDetector : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler,
    IBeginDragHandler, IEndDragHandler
{
    public bool isPressed = false;
    public bool isDragging = false;

    private Vector2 pointerOffset;

    RectTransform rectTransform;
    RectTransform parentRect;
    Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = transform.parent.GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        Debug.Log("Button pressed");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        isDragging = false;
        Debug.Log("Button released");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        Debug.Log("Started dragging on button");

        // Record the offset between the finger and the center of the button
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pointerOffset
        );

        pointerOffset = rectTransform.anchoredPosition - pointerOffset;
        this.gameObject.transform.SetAsLastSibling();
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
        Debug.Log("Stopped dragging on button");
        CheckOverlap();
    }

    void CheckOverlap()
    {
        // Find all other buttons in the parent
        ButtonPressDragDetector[] others = transform.parent.GetComponentsInChildren<ButtonPressDragDetector>();

        foreach (var other in others)
        {
            if (other == this)
                continue;

            if (IsOverlapping(rectTransform, other.rectTransform))
            {
                Debug.Log($"{name} overlapped with {other.name}");
            }
        }
    }

    bool IsOverlapping(RectTransform a, RectTransform b)
    {
        Rect rectA = GetScreenRect(a);
        Rect rectB = GetScreenRect(b);

        return rectA.Overlaps(rectB);
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
        // Button world corners
        Vector3[] buttonCorners = new Vector3[4];
        rectTransform.GetWorldCorners(buttonCorners);

        // Parent world corners
        Vector3[] parentCorners = new Vector3[4];
        parentRect.GetWorldCorners(parentCorners);

        Vector3 pos = rectTransform.position;

        // LEFT boundary
        if (buttonCorners[0].x < parentCorners[0].x)
            pos.x += parentCorners[0].x - buttonCorners[0].x;

        // RIGHT boundary
        if (buttonCorners[2].x > parentCorners[2].x)
            pos.x -= buttonCorners[2].x - parentCorners[2].x;

        // BOTTOM boundary
        if (buttonCorners[0].y < parentCorners[0].y)
            pos.y += parentCorners[0].y - buttonCorners[0].y;

        // TOP boundary
        if (buttonCorners[1].y > parentCorners[1].y)
            pos.y -= buttonCorners[1].y - parentCorners[1].y;

        rectTransform.position = pos;
    }
}
