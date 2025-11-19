using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class TankDoorsUI : MonoBehaviour
    {
        [SerializeField] private Image _leftDoor;
        [SerializeField] private Image _rightDoor;
        private (float leftDoor, float rightDoor) _doorSizeX;
        private (float leftDoor, float rightDoor) _doorPositionX;
        private Sequence _sequence;

        private void Awake()
        {
            _leftDoor.rectTransform.anchoredPosition = Vector2.zero;
            _rightDoor.rectTransform.anchoredPosition = Vector2.zero;
        }

        private void Start()
        {
            _doorSizeX.leftDoor = _leftDoor.GetScaledVisibleSpriteSizeInUIUnits().x;
            _doorSizeX.rightDoor = _leftDoor.GetScaledVisibleSpriteSizeInUIUnits().x;
        }

        public Sequence OpenDoors()
        {
            float halvedHoldingAreaSizeX = Utils.Halve(CaptureManager.Instance.GetHoldingAreaRectTransform().rect.size.x);
            _doorPositionX.leftDoor = halvedHoldingAreaSizeX - _doorSizeX.leftDoor;
            _doorPositionX.rightDoor = halvedHoldingAreaSizeX - _doorSizeX.rightDoor;
            return SlideDoors(_doorPositionX);
        }

        public Sequence CloseDoors()
            => SlideDoors((0f, 0f));

        private Sequence SlideDoors((float leftDoor, float rightDoor) doorPositionX)
        {
            ResetTankDoors();
            _sequence.Append(_leftDoor.rectTransform.DOAnchorPosX(-doorPositionX.leftDoor, 1.5f, true));
            _sequence.Join(_rightDoor.rectTransform.DOAnchorPosX(doorPositionX.rightDoor, 1.5f, true));
            return _sequence;
        }

        private void ResetTankDoors()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
        }
    }
}