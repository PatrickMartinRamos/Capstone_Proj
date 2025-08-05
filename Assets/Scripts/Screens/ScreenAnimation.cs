using System;
using CapstoneProj.ControlPanelSystem;
using CapstoneProj.GridSystem;
using CapstoneProj.ProgressSystem;
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
            TileDetector.Instance.OnTileWithBombDetected
                += TileDetector_OnTileWithBombDetected; // Subscribe to the tile detection event.
            Progression.Instance.OnProgress
                += Progression_OnProgress;
        }

        private void OnDestroy()
        {
            if (TileDetector.Instance != null)
                TileDetector.Instance.OnTileWithBombDetected
                    -= TileDetector_OnTileWithBombDetected; // Unsubscribe from the tile detection event.

            if (Progression.Instance != null)
                Progression.Instance.OnProgress
                    -= Progression_OnProgress;
        }

        private void Progression_OnProgress(object sender, EventArgs e)
        {
            string trigger = _screenType switch
            {
                ScreenType.TopScreen => MAXIMIZE, // Use MAXIMIZE trigger for TopScreen.
                ScreenType.BottomScreen => MINIMIZE, // Use MINIMIZE trigger for BottomScreen.
                _ => ""
            };

            TriggerAnimation(trigger);
        }

        private void TileDetector_OnTileWithBombDetected(object sender, TileDetector.OnTileWithBombDetectedEventArgs e)
        {
            _originalTopTile = e.DetectedTileWithBomb; // Store the original top tile when a bomb is detected.

            string trigger = _screenType switch
            {
                ScreenType.TopScreen => MINIMIZE, // Use MINIMIZE trigger for TopScreen.
                ScreenType.BottomScreen => MAXIMIZE, // Use MAXIMIZE trigger for BottomScreen.
                _ => ""
            };

            TriggerAnimation(trigger); // Set the appropriate animation trigger based on the screen type.
        }

        private void TriggerAnimation(string trigger)
            => _animator.SetTrigger(trigger);

        // Triggered after bottom screen maximize animation completes.
        public void BottomScreenMaximized()
        {
            if (_screenType != ScreenType.BottomScreen)
                return;

            BottomScreenBombSpawner.Instance.SpawnBomb(_originalTopTile);
            CartesianPlaneToggle.Instance.SetIsToggleable(true);
            OrderedPair.Instance.SetCoordinateIsScrollable(true);
            DefuseButton.Instance.SetIsInteractable(true);
        }

        public void BottomScreenMinimized()
        {
            if (_screenType != ScreenType.BottomScreen)
                return;

            CartesianPlaneToggle.Instance.ResetCartesianPlaneToggle();
            OrderedPair.Instance.CoordinateReset();
            DefuseButton.Instance.ResetDefuseButton();
            GridNavigator.Instance.DestroySelf();
        }
    }
}