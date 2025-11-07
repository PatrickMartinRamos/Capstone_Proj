using DG.Tweening;
using UnityEngine;

namespace Stellarfarer
{
    [RequireComponent(typeof(RectTransform))]
    public class HydriousMovement : MonoBehaviour
    {
        public event System.Func<float, float, float, float, Tween> OnFlip;
        public event System.Action OnCapture;

        [SerializeField] private Hydrious _hydrious;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Vector2 _moveCooldownTime;
        [SerializeField] private Vector2 _moveTime;
        [SerializeField] private Vector2 _rotationTime;
        [SerializeField] private Bounds _movementBounds;

        private float _moveCooldownTimer = 0;
        private float _moveTimer = 0;
        private float _rotationTimer = 0;
        private Vector2 _movePosition;
        private Sequence _sequence;

        private void Awake()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            _hydrious.OnIdle
                += Hydrious_OnIdle;
            _hydrious.OnMove
                += Hydrious_OnMove;
            _hydrious.OnCapture
                += Hydrious_OnCapture;

            ResetHydriousMovement();
        }

        private void OnDestroy()
        {
            _hydrious.OnIdle
                -= Hydrious_OnIdle;
            _hydrious.OnMove
                -= Hydrious_OnMove;
            _hydrious.OnCapture
                -= Hydrious_OnCapture;
        }

        private void Hydrious_OnIdle()
            => StartCoroutine(Idle());

        private System.Collections.IEnumerator Idle()
        {
            yield return new WaitForSeconds(_moveCooldownTimer);
            _moveCooldownTimer = GetRandomMoveCooldownTime();
            _moveTimer = GetRandomMoveTime();
            _rotationTimer = GetRandomRotationTime();
            _movePosition = GetRandomMovePosition();
            _hydrious.Move();
        }

        private void Hydrious_OnMove()
        {
            _sequence = DOTween.Sequence();

            Tween flipTween = OnFlip?.Invoke(_movePosition.x, _rectTransform.anchoredPosition.x, _rectTransform.localRotation.y, _rotationTimer);

            if (flipTween != null)
                _sequence.Append(flipTween);

            _sequence.Append(
                _rectTransform.DOAnchorPos(_movePosition, _moveTimer, true)
                    .OnComplete(() => _hydrious.Idle()));
        }

        private void Hydrious_OnCapture()
        {
            ResetHydriousMovement();

            OnCapture?.Invoke();
        }

        private void ResetHydriousMovement()
        {
            _sequence?.Kill();
            _moveCooldownTimer = 0;
            _moveTimer = 0;
            _rotationTimer = 0;
            _movePosition = _rectTransform.anchoredPosition;
        }

        private float GetRandomMoveCooldownTime()
            => GetRandomTime(_moveCooldownTime);

        private float GetRandomMoveTime()
            => GetRandomTime(_moveTime);

        private float GetRandomRotationTime()
            => GetRandomTime(_rotationTime);

        private float GetRandomTime(Vector2 time)
        {
            float randomTime = Random.Range(time.x, time.y);
            float decimalPrecision = 100f;
            return Mathf.Round(randomTime * decimalPrecision) / decimalPrecision;
        }

        private Vector2 GetRandomMovePosition()
        {
            float randomX = Random.Range(-_movementBounds.size.x, _movementBounds.size.x);
            float randomY = Random.Range(-_movementBounds.size.y, _movementBounds.size.y);

            var randomPosition = new Vector2(randomX, randomY);
            return (Vector2)_movementBounds.center + randomPosition;
        }

        public void SetMovementBounds(Bounds movementBounds)
            => _movementBounds = movementBounds;

        public RectTransform GetRectTransform()
            => _rectTransform;

        public void SetParent(Transform parent)
        {
            transform.SetParent(parent, true);
            _rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}