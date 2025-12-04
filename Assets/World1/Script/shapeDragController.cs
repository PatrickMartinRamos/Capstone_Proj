using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

public class shapeDragController : MonoBehaviour
{
    [SerializeField] private LayerMask shapeLayer;
    //[SerializeField] private Transform varParent;

    private GameObject selectedShape;
    private Collider2D overlap;
    private RaycastHit2D hit;
    private bool canInteract = true;
    bool triggerHighlight = false;
    private List<Material> mats = new();

    void Update()
    {
        Vector2 touchPosition = InputManager.Instance.GetTouchPosition();
        bool isTouching = InputManager.Instance.IsTouching();

        if (Input.touchCount > 0 && EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            return;

        if (isTouching)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);
            worldPoint.z = -5f; // Ensure shape stays visible in 2D view

            if (selectedShape == null)
            {
                hit = Physics2D.Raycast(worldPoint, Vector2.zero, Mathf.Infinity, shapeLayer);
                if (hit.collider != null)
                {
                    selectedShape = hit.collider.gameObject;
                    Shapes shape = selectedShape.GetComponent<Shapes>();

                    if (!shape.isGiven || selectedShape.GetComponent<Scissors>()!=null || selectedShape.name == "Rooter")
                    {
                        Debug.Log("Item cannot change origin pos.");
                        selectedShape.GetComponent<Shapes>().ChangeOriginPos(selectedShape.transform.localPosition);

                    }
                    List<Material>mats = shape.materials;
                    if(mats.Count != 0 && triggerHighlight == false)
                    {
                        foreach (var item in mats)
                        {
                            item.SetFloat("_Speed", 0);
                        }
                        triggerHighlight = true;
                    }

                }
            }
            else
            {
                // Dragging
                if (selectedShape != null && selectedShape.GetComponent<Shapes>().isMovable)
                {
                    canInteract = true;
                    selectedShape.GetComponent<Collider2D>().enabled = false;
                    //varParent = selectedShape.transform.parent;
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
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(touchPosition);
            worldPoint.z = 0;
            // Touch released
            //transform.parent = null;
            if (selectedShape != null)
            {
                if (triggerHighlight == true)
                {
                    mats = selectedShape.GetComponent<Shapes>().materials;
                    foreach (var item in mats)
                    {
                        item.SetFloat("_Speed", 1);
                    }
                    triggerHighlight = false;
                }

                selectedShape.GetComponent<Collider2D>().enabled = true;

                if (overlap != null && overlap.gameObject.GetComponent<ITargetable>() != null && overlap.gameObject != selectedShape.gameObject)
                {
                    if (canInteract)
                    overlap.gameObject.GetComponent<ITargetable>().InteractWithDraggedObject(selectedShape);
                    canInteract = false;
                }

                // Reset Pos
                else if (overlap == null || overlap.gameObject.GetComponent<ITargetable>() == null)
                {
                    selectedShape.GetComponent<Shapes>().RevertPosition();
                }

            }

            if (triggerHighlight == true)
            {
                foreach (var item in mats)
                {
                    item.SetFloat("_Speed", 1);
                }
                triggerHighlight = false;
            }

            // Reset Drag Controller
            overlap = null;
            selectedShape = null;
            //transform.parent = varParent;
        }
    }
}
