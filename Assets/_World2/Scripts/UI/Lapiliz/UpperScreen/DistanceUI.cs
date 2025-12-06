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

        private void Awake()
        {
            _distance_X1.OnValueChanged
                += HandleInput;
            _distance_Y1.OnValueChanged
                += HandleInput;
            _distance_X2.OnValueChanged
                += HandleInput;
            _distance_Y2.OnValueChanged
                += HandleInput;
            _distance.OnValueChanged
                += HandleInput;
        }

        private void HandleInput()
        {
            if (_distance_X1.IsNotEmpty() &&
            _distance_Y1.IsNotEmpty() &&
            _distance_X2.IsNotEmpty() &&
            _distance_Y2.IsNotEmpty() &&
            _distance.IsNotEmpty())
                Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplaySwitchToTab1Tutorial);
        }

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

            _distance_X1.OnValueChanged
                -= HandleInput;
            _distance_Y1.OnValueChanged
                -= HandleInput;
            _distance_X2.OnValueChanged
                -= HandleInput;
            _distance_Y2.OnValueChanged
                -= HandleInput;
            _distance.OnValueChanged
                -= HandleInput;
        }

        private bool NumberSlotManager_OnTryExtractDistance()
        {
            PointsManager.Instance.GetPoints(out Vector2 pointA, out Vector2 pointB);

            bool isX1Correct = _distance_X1.Grade(
                (answer) => answer == (int)pointB.x
            );

            bool isY1Correct = _distance_Y1.Grade(
                (answer) => answer == (int)pointA.x
            );

            bool isX2Correct = _distance_X2.Grade(
                (answer) => answer == (int)pointB.y
            );

            bool isY2Correct = _distance_Y2.Grade(
                (answer) => answer == (int)pointA.y
            );

            bool isDCorrect = _distance.Grade(
                (answer) =>
                {
                    float distance = Mathf.Round(
                            Mathf.Sqrt(
                                Mathf.Pow(pointB.x - pointA.x, 2) +
                                Mathf.Pow(pointB.y - pointA.y, 2)
                            ) * 100f
                        ) / 100f;

                    return Mathf.Abs(answer - distance) < 0.0000001f;
                }
            );

            if (isX1Correct &&
            isY1Correct &&
            isX2Correct &&
            isY2Correct &&
            isDCorrect)
                Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplayCleanseTutorial);
            else
            {
                Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplayIncorrectTutorial);
                if (!isX1Correct)
                    _distance_X1.ClearText();
                if (!isY1Correct)
                    _distance_Y1.ClearText();
                if (!isX2Correct)
                    _distance_X2.ClearText();
                if (!isY2Correct)
                    _distance_Y2.ClearText();
                if (!isDCorrect)
                    _distance.ClearText();
            }

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