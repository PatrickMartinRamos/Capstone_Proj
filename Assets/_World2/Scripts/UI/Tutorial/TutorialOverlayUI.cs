using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Image))]
    public class TutorialOverlayUI : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] protected Image _visual;

        private RectTransform _rectTransform;
        private TutorialManager _tutorialManager;

        protected virtual void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();

            _rectTransform = (RectTransform)transform;

            HideCanvas();
        }

        private void Start()
        {
            if (TutorialManager.Instance != null)
                _tutorialManager = TutorialManager.Instance;
        }

        public virtual void DisplayChooseHydriousTutorial()
        {
            ShowCanvas();
            RaycastCanTarget();
        }

        protected void SetSizeAndPosition(Vector2 size, Vector2 position)
        {
            _rectTransform.sizeDelta = size;
            _rectTransform.anchoredPosition = position;
        }

        protected void RaycastCanTarget()
            => SetRaycastTarget(true);

        protected void RaycastCannotTarget()
            => SetRaycastTarget(false);

        private void SetRaycastTarget(bool istargetable)
            => _visual.raycastTarget = istargetable;

        protected void FullAnchors()
            => SetAnchors(Vector2.zero, Vector2.one);

        protected void CenterAnchors()
        {
            Vector2 centerAnchors = Vector2.one * 0.5f;
            SetAnchors(centerAnchors);
        }

        private void SetAnchors(Vector2 anchors)
            => SetAnchors(anchors, anchors);

        private void SetAnchors(Vector2 min, Vector2 max)
        {
            _rectTransform.anchorMin = min;
            _rectTransform.anchorMax = max;
        }

        protected void ShowCanvas()
            => SetCanvasVisiblity(true);

        protected void HideCanvas()
            => SetCanvasVisiblity(false);

        private void SetCanvasVisiblity(bool isVisible)
            => _canvas.SetActive(isVisible);
    }
}