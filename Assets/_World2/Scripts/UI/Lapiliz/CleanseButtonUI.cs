using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Button))]
    public class CleanseButtonUI : MonoBehaviour
    {
        [SerializeField] private Button _button;

        private RectTransform _rectTransform;
        private float _rectWidth;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;

            if (_button == null)
                _button = GetComponent<Button>();

            _button.image.alphaHitTestMinimumThreshold = 0.1f;

            _button.onClick.AddListener(() =>
            {
                DashboardManager.Instance.Extract();
                DashboardManager.Instance.SwitchOff();
            });
        }
        private void Start()
            => _rectWidth = _rectTransform.rect.width;

        public Sequence MoveCenter()
            => Move(0);
        public Sequence MoveRight()
            => Move(_rectWidth);
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
    }
}