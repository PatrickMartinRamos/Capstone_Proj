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
        Vector2 touchPosition = InputManager.Instance.GetTouchPosition();
        bool isTouching = InputManager.Instance.IsTouching();
       
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
            transform.parent = null;

            if (overlap != null)
            {
                string draggedShapeTag = selectedShape.gameObject.tag;
                string targetShapeTag = overlap.gameObject.tag;
                bool isMatched = false;
                Vector3 spawnPosition = (selectedShape.transform.position + overlap.transform.position) / 2f;

                switch (draggedShapeTag)
                {
                    case "Square":
                        Debug.Log("Matching Square...");
                        isMatched = shapeCombination.Instance.Square(targetShapeTag, spawnPosition);
                        break;
                    case "Circle":
                        Debug.Log("Matching Circle...");
                        isMatched = shapeCombination.Instance.Circle(targetShapeTag, spawnPosition);
                        break;
                    case "Triangle":
                        Debug.Log("Matching Triangle...");
                        isMatched = shapeCombination.Instance.Triangle(targetShapeTag, spawnPosition);
                        break;
                    case "Scissor":
                        Debug.Log("Dividing...");
                        break;
                    default:
                        Debug.Log("Matched unknown shape");
                        break;
                }

                // Return Selected Shape to its original position
                if (isMatched)
                {
                    selectedShape.GetComponent<DraggableShape>().RevertPosition();
                }

                overlap = null;
                selectedShape = null;
                transform.parent = varParent;
            }
        }
    }

    
}

