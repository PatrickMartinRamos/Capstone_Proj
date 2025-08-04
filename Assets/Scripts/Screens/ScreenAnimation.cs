using CapstoneProj.GridSystem;
using UnityEngine;

namespace CapstoneProj.ScreenSystem
{
    public class ScreenAnimation : MonoBehaviour
    {
        private enum ScreenType
        {
            TopScreen,
            BottomScreen
        }

        private const string MAXIMIZE = "Maximize"; // Animation trigger for maximizing the screen.
        private const string MINIMIZE = "Minimize"; // Animation trigger for minimizing the screen.

        [SerializeField] private Animator _animator; // Reference to the animator component.
        [SerializeField] private ScreenType _screenType; // Type of the screen (Top or Bottom).
        private Tile _originalTopTile; // Reference to the original top tile for the screen.

        private void Awake()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>(); // Get the animator component if not assigned.
        }

        private void Start()
        {
            InputToTileDetector.Instance.OnTileWithBombDetected
                += InputToTileDetector_OnTileWithBombDetected; // Subscribe to the tile detection event.
        }

        private void OnDestroy()
        {
            if (InputToTileDetector.Instance != null)
                InputToTileDetector.Instance.OnTileWithBombDetected
                    -= InputToTileDetector_OnTileWithBombDetected; // Unsubscribe from the tile detection event.
        }

        private void InputToTileDetector_OnTileWithBombDetected(object sender, InputToTileDetector.OnTileWithBombDetectedEventArgs e)
        {
            _originalTopTile = e.DetectedTileWithBomb; // Store the original top tile when a bomb is detected.

            string trigger = _screenType switch
            {
                ScreenType.TopScreen => MINIMIZE, // Use MINIMIZE trigger for TopScreen.
                ScreenType.BottomScreen => MAXIMIZE, // Use MAXIMIZE trigger for BottomScreen.
                _ => ""
            };

            _animator.SetTrigger(trigger); // Set the appropriate animation trigger based on the screen type.
        }

        public void ScreenRefocused()
        {
            if (_screenType != ScreenType.BottomScreen)
                return;

            BottomScreenBombSpawner.Instance.SpawnBomb(_originalTopTile);
        }
    }
}