using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(RectTransform))]
    public class ClawUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private RectTransform _container;
        [SerializeField] private Image _leftHead;
        [SerializeField] private Image _rightHead;
        [SerializeField] private Sprite _leftHeadOpenSprite;
        [SerializeField] private Sprite _rightHeadOpenSprite;
        [SerializeField] private Sprite _leftHeadClosedSprite;
        [SerializeField] private Sprite _rightHeadClosedSprite;
        private Sequence _sequence;

        private void Awake()
        {
            if (_rectTransform == null)
                _rectTransform = GetComponent<RectTransform>();

            ResetClaw();
        }

        private void Update()
        {
            _rectTransform.SetSiblingIndex(10);
        }

        public Sequence Capture()
        {
            Time.timeScale = 0.1f;
            ResetClaw();

            Hydrious hydrious = CaptureManager.Instance.GetHydriousTarget();
            RectTransform hydriousRectTransform = hydrious.GetRectTransform();
            RotateClaw(hydriousRectTransform);

            // Move toward fish along the claw’s local up direction
            Vector3 targetPos = _rectTransform.position + _rectTransform.up * Vector3.Distance(hydriousRectTransform.position, _rectTransform.position);

            // Tween in world space
            _sequence.Append(_rectTransform.DOMove(targetPos, 1f).OnComplete(() =>
                CloseClaw()));

            // Run this 0.75s *after the tween starts* (during the move)
            _sequence.Insert(0.5f, DOVirtual.DelayedCall(0f, () =>
            {
                hydrious.SetParent(_container);
            }));

            targetPos = _rectTransform.position + _rectTransform.up * Vector3.Distance(Vector2.zero, _rectTransform.position);

            _sequence.Append(_rectTransform.DOMove(-targetPos, 5f)).OnComplete(() =>
                Time.timeScale = 1f);

            // _sequence.Join(_rectTransform.dopuns)

            return _sequence;
        }

        private void RotateClaw(RectTransform hydriousRectTransform)
        {
            // Get direction vector in the same coordinate space
            Vector2 direction = hydriousRectTransform.position - _rectTransform.position;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            // Apply rotation only on Z axis
            _rectTransform.rotation = Quaternion.Euler(0, 0, -angle);
        }

        private void ResetClaw()
        {
            _sequence?.Kill();
            _sequence = DOTween.Sequence();
            _rectTransform.anchoredPosition = Vector2.zero;
            _rectTransform.rotation = Quaternion.identity;
            OpenClaw();
        }

        private void OpenClaw()
            => ToggleClaw(_leftHeadOpenSprite, _rightHeadOpenSprite);

        private void CloseClaw()
            => ToggleClaw(_leftHeadClosedSprite, _rightHeadClosedSprite);

        private void ToggleClaw(Sprite leftHeadSprite, Sprite rightHeadSprite)
        {
            _leftHead.sprite = leftHeadSprite;
            _rightHead.sprite = rightHeadSprite;
        }

    }
}