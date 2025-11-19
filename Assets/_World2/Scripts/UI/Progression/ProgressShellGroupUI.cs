using UnityEngine;

namespace Stellarfarer
{
    public class ProgressShellGroupUI : MonoBehaviour
    {
        [SerializeField] private ProgressShellUI _shellUI1;
        [SerializeField] private ProgressShellUI _shellUI2;
        [SerializeField] private ProgressShellUI _shellUI3;

        public void Progress(float progression)
        {
            float shell1Threshold = 1 / 3f;
            float shell2Threshold = 2 / 3f;
            float shell3Threshold = 3 / 3f;

            if (IsAboveThreshold(progression, shell1Threshold))
                _shellUI1.FadeIn();

            if (IsAboveThreshold(progression, shell2Threshold))
                _shellUI2.FadeIn();

            if (IsAboveThreshold(progression, shell3Threshold))
                _shellUI3.FadeIn();
        }

        private bool IsAboveThreshold(float value, float minThreshold)
            => value >= minThreshold;
    }
}