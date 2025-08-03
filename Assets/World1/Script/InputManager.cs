using CapstoneProj.GameInputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    private InputSystem_Actions inputActions;
    private Vector2 touchPos;
    private bool isTouching;
    
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool touchEnded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.TouchControls.Touch.started += ctx =>
        {
            isTouching = true;
            touchEnded = false;
            startTouchPos = inputActions.TouchControls.Swipe.ReadValue<Vector2>();
        };

        inputActions.TouchControls.Touch.canceled += ctx =>
        {
            isTouching = false;
            endTouchPos = inputActions.TouchControls.Swipe.ReadValue<Vector2>();
            touchEnded = true;
        };

        inputActions.TouchControls.Drag.performed += ctx => touchPos = ctx.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        inputActions.TouchControls.Touch.started -= ctx => isTouching = true;
        inputActions.TouchControls.Touch.canceled -= ctx => isTouching = false;

        inputActions.Disable();
    }

    public bool  ResetTouch()
    {
        if (touchEnded)
        {
            touchEnded = false; 
            return true;
        }
    return false;
    }

    public Vector2 GetTouchPosition() => touchPos;
    public Vector2 StartTouchPosition() => startTouchPos;
    public Vector2 EndTouchPosition() => endTouchPos;
    public bool IsTouching() => isTouching;
    public bool TouchEnded() => touchEnded;
}
