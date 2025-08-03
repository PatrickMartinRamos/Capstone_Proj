using CapstoneProj.GameInputSystem;
using UnityEngine;

namespace CapstoneProj.GridSystem
{
    public class InputToTileDetector : MonoBehaviour
    {
        [SerializeField] private GameInputs _gameInputs; // Reference to the game inputs.
        private Vector2 _mousePosition; // Stores the mouse position.

        private void Start()
        {
            // Initialize the game inputs.
            _gameInputs = GameInputs.Instance;

            // Subscribe to input actions.
            _gameInputs.OnPrimaryInteractStartedAction += GameInputs_OnPrimaryInteractStartedAction; // Subscribe to primary interact start action.
            _gameInputs.OnPointerPositionPerformedAction += GameInputs_OnPointerPositionPerformedAction; // Subscribe to pointer position action.
        }

        private void GameInputs_OnPointerPositionPerformedAction(Vector2 vector)
            => _mousePosition = vector;

        private void GameInputs_OnPrimaryInteractStartedAction()
        {
            Ray ray = Camera.main.ScreenPointToRay(_mousePosition);
            
            Vector2 origin = ray.origin;
            Vector2 direction = ray.direction;

            int layerMask = LayerMask.GetMask("Tile"); // Define the layer mask for the tile layer.

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                if (!hit.collider.TryGetComponent(out TileInteraction tileInteraction))
                    return;

                tileInteraction.OnInteracted();
            }
        }
    }
}