using DG.Tweening;
using UnityEngine;

namespace Stellarfarer
{
    public class LapilizScreenGroupUI : MonoBehaviour
    {
        [SerializeField] private LapilizUpperScreenUI _lapilizUpperScreenUI;
        [SerializeField] private LapilizLowerScreenUI _lapilizLowerScreenUI;

        private RectTransform _rectTransform;
        private float _rectWidth;

        private void Awake()
            => _rectTransform = (RectTransform)transform;
        private void Start()
            => _rectWidth = _rectTransform.rect.width;

        public Sequence MoveLeft()
            => Move(-_rectWidth);
        public Sequence MoveCenter()
            => Move(0);
        public Sequence Move(float endValue)
        {
            Sequence sequence = DOTween.Sequence();
            if (endValue == 0)
                sequence.JoinCallback(Show);
            sequence.Join(_rectTransform.DOAnchorPosX(endValue, 0.5f, true));
            if (Mathf.Abs(endValue) == _rectWidth)
                sequence.AppendCallback(Hide);
            return sequence;
        }

        private void Show()
            => SetVisibility(true);
        private void Hide()
            => SetVisibility(false);
        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);

        public void SetPlottingMode()
        {
            _lapilizUpperScreenUI.SetPlottingMode();
            _lapilizLowerScreenUI.SetPlottingMode();
        }
        public void ResetPlottingMode()
            => _lapilizUpperScreenUI.ResetPlottingMode();

        public void SetMidpointMode()
        {
            _lapilizUpperScreenUI.SetMidpointMode();
            _lapilizLowerScreenUI.SetMidpointDistanceMode();
        }
        public void ResetMidpoindMode()
            => _lapilizUpperScreenUI.ResetMidpointMode();

        public void SetDistanceMode()
        {
            _lapilizUpperScreenUI.SetDistanceMode();
            _lapilizLowerScreenUI.SetMidpointDistanceMode();
        }
        public void ResetDistanceMode()
            => _lapilizUpperScreenUI.ResetDistanceMode();
    }
}