using UnityEngine;

namespace CapstoneProj.ProgressSystem
{
    public class ProgressStar : MonoBehaviour
    {
        enum ProgressStarType
        {
            Star1,
            Star2,
            Star3
        }

        [SerializeField] private ProgressStarType _progressStarType;

        private void Start()
        {
            ProgressBar.Instance.OnStarProgress +=
                ProgressBar_OnStarProgress;

            SetVisibility(false);
        }

        private void ProgressBar_OnStarProgress(object sender, ProgressBar.OnStarProgressEventArgs e)
        {
            float progressPercentThreshold = _progressStarType switch
            {
                ProgressStarType.Star1 => 1f / 3f,
                ProgressStarType.Star2 => 2f / 3f,
                ProgressStarType.Star3 => 3f / 3f,
                _ => 0f
            };

            SetVisibility(e.ProgressPercent >= progressPercentThreshold);
        }

        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}