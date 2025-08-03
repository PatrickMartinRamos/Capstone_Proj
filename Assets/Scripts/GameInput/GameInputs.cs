using System;
using CapstoneProj.MiscSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CapstoneProj.GameInputSystem
{
    public class GameInputs : SingletonBehaviour<GameInputs>, InputSystem_Actions.ITouchControlsActions
    {
        public Action<Vector2> OnDragPerformedAction; // Action to invoke when dragging.
        public Action<Vector2> OnPointerPositionPerformedAction; // Action to invoke when pointer position changes.
        public Action OnPrimaryInteractStartedAction; // Action to invoke when primary interaction starts.
        public Action<Vector2> OnSwipePerformedAction; // Action to invoke when swiping.
        public Action OnTouchStartedAction; // Action to invoke when touch starts.

        private InputSystem_Actions _inputSystemActions; // Source code representation of asset.
        private InputSystem_Actions.TouchControlsActions _touchControlsActions; // Source code representation of action map.

        protected override void Awake()
        {
            base.Awake();

            _inputSystemActions = new InputSystem_Actions(); // Create asset object.

            _touchControlsActions = _inputSystemActions.TouchControls; // Extract action map object.
            _touchControlsActions.AddCallbacks(this); // Register callback interface ITouchControlsActions.
        }

        private void OnDestroy()
            => _inputSystemActions.Dispose(); // Destroy asset object.

        private void OnEnable()
            => _touchControlsActions.Enable(); // Enable all actions within touch controls.

        private void OnDisable()
            => _touchControlsActions.Disable(); // Disable all actions within touch controls.

        public void OnDrag(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnDragPerformedAction?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPointerPosition(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnPointerPositionPerformedAction?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPrimaryInteract(InputAction.CallbackContext context)
        {
            if (context.started)
                OnPrimaryInteractStartedAction?.Invoke();
        }

        public void OnSwipe(InputAction.CallbackContext context)
        {
            if (context.performed)
                OnSwipePerformedAction?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnTouch(InputAction.CallbackContext context)
        {
            if (context.started)
                OnTouchStartedAction?.Invoke();
        }
    }
}