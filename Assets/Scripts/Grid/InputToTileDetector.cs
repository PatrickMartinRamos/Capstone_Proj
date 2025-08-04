using System;
using CapstoneProj.GameInputSystem;
using CapstoneProj.MiscSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class InputToTileDetector : SingletonBehaviour<InputToTileDetector>
    {
        public event EventHandler<OnTileWithBombDetectedEventArgs> OnTileWithBombDetected; // Event to notify when a tile is detected.
        public class OnTileWithBombDetectedEventArgs : EventArgs
        {
            public Tile DetectedTileWithBomb { get; private set; } // The tile that has a bomb.

            public OnTileWithBombDetectedEventArgs(Tile detectedTileWithBomb)
            {
                DetectedTileWithBomb = detectedTileWithBomb;
            }
        }

        [SerializeField] private GameInputs _gameInputs; // Reference to the game inputs.
        private Vector2 _mousePosition; // Stores the mouse position.
        private bool _hasInteractedSuccessfully = false; // Flag to check if the interaction was successful.
        [SerializeField] private bool _isGridReady = false; // Flag to check if the grid is ready for interaction.

        protected override void Awake()
        {
            base.Awake();
            
            SetGridReady(false); // Initially set the grid to not ready.
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
            if (!_isGridReady)
                return;

            if (_hasInteractedSuccessfully)
                return; // If the interaction was already successful, do nothing.

            Ray ray = Camera.main.ScreenPointToRay(_mousePosition);

            Vector2 origin = ray.origin;
            Vector2 direction = ray.direction;

            int layerMask = LayerMask.GetMask("Tile"); // Define the layer mask for the tile layer.

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                if (!hit.collider.TryGetComponent(out Tile tile))
                    return;

                if (!tile.TryGetComponent(out TileInteraction tileInteraction))
                    return;

                if (tile.HasBomb())
                {
                    _hasInteractedSuccessfully = true; // Mark the interaction as successful.
                    tileInteraction.SuccessfulInteraction(); // Notify the tile interaction of a successful interaction.
                    OnTileWithBombDetected?.Invoke(this, new OnTileWithBombDetectedEventArgs(tile)); // Invoke the event with the detected tile.
                }
                else
                {
                    _hasInteractedSuccessfully = false; // Mark the interaction as unsuccessful.
                    tileInteraction.UnsuccessfulInteraction(); // Notify the tile interaction of an unsuccessful interaction.
                }
            }
        }
        public void ResetInteraction()
            => _hasInteractedSuccessfully = false; // Reset the interaction state.

        public void SetGridReady(bool isGridReady)
        {
            _isGridReady = isGridReady;
            ResetInteraction();
        }
    }
}