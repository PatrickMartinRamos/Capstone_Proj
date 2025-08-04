using System;
using CapstoneProj.GameInputSystem;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.ControlPanelSystem
{
    public class CartesianPlaneToggle : SingletonBehaviour<CartesianPlaneToggle>
    {
        public event EventHandler OnToggleable;
        public event EventHandler OnUntoggleable;
        public event EventHandler OnSelected;
        public event EventHandler OnDeselected;

        private const string CARTESIAN_PLANE_TOGGLE_LAYER = "CartesianPlaneToggle";

        [SerializeField] private GameInputs _gameInputs; // Reference to the game inputs.
        [SerializeField] private bool _isSelected = false;
        private Vector2 _mousePosition; // Stores the mouse position.
        private bool _isToggleable = false;

        protected override void Awake()
        {
            base.Awake();

            ResetCartesianPlaneButton();
        }

        private void Start()
        {
            // Initialize the game inputs.
            _gameInputs = GameInputs.Instance;

            // Subscribe to input actions.
            _gameInputs.OnPrimaryInteractStartedAction
                += GameInputs_OnPrimaryInteractStartedAction; // Subscribe to primary interact start action.
            _gameInputs.OnPointerPositionPerformedAction
                += GameInputs_OnPointerPositionPerformedAction; // Subscribe to pointer position action.
        }

        private void OnDestroy()
        {
            // Unsubscribe from input actions to prevent memory leaks.
            if (_gameInputs != null)
            {
                _gameInputs.OnPrimaryInteractStartedAction
                    -= GameInputs_OnPrimaryInteractStartedAction;
                _gameInputs.OnPointerPositionPerformedAction
                    -= GameInputs_OnPointerPositionPerformedAction;
            }
        }

        private void GameInputs_OnPointerPositionPerformedAction(Vector2 vector)
            => _mousePosition = vector;

        private void GameInputs_OnPrimaryInteractStartedAction()
        {
            if (!_isToggleable)
                return;

            Ray ray = Camera.main.ScreenPointToRay(_mousePosition);

            Vector2 origin = ray.origin;
            Vector2 direction = ray.direction;

            int layerMask = LayerMask.GetMask(CARTESIAN_PLANE_TOGGLE_LAYER); // Define the layer mask for the tile layer.

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                _isSelected = !_isSelected; // Toggle the selection state.

                if (_isSelected)
                    OnSelected?.Invoke(this, EventArgs.Empty); // Invoke the selected event.
                else
                    OnDeselected?.Invoke(this, EventArgs.Empty); // Invoke the deselected event.
            }
        }

        public void ResetCartesianPlaneButton()
        {
            _isSelected = false;
            SetIsToggleable(false);

            OnDeselected?.Invoke(this, EventArgs.Empty); // Invoke the deselected event.
        }

        public void SetIsToggleable(bool isToggleable)
        {
            _isToggleable = isToggleable;

            if (isToggleable)
                OnToggleable?.Invoke(this, EventArgs.Empty);
            else
                OnUntoggleable?.Invoke(this, EventArgs.Empty);
        }
    }
}