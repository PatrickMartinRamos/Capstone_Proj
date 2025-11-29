using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Button))]
    public class CleanseButtonUI : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;

        private LapilizManager _lapilizManager;
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
                if (Hydros7WorldManager.Instance.IsGamePlaying() ||
                (Hydros7WorldManager.Instance.IsDisplayingTutorial() &&
                TutorialManager.Instance.IsDisplayingCleanseTutorial()))
                {
                    DashboardManager.Instance.Extract();
                    DashboardManager.Instance.SwitchOff();
                    Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.Wait);
                }
            });
        }
        private void Start()
        {
            _rectWidth = _rectTransform.rect.width;

            _button.image.ConfigureImageFromFullToVisibleSprite(_full, _visible);

            if (LapilizManager.Instance != null)
            {
                _lapilizManager = LapilizManager.Instance;

                _lapilizManager.OnCleanseButtonRectGot
                    += LapilizManager_OnCleanseButtonRectGot;
            }
        }

        private void OnDestroy()
        {
            if (_lapilizManager != null)
            {
                _lapilizManager.OnCleanseButtonRectGot
                    -= LapilizManager_OnCleanseButtonRectGot;
            }
        }

        private Rect LapilizManager_OnCleanseButtonRectGot()
        {
            RectTransform buttonRectTransform = _button.image.rectTransform;
            return new Rect(buttonRectTransform.position, buttonRectTransform.sizeDelta);
        }

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