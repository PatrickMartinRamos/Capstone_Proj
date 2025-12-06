using UnityEngine;

namespace Stellarfarer
{
    public class ContainerResponsiveUI : MonoBehaviour
    {
        private const float MAX_OFFSET = 75f;
        private const float ORIGINAL_WIDTH = 900f;
        private const float ORIGINAL_HEIGHT = 1600f;

        private void Start()
        {
            RectTransform rectTransform = (RectTransform)transform;
            float screenAspect = (float)Screen.safeArea.width / Screen.safeArea.height;
            float originalAspect = ORIGINAL_WIDTH / ORIGINAL_HEIGHT;

            // Ratio of how wide or tall the screen is compared to your design
            float aspectRatioFactor = screenAspect / originalAspect;

            // If aspectRatioFactor < 1 → screen is taller → offset smaller
            // If aspectRatioFactor > 1 → screen is wider → offset larger
            float responsiveOffset = MAX_OFFSET * aspectRatioFactor;

            // Don’t allow it to go above max
            responsiveOffset = Mathf.Min(responsiveOffset, MAX_OFFSET);

            // Don't allow negative (super tall screens)
            responsiveOffset = Mathf.Max(responsiveOffset, 0f);

            // Apply offset
            rectTransform.offsetMin = new Vector2(responsiveOffset, responsiveOffset);
            rectTransform.offsetMax = new Vector2(-responsiveOffset, -responsiveOffset);
        }
    }
}
