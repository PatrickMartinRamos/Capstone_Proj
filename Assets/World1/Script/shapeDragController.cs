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
            worldPoint.z = -2f; // Ensure shape stays visible in 2D view

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
                Vector3 spawnPosition = (selectedShape.transform.position + overlap.transform.position) / 2f;

                overlap.gameObject.GetComponent<ITargetable>().InteractWithDraggedObject(selectedShape);
                Debug.Log(overlap.gameObject.name + " \n" + selectedShape.name);
                selectedShape.GetComponent<Shapes>().RevertPosition();

            }

            // Reset
            overlap = null;
            selectedShape = null;
            transform.parent = varParent;
        }
    }
}
