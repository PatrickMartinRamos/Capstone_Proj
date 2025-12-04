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

        protected virtual void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();

            _rectTransform = (RectTransform)transform;

            Hide();
        }

        protected void DisplayTargetable()
        {
            Show();
            RaycastCanTarget();
        }

        protected void DisplayFullScreenTargetable()
        {
            DisplayTargetable();
            FullAnchors();
            ResetOffset();
        }

        protected void DisplayWindowedScreenTargetable()
        {
            DisplayTargetable();
            CenterAnchors();
        }

        public virtual void DisplayChooseHydriousTutorial()
            => DisplayFullScreenTargetable();

        public virtual void DisplayClickDashboardTutorial()
        {
            DisplayWindowedScreenTargetable();
            Rect dashboardRect = DashboardManager.Instance.GetDashboardRect();
            SetSizeAndPosition(dashboardRect.size, dashboardRect.position);
        }

        public virtual void DisplayPlottingCartesianPlaneTutorial()
        {
            DisplayWindowedScreenTargetable();
            Rect cartesianPlaneToggleRect = LapilizManager.Instance.GetCartesianPlaneToggleRect();
            SetSizeAndPosition(cartesianPlaneToggleRect.size, cartesianPlaneToggleRect.position);
        }

        public virtual void DisplayBottom(float offset)
        {
            DisplayFullScreenTargetable();
            OffsetTop(offset);
        }

        public virtual void DisplayTop(float offset)
        {
            DisplayFullScreenTargetable();
            OffsetBottom(offset);
        }

        public virtual void DisplaySwitchToTab2Tutorial()
        {
            DisplayWindowedScreenTargetable();
            Rect tab2Rect = LapilizManager.Instance.GetTab2Rect();
            SetSizeAndPosition(tab2Rect.size, tab2Rect.position);
        }

        public virtual void DisplayLapilizLowerScreen()
        {
            DisplayWindowedScreenTargetable();
            Rect lowerScreenRect = LapilizManager.Instance.GetLowerScreenRect();
            SetSizeAndPosition(lowerScreenRect.size, lowerScreenRect.position);
        }

        public virtual void DisplaySwitchToTab1Tutorial()
        {
            DisplayWindowedScreenTargetable();
            Rect tab1Rect = LapilizManager.Instance.GetTab1Rect();
            SetSizeAndPosition(tab1Rect.size, tab1Rect.position);
        }

        public virtual void DisplayConfirmTutorial()
        {
            DisplayWindowedScreenTargetable();
            Rect cartesianPlaneToggleRect = LapilizManager.Instance.GetConfirmButtonRect();
            SetSizeAndPosition(cartesianPlaneToggleRect.size, cartesianPlaneToggleRect.position);
        }

        public virtual void DisplayCleanseTutorial()
        {
            DisplayWindowedScreenTargetable();
            Rect cleanseButtonRect = LapilizManager.Instance.GetCleansButtonRect();
            SetSizeAndPosition(cleanseButtonRect.size, cleanseButtonRect.position);
        }

        public virtual void DisplayProgressionTutorial()
        {
            DisplayWindowedScreenTargetable();
            Rect progressBarRect = ProgressionManager.Instance.GetProgressBarRect();
            SetSizeAndPosition(progressBarRect.size, progressBarRect.position);
        }

        public virtual void DisplayLapilizUpperScreen()
        {
            DisplayWindowedScreenTargetable();
            Rect upperScreenRect = LapilizManager.Instance.GetUpperScreenRect();
            SetSizeAndPosition(upperScreenRect.size, upperScreenRect.position);
        }

        protected void SetSizeAndPosition(Vector2 size, Vector2 position)
        {
            _rectTransform.sizeDelta = size;
            _rectTransform.position = position;
        }

        protected void ResetOffset()
        {
            _rectTransform.offsetMax = Vector2.zero;
            _rectTransform.offsetMin = Vector2.zero;
        }

        protected void OffsetTop(float offset)
            => _rectTransform.offsetMax = new Vector2(0, -offset);

        protected void OffsetBottom(float offset)
            => _rectTransform.offsetMin = new Vector2(0, offset);

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

        protected void Show()
            => SetVisibility(true);

        public void Hide()
            => SetVisibility(false);

        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}