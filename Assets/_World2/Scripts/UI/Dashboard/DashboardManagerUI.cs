using DG.Tweening;
using UnityEngine;

namespace Stellarfarer
{
    public class DashboardManagerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _content; // parent of everything in your screen
        [SerializeField] private DashboardScreenUI _upperScreen;
        [SerializeField] private DashboardScreenUI _lowerScreen;

        private DashboardManager _dashboardManager;
        private LoadingScreenUI _loadingScreenUI;
        private LapilizManager _lapilizManager;

        private void Start()
        {
            if (DashboardManager.Instance != null)
            {
                _dashboardManager = DashboardManager.Instance;

                _dashboardManager.OnPowerStateChanged
                    += DashboardManager_OnPowerStateChanged;
                _dashboardManager.OnSwitchStateChanged
                    += DashboardManager_OnSwitchStateChanged;
            }

            if (LoadingScreenUI.Instance != null)
                _loadingScreenUI = LoadingScreenUI.Instance;

            if (LapilizManager.Instance != null)
                _lapilizManager = LapilizManager.Instance;
        }

        private void OnDestroy()
        {
            if (_dashboardManager != null)
            {
                _dashboardManager.OnPowerStateChanged
                    -= DashboardManager_OnPowerStateChanged;
                _dashboardManager.OnSwitchStateChanged
                    -= DashboardManager_OnSwitchStateChanged;
            }
        }

        public Sequence ZoomToTarget()
        {
            Vector3 upperScreenPos = _upperScreen.GetPosition();
            Vector3 lowerScreenPos = _lowerScreen.GetPosition();

            float zoomScale = 1.5f;

            // 1️⃣ Get world position of target area
            Vector3 targetPos = (upperScreenPos + lowerScreenPos) / 2;

            // 2️⃣ Convert to local position relative to parent
            Vector3 localTarget = _content.InverseTransformPoint(targetPos);

            Vector3 movePosition = -localTarget * zoomScale;

            return SetZoom(zoomScale, movePosition);
        }

        public Sequence ResetZoom()
            => SetZoom(1f, Vector3.zero);

        private Sequence SetZoom(float zoomScale, Vector3 movePosition)
        {
            float duration = 0.5f;

            Sequence sequence = DOTween.Sequence();
            sequence.Append(_content.DOScale(zoomScale, duration).SetEase(Ease.OutSine));
            sequence.Join(_content.DOLocalMove(movePosition, duration).SetEase(Ease.OutSine));

            return sequence;
        }

        private void DashboardManager_OnSwitchStateChanged()
        {
            Sequence sequence = DOTween.Sequence();

            if (_dashboardManager.IsSwitchedOn())
            {
                sequence.Append(ZoomToTarget());
                sequence.Append(_loadingScreenUI.LoadScreen(_lapilizManager.Activate));
            }
            else if (_dashboardManager.IsSwitchedOff())
            {
                sequence.Append(_loadingScreenUI.LoadScreen(_lapilizManager.Deactivate));
                sequence.Append(ResetZoom());
                sequence.AppendCallback(() => CaptureManager.Instance.Cleanse());
            }
        }

        private void DashboardManager_OnPowerStateChanged()
        {
            if (_dashboardManager.IsPoweredOn())
            {
                HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();
                Color hydriousColor = hydriousUI.GetColor();
                _upperScreen.PowerOn(hydriousColor);
                _lowerScreen.PowerOn(hydriousColor);
                _loadingScreenUI.SetColor(hydriousColor);
            }
            else if (_dashboardManager.IsPoweredOff())
            {
                _upperScreen.PowerOff();
                _lowerScreen.PowerOff();
            }
        }
    }
}