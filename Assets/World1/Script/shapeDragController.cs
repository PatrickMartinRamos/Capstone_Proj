using UnityEngine;
using UnityEngine.EventSystems;

public class shapeDragController : MonoBehaviour
{
    [SerializeField] private LayerMask shapeLayer;
    [SerializeField] private Transform varParent;

    private GameObject selectedShape;
    private Collider2D overlap;
    private RaycastHit2D hit;

    void Update()
    {
        Vector2 touchPosition = InputManager.Instance.GetTouchPosition();
        bool isTouching = InputManager.Instance.IsTouching();

        // 🛑 Block shape dragging if touching a UI element
        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;

        if (isTouching)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);
            worldPoint.z = 1f; // Ensure shape stays visible in 2D view

            if (selectedShape == null)
            {
                hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, shapeLayer);
                if (hit.collider != null)
                {
                    selectedShape = hit.collider.gameObject;
                }
            }
            else
            {
                // Dragging
                if (selectedShape != null)
                {
                    selectedShape.transform.position = worldPoint;

                    if (hit.collider != null)
                    {
                        overlap = Physics2D.OverlapBox(selectedShape.transform.position, hit.collider.bounds.size, 0);
                        if (overlap != null)
                        {
                            Debug.Log("Overlapping: " + overlap.name);
                        }
                    }

                    Debug.Log("Dragging: " + selectedShape.name);
                }
            }
        }
        else
        {
            // Touch released
            Debug.Log("Released");
            transform.parent = null;

            if (overlap != null && selectedShape != null && overlap.gameObject != selectedShape.gameObject)
            {
                string draggedShapeTag = selectedShape.tag;
                string targetShapeTag = overlap.tag;
                bool isMatched = false;
                Vector3 spawnPosition = (selectedShape.transform.position + overlap.transform.position) / 2f;

                switch (draggedShapeTag)
                {
                    case "Square":
                        Debug.Log("Matching Square...");
                        isMatched = shapeCombination.Instance.Square(overlap.gameObject, spawnPosition);
                        break;
                    case "Circle":
                        Debug.Log("Matching Circle...");
                        isMatched = shapeCombination.Instance.Circle(overlap.gameObject, spawnPosition);
                        break;
                    case "Triangle":
                        Debug.Log("Matching Triangle...");
                        isMatched = shapeCombination.Instance.Triangle(selectedShape, overlap.gameObject, spawnPosition);
                        break;
                    case "Scissor":
                        Debug.Log("Dividing...");
                        break;
                    default:
                        Debug.Log("Matched unknown shape");
                        break;
                }

                if (isMatched)
                {
                    selectedShape.GetComponent<DraggableShape>().RevertPosition();
                }
            }

            // Reset
            overlap = null;
            selectedShape = null;
            transform.parent = varParent;
        }
    }
}
