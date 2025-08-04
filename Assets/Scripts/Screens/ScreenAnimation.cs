using CapstoneProj.GridSystem;
using UnityEngine;

namespace CapstoneProj.ScreenSystem
{
    public class ScreenAnimation : MonoBehaviour
    {
        protected const string MAXIMIZE = "Maximize"; // Animation trigger for maximizing the screen.
        protected const string MINIMIZE = "Minimize"; // Animation trigger for minimizing the screen.

        [SerializeField] protected InputToTileDetector inputToTileDetectorInternal; // Reference to the InputToTileDetector component.
        [SerializeField] protected Animator animatorInternal; // Reference to the animator component.

        protected virtual void Awake()
        {
            if (animatorInternal == null)
                animatorInternal = GetComponent<Animator>(); // Get the animator component if not assigned.

            inputToTileDetectorInternal.OnTileWithBombDetected
                += InputToTileDetector_OnTileWithBombDetected; // Subscribe to the tile detection event.
        }
        
        protected void OnDestroy()
        {
            if (inputToTileDetectorInternal != null)
                inputToTileDetectorInternal.OnTileWithBombDetected
                    -= InputToTileDetector_OnTileWithBombDetected; // Unsubscribe from the tile detection event.
        }

        protected virtual void InputToTileDetector_OnTileWithBombDetected(object sender, InputToTileDetector.OnTileWithBombDetectedEventArgs e) { }
    }
}