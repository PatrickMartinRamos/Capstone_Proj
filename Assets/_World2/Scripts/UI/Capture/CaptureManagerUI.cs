using DG.Tweening;
using UnityEngine;

namespace Stellarfarer
{
    public class CaptureManagerUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _content; // parent of everything in your screen
        [SerializeField] private LaserCannonUI _laserCannonUI;
        [SerializeField] private ClawUI _clawUI;
        [SerializeField] private TankDoorsUI _tankDoorsUI;

        private CaptureManager _captureManager;

        private void Start()
        {
            if (CaptureManager.Instance != null)
            {
                _captureManager = CaptureManager.Instance;

                _captureManager.OnCapture
                    += CaptureManager_OnCapture;
                _captureManager.OnCleanse
                    += CaptureManager_OmCleanse;
            }
        }

        private void OnDestroy()
        {
            if (_captureManager != null)
            {
                _captureManager.OnCapture
                    -= CaptureManager_OnCapture;
                _captureManager.OnCleanse
                    -= CaptureManager_OmCleanse;
            }
        }

        private void CaptureManager_OmCleanse()
        {
            Sequence mainSequence = DOTween.Sequence();

            HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();

            Sequence tankDoorSequence = _tankDoorsUI.CloseDoors();
            Sequence clawSequence = _clawUI.Release(hydriousUI);

            mainSequence.Append(tankDoorSequence);
            mainSequence.Append(_content.DOPunchAnchorPos(Vector2.one * 25f, 3f));
            mainSequence.Append(clawSequence);
            mainSequence.AppendCallback(() =>
            {
                CaptureManager captureManager = CaptureManager.Instance;
                ProgressionManager.Instance.AddPoints(captureManager.GetHydriousUITarget().GetHydrionEnergyAmountGiven());
                captureManager.ClearHydriousUITarget();
                DashboardManager.Instance.Idle();
            });
        }

        private void CaptureManager_OnCapture()
        {
            Sequence mainSequence = DOTween.Sequence();

            HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();
            hydriousUI.SetAsLastSibling();

            RectTransform hydriousRectTransform = hydriousUI.GetRectTransform();
            RectTransform holdingAreaRectTransform = _captureManager.GetHoldingAreaRectTransform();

            Sequence laserCannonSequence = _laserCannonUI.Capture(hydriousUI, hydriousRectTransform);
            Sequence clawSequence = _clawUI.Capture(hydriousUI, hydriousRectTransform, holdingAreaRectTransform);
            Sequence tankDoorSequence = _tankDoorsUI.OpenDoors();

            mainSequence.Append(laserCannonSequence);
            mainSequence.Append(clawSequence);
            mainSequence.Append(tankDoorSequence);
        }
    }
}