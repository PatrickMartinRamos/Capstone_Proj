using UnityEngine;

public class shapeDragController : MonoBehaviour
{
    [SerializeField] private LayerMask shapeLayer;

    private GameObject selectedShape;

    void Update()
    {

        Vector2 touchPosition = shapeInputController.Instance.GetTouchPosition();
        bool isTouching = shapeInputController.Instance.IsTouching();
        //make the order layer of sprites to >1 so it wont appear behind other sprites
        if (isTouching)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);

            if (selectedShape == null)
            {
                RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, shapeLayer);
                if (hit.collider)
                {
                    selectedShape = hit.collider.gameObject;
                    Debug.Log("Shape selected: " + selectedShape.name);
                }
            }
            else
            {
                // Dragging
                selectedShape.transform.position = worldPoint;
            }
        }
        else
        {
            // Touch released
            selectedShape = null;
        }
    }
}

