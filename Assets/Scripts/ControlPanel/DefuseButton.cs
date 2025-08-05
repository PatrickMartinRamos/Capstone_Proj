using System;
using CapstoneProj.GameInputSystem;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.ControlPanelSystem
{
    public class DefuseButton : SingletonBehaviour<DefuseButton>
    {
        public event EventHandler OnInteractable;
        public event EventHandler OnNotInteractable;
        public event EventHandler OnClick;
        public event EventHandler OnCorrectClick;
        public event EventHandler OnIncorrectClick;

        private const string DEFUSE_BUTTON_LAYER = "DefuseButton";

        [SerializeField] private GameInputs _gameInputs; // Reference to the game inputs.
        private Vector2 _mousePosition; // Stores the mouse position.
        private bool _isInteractale = false;

        protected override void Awake()
        {
            base.Awake();

            ResetDefuseButton();
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
            if (!_isInteractale)
                return;

            Ray ray = Camera.main.ScreenPointToRay(_mousePosition);

            Vector2 origin = ray.origin;
            Vector2 direction = ray.direction;

            int layerMask = LayerMask.GetMask(DEFUSE_BUTTON_LAYER); // Define the layer mask for the tile layer.

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);

            if (hit.collider != null)
                OnClick?.Invoke(this, EventArgs.Empty);
        }

        public void ResetDefuseButton()
            => SetIsInteractable(false);

        public void SetIsInteractable(bool isToggleable)
        {
            _isInteractale = isToggleable;

            if (isToggleable)
                OnInteractable?.Invoke(this, EventArgs.Empty);
            else
                OnNotInteractable?.Invoke(this, EventArgs.Empty);
        }

        public void CorrectClick()
            => OnCorrectClick?.Invoke(this, EventArgs.Empty);

        public void IncorrectClick()
            => OnIncorrectClick?.Invoke(this, EventArgs.Empty);
    }
}