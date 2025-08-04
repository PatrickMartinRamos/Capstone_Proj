using System;
using CapstoneProj.EnumSystem;
using CapstoneProj.GameInputSystem;
using TMPro;
using UnityEngine;

namespace CapstoneProj.ControlPanelSystem
{
    public class Coordinate : MonoBehaviour
    {
        private const string ORDERED_PAIR_LAYER = "OrderedPair"; // Layer name for the ordered pair layer.
        private const float SWIPE_THRESHOLD = 50f; // Threshold for swipe detection.

        [SerializeField] private CoordinateType _coordinateType;
        [SerializeField] private GameInputs _gameInputs;
        [SerializeField] private TextMeshPro _coordinateText; // Text component for displaying the X coordinate.
        private int _coordinate = 0;
        private Vector2 _currentMousePosition, _startMousePosition, _endMousePosition;
        private const int COORDINATE_THRESHOLD = 4;
        private bool _isScrollable = false;
        private bool _isDragging = false;

        private void Awake()
            => ResetCoordinate();

        private void Start()
        {
            // Initialize the game inputs.
            _gameInputs = GameInputs.Instance;

            // Subscribe to input actions.
            _gameInputs.OnPrimaryInteractStartedAction
                += GameInputs_OnPrimaryInteractStartedAction; // Subscribe to primary interact start action.
            _gameInputs.OnPrimaryInteractCanceledAction
                += GameInputs_OnPrimaryInteractCanceledAction; // Subscribe to primary interact cancel action.
            _gameInputs.OnPointerPositionPerformedAction
                += GameInputs_OnPointerPositionPerformedAction; // Subscribe to pointer position action.

            OrderedPair.Instance.OnCoordinateIsScrollableSet
                += OrderedPair_OnCoordinateIsScrollableSet;
        }

        private void OnDestroy()
        {
            // Unsubscribe from input actions to prevent memory leaks.
            if (_gameInputs != null)
            {
                _gameInputs.OnPrimaryInteractCanceledAction
                    -= GameInputs_OnPrimaryInteractCanceledAction;
                _gameInputs.OnPrimaryInteractStartedAction
                    -= GameInputs_OnPrimaryInteractStartedAction;
                _gameInputs.OnPointerPositionPerformedAction
                    -= GameInputs_OnPointerPositionPerformedAction;
            }

            if (OrderedPair.Instance != null)
            {
                OrderedPair.Instance.OnCoordinateIsScrollableSet
                    -= OrderedPair_OnCoordinateIsScrollableSet;
            }
        }

        private void OrderedPair_OnCoordinateIsScrollableSet(object sender, OrderedPair.OnCoordinateIsScrollableSetEventArgs e)
            => SetIsScrollable(e.IsScrollable);

        private void GameInputs_OnPointerPositionPerformedAction(Vector2 vector)
            => _currentMousePosition = vector;

        private void GameInputs_OnPrimaryInteractStartedAction()
        {
            if (!_isScrollable)
                return;

            Ray ray = Camera.main.ScreenPointToRay(_currentMousePosition);

            Vector2 origin = ray.origin;
            Vector2 direction = ray.direction;

            int layerMask = LayerMask.GetMask(ORDERED_PAIR_LAYER); // Define the layer mask for the tile layer.

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent(out Coordinate orderedPair))
                {
                    if (orderedPair != this)
                        return;

                    _isDragging = true;
                    _startMousePosition = _currentMousePosition;
                }
            }
        }

        private void GameInputs_OnPrimaryInteractCanceledAction()
        {
            if (!_isScrollable)
                return;

            if (!_isDragging)
                return;

            _isDragging = false;
            _endMousePosition = _currentMousePosition;

            Vector2 delta = _endMousePosition - _startMousePosition;
            float distance = delta.magnitude;
            Vector2 direction = delta.normalized;

            if (distance > SWIPE_THRESHOLD)
            {
                float directionThreshold = 0f;
                if (Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
                {
                    if (direction.y > directionThreshold)
                        _coordinate++;
                    else
                        _coordinate--;

                    _coordinate = Mathf.Clamp(_coordinate, -COORDINATE_THRESHOLD, COORDINATE_THRESHOLD);
                    _coordinateText.text = _coordinate.ToString();

                    switch (_coordinateType)
                    {
                        case CoordinateType.X_Coordinate:
                            OrderedPair.Instance.SetXTileCoordinate(_coordinate);
                            break;
                        case CoordinateType.Y_Coordinate:
                            OrderedPair.Instance.SetYTileCoordinate(_coordinate);
                            break;
                    }
                }
            }
        }

        public void ResetCoordinate()
        {
            _coordinateText.text = "0";
            _coordinate = 0;
            _currentMousePosition = _startMousePosition = _endMousePosition = Vector2Int.zero;
            _isScrollable = _isDragging = false;
        }

        public void SetIsScrollable(bool isScrollable)
            => _isScrollable = isScrollable;
    }
}