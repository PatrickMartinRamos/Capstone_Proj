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

            bool isX1Correct = _midpoint_X1.Grade(
                (answer) => answer == (int)pointA.x || answer == (int)pointB.x
            );

            bool isY1Correct = _midpoint_Y1.Grade(
                (answer) => answer == (int)pointA.x || answer == (int)pointB.x
            );

            bool isX2Correct = _midpoint_X2.Grade(
                (answer) => answer == (int)pointA.y || answer == (int)pointB.y
            );

            bool isY2Correct = _midpoint_Y2.Grade(
                (answer) => answer == (int)pointA.y || answer == (int)pointB.y
            );

            bool isXCorrect = _midpoint_X.Grade(
                (answer) => answer == (pointA.x + pointB.x) / 2
            );

            bool isYCorrect = _midpoint_Y.Grade(
                (answer) => answer == (pointA.y + pointB.y) / 2
            );

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