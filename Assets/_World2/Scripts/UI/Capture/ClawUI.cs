using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(RectTransform))]
    public class ClawUI : MonoBehaviour
    {
        [SerializeField] private RectTransform _container;
        [SerializeField] private Image _leftHead;
        [SerializeField] private Image _rightHead;
        [SerializeField] private Sprite _leftHeadOpenSprite;
        [SerializeField] private Sprite _rightHeadOpenSprite;
        [SerializeField] private Sprite _leftHeadClosedSprite;
        [SerializeField] private Sprite _rightHeadClosedSprite;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;

            ResetClaw();
        }

        public Sequence Capture(HydriousUI hydriousUI, RectTransform hydriousRectTransform, RectTransform holdingAreaRectTransform)
        {
            ResetClaw();

            RotateClaw(hydriousRectTransform);

            // Calculate distance between claw and fish
            float distance = Vector3.Distance(hydriousRectTransform.position, _rectTransform.position);

            // Move forward along claw’s local up direction
            Vector3 targetPos = _rectTransform.position + _rectTransform.up * distance;

            Sequence sequence = DOTween.Sequence();
            bool isCaptured = false;
            Vector2 hydriousPosition = hydriousRectTransform.position;
            Quaternion hydriousRotation = hydriousRectTransform.rotation;

            // Move toward fish
            sequence.Append(
                _rectTransform.DOMove(targetPos, 1f)
                    .SetEase(Ease.OutSine)
                    .OnUpdate(() =>
                    {
                        if (!isCaptured)
                        {
                            if (IsOverlapping(_rectTransform, hydriousRectTransform))
                            {
                                isCaptured = true;
                                hydriousUI.SetParent(_container, _container.position);
                            }
                        }
                        else
                            hydriousUI.SetPositionAndRotation(hydriousPosition, hydriousRotation);
                    })
            );

            sequence.AppendCallback(() =>
            {
                CloseClaw();
                hydriousUI.Capture();
            });

            // Then return along the *same path*, by moving back along local down direction
            sequence.Append(
                _rectTransform.DOMove(_rectTransform.position - _rectTransform.up * distance, 1f)
                    .SetEase(Ease.InSine)
            );

            sequence.AppendCallback(() =>
            {
                hydriousUI.PlaceInTank();
                hydriousUI.Mutate();
                hydriousUI.SetParent(holdingAreaRectTransform, holdingAreaRectTransform.position);
                hydriousUI.Float();
                DashboardManager.Instance.PowerOn();
                Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplayClickDashboardTutorial);
                ResetClaw();
            });

            return sequence;
        }

        public Sequence Release(HydriousUI hydriousUI)
        {
            ResetClaw();

            RectTransform spawnAreaRectTransform = HydriousSpawnManager.Instance.GetSpawnAreaRectTransform();

            // Calculate distance between claw and fish
            float distance = Vector3.Distance(spawnAreaRectTransform.position, _rectTransform.position);

            // Move forward along claw’s local up direction
            Vector3 targetPos = _rectTransform.position + _rectTransform.up * distance;

            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(() =>
            {
                CloseClaw();
                DashboardManager.Instance.PowerOff();
                hydriousUI.Cleanse();
                hydriousUI.Wait();
                hydriousUI.Capture();
                hydriousUI.SetParent(_container, _container.position);
            });
            sequence.Append(_rectTransform.DOMove(targetPos, 1f));
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() =>
            {
                OpenClaw();
                hydriousUI.SetParent(spawnAreaRectTransform, spawnAreaRectTransform.position);
                hydriousUI.PlaceInOcean();
                hydriousUI.Release();
            });
            sequence.Append(_rectTransform.DOMove(_rectTransform.position - _rectTransform.up * distance, 1f));

            return sequence;
        }

        bool IsOverlapping(RectTransform a, RectTransform b)
        {
            Rect rectA = GetWorldRect(a);
            Rect rectB = GetWorldRect(b);
            return rectA.Overlaps(rectB);
        }

        Rect GetWorldRect(RectTransform rectTransform)
        {
            Vector3[] corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return new Rect(corners[0].x, corners[0].y,
                corners[2].x - corners[0].x,
                corners[2].y - corners[0].y);
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