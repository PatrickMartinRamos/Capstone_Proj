using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class GridNavigator : MonoBehaviour
{
    [Header("Swipe Settings")]
    [SerializeField] private float minSwipeDistance = 25f;
    [SerializeField] private GraphicRaycaster raycaster;
    [SerializeField] private EventSystem eventSystem;

    [Header("Text UI")]
    [SerializeField] private TextMeshProUGUI xCoordinateText;
    [SerializeField] private TextMeshProUGUI yCoordinateText;

    // Coordinates
    private int xCoordinate = 0;
    private int yCoordinate = 0;

    private bool validSwipeTarget = false;
    private bool isXWheel = false;
    private bool isYWheel = false;

    private void Update()
    {
        SwipeAction();
    }

    private void SwipeAction()
    {
        // Detect UI at touch start
        if (InputManager.Instance.IsTouching())
        {
            validSwipeTarget = IsTouchOnUI();
        }

        // Process swipe
        if (InputManager.Instance.TouchEnded())
        {
            if (validSwipeTarget)
            {
                DetectSwipe();
            }
            validSwipeTarget = false;
        }
    }

    private bool IsTouchOnUI()
    {
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = InputManager.Instance.StartTouchPosition();

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag("SwipeUI"))
            {
                //Debug.Log("Wheel name: " + result.gameObject.name);
                if (result.gameObject.name.Contains("xWheel"))
                {
                    isXWheel = true;
                    isYWheel = false;
                }
                else if (result.gameObject.name.Contains("yWheel"))
                {
                    isYWheel = true;
                    isXWheel = false;
                }
                else
                {
                    isXWheel = false;
                    isYWheel = false;
                }
                return true;
            }
        }
        return false;
    }

    private void DetectSwipe()
    {
        Vector2 start = InputManager.Instance.StartTouchPosition();
        Vector2 end = InputManager.Instance.EndTouchPosition();
        Vector2 swipeDelta = end - start;

        if (swipeDelta.magnitude < minSwipeDistance) return;

        // Only handle vertical swipes
        if (Mathf.Abs(swipeDelta.y) > Mathf.Abs(swipeDelta.x))
        {
            if (swipeDelta.y > 0)
            {
                // Swipe Up
                if (isXWheel)
                {
                    xCoordinate++;
                    UpdateXText();
                }
                else if (isYWheel)
                {
                    yCoordinate++;
                    UpdateYText();
                }
            }
            else
            {
                // Swipe Down
                if (isXWheel)
                {
                    xCoordinate--;
                    UpdateXText();
                }
                else if (isYWheel)
                {
                    yCoordinate--;
                    UpdateYText();
                }
            }
        }
    }

    private void UpdateXText()
    {
        xCoordinateText.text = $"{xCoordinate}";
    }

    private void UpdateYText()
    {
        yCoordinateText.text = $"{yCoordinate}";
    }

    public int GetXCoordinate()
    {
        return xCoordinate;
    }
    public int GetYCoordinate()
    {
        return yCoordinate;
    }
}
