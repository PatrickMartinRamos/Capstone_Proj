using DG.Tweening;

namespace Stellarfarer
{
    [UnityEngine.RequireComponent(typeof(UnityEngine.RectTransform))]
    [UnityEngine.RequireComponent(typeof(UnityEngine.UI.Image))]
    public class HydriousVisual : UnityEngine.MonoBehaviour
    {
        [UnityEngine.SerializeField] private HydriousMovement _hydriousMovement;
        [UnityEngine.SerializeField] private UnityEngine.RectTransform _rectTransform;
        [UnityEngine.SerializeField] private UnityEngine.UI.Image _visual;

        private void Awake()
        {
            if (_hydriousMovement == null)
            {
                UnityEngine.Debug.LogError($"{typeof(HydriousMovement).Name} is not assigned.");
                return;
            }

            if (_rectTransform == null)
                _rectTransform = GetComponent<UnityEngine.RectTransform>();

            if (_visual == null)
                _visual = GetComponent<UnityEngine.UI.Image>();

            _hydriousMovement.OnFlip
                += HydriousMovement_OnFlip;
            _hydriousMovement.OnCapture
                += HydriousMovement_OnCapture;
        }

        private void OnDestroy()
        {
            _hydriousMovement.OnFlip
                -= HydriousMovement_OnFlip;
            _hydriousMovement.OnCapture
                -= HydriousMovement_OnCapture;
        }

        private Tween HydriousMovement_OnFlip(
            float movePositionX,
            float currentAnchoredPositionX,
            float currentRotationY,
            float _rotationTimer)
        {
            float lookLeftRotation = 0f;
            float lookRightRotation = 180f;

            float rotationY = movePositionX switch
            {
                var movePosX when movePosX > currentAnchoredPositionX => lookRightRotation,
                var movePosX when movePosX < currentAnchoredPositionX => lookLeftRotation,
                _ => currentRotationY
            };

            Tween flipTween = _rectTransform.DOLocalRotate(new UnityEngine.Vector3(0f, rotationY, 0f), _rotationTimer);

            return rotationY != currentRotationY ? flipTween : null;
        }

        private void HydriousMovement_OnCapture()
        {
            float _intensity = 45f;   // max random tilt angle
            float _duration = 0.3f;   // how long before repeating
            int _vibrato = 10;        // number of shakes

            _rectTransform
                .DOShakeRotation(_duration, new UnityEngine.Vector3(0, 0, _intensity), _vibrato, 90f, false, ShakeRandomnessMode.Full)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear);
        }
    }
}