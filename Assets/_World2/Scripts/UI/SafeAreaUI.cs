using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeAreaUI : MonoBehaviour
    {
        [SerializeField] private CanvasScaler _canvasScaler;

        private void Awake()
        {
            if (_canvasScaler == null)
            {
                Canvas canvas = gameObject.GetComponentInParent<Canvas>();
                _canvasScaler = canvas.GetComponent<CanvasScaler>();
            }

            AdjustSafeArea();
        }

        private void AdjustSafeArea()
        {
            Rect safeArea = Screen.safeArea;
            float screenHeight = Screen.height;

            // How many pixels are cut off at the top
            float topInsetPixels = screenHeight - safeArea.yMax;

            if (topInsetPixels <= 0)
                return;

            float scaleFactor = screenHeight / _canvasScaler.referenceResolution.y;
            float topInsetCanvasUnits = topInsetPixels / scaleFactor;

            // Apply offset to RectTransform
            RectTransform rectTransform = (RectTransform)transform;
            Vector2 offsetMax = rectTransform.offsetMax;
            offsetMax.y = -topInsetCanvasUnits;
            rectTransform.offsetMax = offsetMax;
        }

    }
}