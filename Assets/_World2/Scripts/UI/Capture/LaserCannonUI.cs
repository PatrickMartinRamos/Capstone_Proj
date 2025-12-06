using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class LaserCannonUI : MonoBehaviour
    {
        [System.Flags]
        private enum Location
        {
            Left = 1 << 0,
            Right = 1 << 1,
            Top = 1 << 2,
            Bottom = 1 << 3,
            TopLeft = Top | Left,
            BottomLeft = Bottom | Left,
            TopRight = Top | Right,
            BottomRight = Bottom | Right,
        }

        [SerializeField] private RectTransform _submarineCanvas;
        [SerializeField] private Location _location;
        [SerializeField] private RectTransform _laserBeamContainerRectTransform;
        [SerializeField] private RectTransform _laserBeamRectTransform;
        [SerializeField] private RectTransform _laserZap;
        [SerializeField] private Image _laserBeamBody;
        [SerializeField] private Image _laserBeamHead;
        private RectTransform _rectTransform;
        private float _idleRotY;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            _idleRotY = IsRight() ? 180f : 0f;
        }

        private void Start()
            => ResetLaserCannon();

        private void ResetLaserCannon()
        {
            if (HydriousSpawnManager.Instance != null)
            {
                RectTransform spawnAreaRectTransform = HydriousSpawnManager.Instance.GetSpawnAreaRectTransform();
                Vector2 padding = new Vector2(28.5f, 33f);
                Vector2 position = spawnAreaRectTransform.anchoredPosition;

                float offsetX = spawnAreaRectTransform.rect.width / 2 + padding.x;
                float offsetY = spawnAreaRectTransform.rect.height / 2 + padding.y;

                if (IsLeft())
                    position.x -= offsetX;

                if (IsRight())
                    position.x += offsetX;

                if (IsBottom())
                    position.y -= offsetY;

                if (IsTop())
                    position.y += offsetY;

                _rectTransform.anchoredPosition = position;
            }

            float height = 1920f;

            // original size from your prefab sprite
            Vector2 originalScale = _rectTransform.localScale;

            // scale factor based only on height (because Match = Height)
            float scale = _submarineCanvas.rect.height / height;

            // final size (maintains aspect ratio)
            Vector2 finalSize = originalScale * scale;

            _rectTransform.localScale = finalSize;

            _rectTransform.rotation = Quaternion.Euler(0f, _idleRotY, 0f);
            _laserBeamRectTransform.anchoredPosition = -Vector2.right * (_laserBeamHead.rectTransform.sizeDelta.x + _laserBeamBody.rectTransform.sizeDelta.x);
            _laserZap.anchoredPosition = Vector2.zero;
            _laserZap.sizeDelta = Vector2.zero;
            _laserBeamBody.fillAmount = 1f;
            _laserBeamHead.fillAmount = 1f;
        }

        private bool IsTop()
            => (_location & Location.Top) != 0;
        private bool IsBottom()
            => (_location & Location.Bottom) != 0;
        private bool IsLeft()
            => (_location & Location.Left) != 0;
        private bool IsRight()
            => (_location & Location.Right) != 0;

        public Sequence Capture(HydriousUI hydriousUI, RectTransform hydriousRectTransform)
        {
            bool isLaserBeamBodyDisplayed = false;
            ResetLaserCannon();

            // Main Sequence
            Sequence mainSequence = DOTween.Sequence();

            #region Laser Cannon Rotate
            mainSequence.Append(RotateLaserCannonToHydrious(hydriousRectTransform));
            #endregion

            #region Laser Cannon Fire
            mainSequence.AppendCallback(() =>
            {
                float distanceX = Mathf.Abs(_laserBeamContainerRectTransform.position.x - hydriousRectTransform.position.x);

                RectTransform laserBeamHeadRectTransform = _laserBeamHead.rectTransform;
                RectTransform laserBeamBodyRectTransform = _laserBeamBody.rectTransform;

                float laserBeamHeadWidth = Mathf.Min(300f, distanceX);
                float laserBeamBodyWidth = Mathf.Max(0f, distanceX - laserBeamHeadWidth);
                isLaserBeamBodyDisplayed = laserBeamBodyWidth > 0f;

                laserBeamHeadRectTransform.sizeDelta = new Vector2(laserBeamHeadWidth, laserBeamHeadRectTransform.sizeDelta.y);
                laserBeamBodyRectTransform.sizeDelta = new Vector2(laserBeamBodyWidth, laserBeamBodyRectTransform.sizeDelta.y);

                _laserBeamRectTransform.anchoredPosition = -Vector2.right * (laserBeamHeadWidth + laserBeamBodyWidth);
            });

            mainSequence.Append(_laserBeamRectTransform.DOLocalMove(Vector3.zero, 0.25f));
            #endregion

            #region Laser Zap
            mainSequence.AppendCallback(() =>
            {
                _laserZap.position = hydriousRectTransform.position;
                _laserZap.rotation = Quaternion.identity;
                _laserZap.sizeDelta = Vector2.zero;
            });

            Vector2 fullSize = hydriousRectTransform.rect.size;

            mainSequence.Append(_laserZap.DOSizeDelta(fullSize, 0.1f));

            mainSequence.AppendCallback(() => hydriousUI.Stun());
            #endregion

            #region Reset
            mainSequence.AppendCallback(() =>
            {
                Sequence laserZapDisappearSequence = DOTween.Sequence();
                laserZapDisappearSequence.AppendInterval(0.25f);
                laserZapDisappearSequence.Append(_laserZap.DOSizeDelta(Vector2.zero, 0.1f));
            });

            mainSequence.JoinCallback(() =>
            {
                Sequence laserBeamDisappearSequence = DOTween.Sequence();

                if (isLaserBeamBodyDisplayed)
                    laserBeamDisappearSequence.Append(_laserBeamBody.DOFillAmount(0f, 0.1f));

                laserBeamDisappearSequence.Append(_laserBeamHead.DOFillAmount(0f, 0.1f));

                laserBeamDisappearSequence.Append(ResetLaserCannonRotation());
            });
            #endregion

            return mainSequence;
        }

        private Tween RotateLaserCannonToHydrious(RectTransform hydriousRectTransform)
        {
            // Get direction vector in the same coordinate space
            Vector2 direction = hydriousRectTransform.position - _rectTransform.position;

            // Calculate the angle in degrees
            float angle = Mathf.Atan2(Mathf.Abs(direction.y), Mathf.Abs(direction.x)) * Mathf.Rad2Deg;

            if (IsTop())
                angle *= -1f;

            return RotateLaserCannon(angle);
        }

        private Tween ResetLaserCannonRotation()
            => RotateLaserCannon(0f);

        private Tween RotateLaserCannon(float angleZ)
            => _rectTransform.DOLocalRotate(new Vector3(0f, _idleRotY, angleZ), 0.5f, RotateMode.Fast);
    }
}