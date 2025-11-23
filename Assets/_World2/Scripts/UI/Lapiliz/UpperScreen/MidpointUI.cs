using UnityEngine;

namespace Stellarfarer
{
    public class MidpointUI : MonoBehaviour
    {
        [SerializeField] private SolutionSlotButtonUI _midpoint_X1;
        [SerializeField] private SolutionSlotButtonUI _midpoint_Y1;
        [SerializeField] private SolutionSlotButtonUI _midpoint_X2;
        [SerializeField] private SolutionSlotButtonUI _midpoint_Y2;
        [SerializeField] private AnswerSlotUI _midpoint_X;
        [SerializeField] private AnswerSlotUI _midpoint_Y;

        private NumberSlotManager _numberSlotManager;

        private void Start()
        {
            if (NumberSlotManager.Instance != null)
            {
                _numberSlotManager = NumberSlotManager.Instance;

                _numberSlotManager.OnTryExtractMidpoint
                    += NumberSlotManager_OnTryExtractMidpoint;
            }
        }

        private void OnDestroy()
        {
            if (_numberSlotManager != null)
            {
                _numberSlotManager.OnTryExtractMidpoint
                    -= NumberSlotManager_OnTryExtractMidpoint;
            }
        }

        private bool NumberSlotManager_OnTryExtractMidpoint()
        {
            PointsManager.Instance.GetPoints(out Vector2 pointA, out Vector2 pointB);

            if (!_midpoint_X1.TryGetValue(out int x1))
                return false;

            if (!_midpoint_Y1.TryGetValue(out int y1))
                return false;

            if (!_midpoint_X2.TryGetValue(out int x2))
                return false;

            if (!_midpoint_Y2.TryGetValue(out int y2))
                return false;

            if (!_midpoint_X.TryGetValue(out float x))
                return false;

            if (!_midpoint_Y.TryGetValue(out float y))
                return false;

            bool isX1Correct = x1 == (int)pointA.x || x1 == (int)pointB.x;
            bool isY1Correct = y1 == (int)pointA.x || y1 == (int)pointB.x;
            bool isX2Correct = x2 == (int)pointA.y || x2 == (int)pointB.y;
            bool isY2Correct = y2 == (int)pointA.y || y2 == (int)pointB.y;
            bool isXCorrect = x == (pointA.x + pointB.x) / 2;
            bool isYCorrect = y == (pointA.y + pointB.y) / 2;

            return isX1Correct && isY1Correct &&
                isX2Correct && isY2Correct &&
                isXCorrect && isYCorrect;
        }

        public void Show()
            => SetVisiblity(true);

        public void Hide()
            => SetVisiblity(false);

        private void SetVisiblity(bool isVisible)
            => gameObject.SetActive(isVisible);

        public void ResetMidpointUI()
        {
            _midpoint_X1.ClearText();
            _midpoint_Y1.ClearText();
            _midpoint_X2.ClearText();
            _midpoint_Y2.ClearText();
            _midpoint_X.ClearText();
            _midpoint_Y.ClearText();
        }
    }
}