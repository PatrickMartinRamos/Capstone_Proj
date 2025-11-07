using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class CaptureManagerUI : MonoBehaviour
    {
        [SerializeField] private ClawUI _clawUI;
        [SerializeField] private Image _leftDoor;
        [SerializeField] private Image _rightDoor;
        private (float leftDoor, float rightDoor) _doorSizeX;
        private (float leftDoor, float rightDoor) _doorPositionX;
        private CaptureManager _captureManager;
        private Sequence _sequence;

        private void Awake()
        {
            ResetCaptureManagerUI();
            _doorSizeX.leftDoor = _leftDoor.GetScaledVisibleSpriteSizeInUIUnits().x;
            _doorSizeX.rightDoor = _leftDoor.GetScaledVisibleSpriteSizeInUIUnits().x;
        }

        private void Start()
        {
            if (CaptureManager.Instance != null)
            {
                _captureManager = CaptureManager.Instance;

                float halvedHoldingAreaSizeX = _captureManager.GetHoldingAreaBounds().size.x / 2;
                _doorPositionX.leftDoor = halvedHoldingAreaSizeX - _doorSizeX.leftDoor;
                _doorPositionX.rightDoor = halvedHoldingAreaSizeX - _doorSizeX.rightDoor;

                _captureManager.OnCapture
                    += CaptureManager_OnCapture;
            }
        }

        private void OnDestroy()
        {
            if (_captureManager != null)
                _captureManager.OnCapture
                    -= CaptureManager_OnCapture;
        }

        private void CaptureManager_OnCapture()
        {
            _clawUI.Capture().OnComplete(OpenDoors);
        }

        private void OpenDoors()
            => SlideDoors(_doorPositionX);

        private void CloseDoors()
            => SlideDoors();

        private void SlideDoors()
            => SlideDoors((0f, 0f));

        private void SlideDoors((float leftDoor, float rightDoor) doorPositionX)
        {
            ResetCaptureManagerUI();
            _sequence.Append(_leftDoor.rectTransform.DOAnchorPosX(-doorPositionX.leftDoor, 1.5f, true));
            _sequence.Join(_rightDoor.rectTransform.DOAnchorPosX(doorPositionX.rightDoor, 1.5f, true));
        }

        private void ResetCaptureManagerUI()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
        }
    }
}