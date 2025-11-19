using NUnit.Framework;
using UnityEngine;

namespace Stellarfarer
{
    public class DistanceUI : MonoBehaviour
    {
        [SerializeField] private SolutionSlotButtonUI _distance_X1;
        [SerializeField] private SolutionSlotButtonUI _distance_Y1;
        [SerializeField] private SolutionSlotButtonUI _distance_X2;
        [SerializeField] private SolutionSlotButtonUI _distance_Y2;
        [SerializeField] private AnswerSlotUI _distance;

        private NumberSlotManager _numberSlotManager;

        private void Start()
        {
            if (NumberSlotManager.Instance != null)
            {
                _numberSlotManager = NumberSlotManager.Instance;

                _numberSlotManager.OnTryExtractDistance
                    += NumberSlotManager_OnTryExtractDistance;
            }
        }

        private void OnDestroy()
        {
            if (_numberSlotManager != null)
            {
                _numberSlotManager.OnTryExtractDistance
                    -= NumberSlotManager_OnTryExtractDistance;
            }
        }

        private bool NumberSlotManager_OnTryExtractDistance()
        {
            PointsManager.Instance.GetPoints(out Vector2 pointA, out Vector2 pointB);

            if (!_distance_X1.TryGetValue(out int x1))
                return false;

            if (!_distance_Y1.TryGetValue(out int y1))
                return false;

            if (!_distance_X2.TryGetValue(out int x2))
                return false;

            if (!_distance_Y2.TryGetValue(out int y2))
                return false;

            if (!_distance.TryGetValue(out float d))
                return false;

            bool isX1Correct = x1 == (int)pointB.x;
            bool isY1Correct = y1 == (int)pointA.x;
            bool isX2Correct = x2 == (int)pointB.y;
            bool isY2Correct = y2 == (int)pointA.y;
            float distance = Mathf.Round(
                    Mathf.Sqrt(
                        Mathf.Pow(pointB.x - pointA.x, 2) +
                        Mathf.Pow(pointB.y - pointA.y, 2)
                    ) * 100f
                ) / 100f;
            bool isDCorrect = Mathf.Abs(d - distance) < 0.001f;

            return isX1Correct && isY1Correct &&
                isX2Correct && isY2Correct &&
                isDCorrect;
        }

        public void Show()
            => SetVisiblity(true);

        public void Hide()
            => SetVisiblity(false);

        private void SetVisiblity(bool isVisible)
            => gameObject.SetActive(isVisible);

        public void ResetDistanceUI()
        {
            _distance_X1.ClearText();
            _distance_Y1.ClearText();
            _distance_X2.ClearText();
            _distance_Y2.ClearText();
            _distance.ClearText();
        }
    }
}