using UnityEngine;

public class shapeDragController : MonoBehaviour
{
    [SerializeField] private LayerMask shapeLayer;
    [SerializeField] private Transform varParent;
    private GameObject selectedShape;
    private Collider2D overlap;
    private RaycastHit2D hit;
    void Update()
    {
        Vector2 touchPosition = shapeInputController.Instance.GetTouchPosition();
        bool isTouching = shapeInputController.Instance.IsTouching();
       
        if (isTouching)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);
            worldPoint.z = 1; //make the order layer of sprites to >3 to prevent shapes going behind other sprites
            if (selectedShape == null)
            {
                hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, shapeLayer);
                selectedShape = hit.collider.gameObject;
            }
            else
            {
                // Dragging
                if (hit.collider)
                {
                    overlap = Physics2D.OverlapBox(selectedShape.transform.position, hit.collider.bounds.size, 0);
                    selectedShape.transform.position = worldPoint;
                    Debug.Log(selectedShape, selectedShape);
                    if (overlap != null)
                    {
                        Debug.Log(overlap.name, overlap);
                    }
                    //Debug.Log("Shape selected: " + selectedShape.gameObject.name);
                }
            }
        }
        else
        {
            // Touch released
            Debug.Log("Released");
            Debug.Log(overlap);
            transform.parent = null;

            if (overlap != null)
            {
                //check if both shapes has a combination
                if (selectedShape.gameObject.tag == overlap.gameObject.tag)
                {
                    Debug.Log("Set triangle");
                }
            }
            overlap = null;
            selectedShape = null;
            transform.parent = varParent;
        }
    }

    
}

