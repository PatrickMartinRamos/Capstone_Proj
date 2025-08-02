using UnityEngine;

public class shapeInputController : MonoBehaviour
{
    public static shapeInputController Instance { get; private set; }

    private InputSystem_Actions inputActions;
    private Vector2 touchPos;
    private bool isTouching;

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

        inputActions.TouchControls.Touch.started += ctx => isTouching = true;
        inputActions.TouchControls.Touch.canceled += ctx => isTouching = false;

        inputActions.TouchControls.Drag.performed += ctx => touchPos = ctx.ReadValue<Vector2>();
    }

    private void OnDisable()
    {
        inputActions.TouchControls.Touch.started -= ctx => isTouching = true;
        inputActions.TouchControls.Touch.canceled -= ctx => isTouching = false;

        inputActions.Disable();
    }

    public Vector2 GetTouchPosition() => touchPos;
    public bool IsTouching()
    {
        return isTouching;
    }
}
