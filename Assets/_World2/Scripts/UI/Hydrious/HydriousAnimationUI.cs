using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(HydriousUI))]
    public class HydriousAnimationUI : MonoBehaviour
    {
        private enum LookState
        {
            Left,
            Right
        }

        [SerializeField] private HydriousUI _hydriousUI;
        [SerializeField] private Image _visual;
        [SerializeField] private Vector2 _floatTimeMax;
        [SerializeField] private Vector2 _swimTimeMax;
        [SerializeField] private Vector2 _flipTimeMax;

        private LookState _lookState = LookState.Left;
        private float _floatTimer = 0;
        private float _swimTimer = 0;
        private float _flipTimer = 0;
        private Vector2 _movePosition;
        private Sequence _sequence;
        private System.Random _rng;

        private void InitializeRandom()
        {
            int seed = (int)((System.DateTime.Now.Ticks + Random.Range(0, 100000)) & 0x0000FFFF) ^ GetInstanceID();
            _rng = new System.Random(seed);
        }

        private void Awake()
        {
            if (_hydriousUI == null)
                _hydriousUI = GetComponent<HydriousUI>();

            _hydriousUI.OnMovementStateChanged
                += Hydrious_OnMovementStateChanged;
            _hydriousUI.OnSpriteStateChanged
                += Hydrious_OnSpriteStateChanged;

            InitializeRandom();
            SetVisualAlphaHitTestMinimumThreshold();
            ResetHydriousMovement();
        }

        private void ResetAnimations()
        {
            if (!_hydriousUI.IsFloating())
                StopAllCoroutines();
            _sequence?.Kill();
            _visual.rectTransform.DOKill();
            _visual.rectTransform.rotation = Quaternion.identity;
        }

        private void ResetValues()
        {
            _floatTimer = 0f;
            _swimTimer = 0f;
            _flipTimer = 0f;
            _movePosition = _hydriousUI.GetRectTransform().anchoredPosition;
            if (!_hydriousUI.IsWaiting() && !_hydriousUI.IsReleasing())
                _lookState = LookState.Left;
        }

        private void ResetHydriousMovement()
        {
            ResetAnimations();
            ResetValues();
        }

        private void OnDestroy()
        {
            _hydriousUI.OnMovementStateChanged
                -= Hydrious_OnMovementStateChanged;
            _hydriousUI.OnSpriteStateChanged
                -= Hydrious_OnSpriteStateChanged;
        }

        #region Look State Management
        private void LookLeft()
            => SetLookState(LookState.Left);
        private bool IsLookingLeft()
            => IsLookState(LookState.Left);

        private void LookRight()
            => SetLookState(LookState.Right);
        private bool IsLookingRight()
            => IsLookState(LookState.Right);

        private void SetLookState(LookState lookState)
            => _lookState = lookState;
        private bool IsLookState(LookState lookState)
            => _lookState == lookState;
        #endregion

        #region On Sprite State Changed Event Handler
        private void Hydrious_OnSpriteStateChanged()
            => SetVisualSprite();

        public void SetVisualSprite()
        {
            if (IsLookingLeft())
                _visual.sprite = _hydriousUI.GetOverWorldSpriteLeft();
            else if (IsLookingRight())
                _visual.sprite = _hydriousUI.GetOverWorldSpriteRight();

            SetVisualAlphaHitTestMinimumThreshold();
        }

        private void SetVisualAlphaHitTestMinimumThreshold()
            => _visual.alphaHitTestMinimumThreshold = 0.1f;
        #endregion

        #region On Movement State Changed Event Handler
        private void Hydrious_OnMovementStateChanged()
        {
            if (_hydriousUI.IsFloating())
                StartCoroutine(Float());
            else if (_hydriousUI.IsSwimming())
                Swim();
            else if (_hydriousUI.IsWaiting())
                Wait();
            else if (_hydriousUI.IsCapturing())
                Capture();
            else if (_hydriousUI.IsReleasing())
                Release();
        }

        private System.Collections.IEnumerator Float()
        {
            ResetAnimations();
            yield return new WaitForSeconds(_floatTimer);
            _floatTimer = GetRandomMoveCooldownTime();
            _swimTimer = GetRandomMoveTime();
            _flipTimer = GetRandomRotationTime();
            _movePosition = GetRandomMovePosition();
            _hydriousUI.Swim();
        }

        private float GetRandomMoveCooldownTime()
            => GetRandomTime(_floatTimeMax);
        private float GetRandomMoveTime()
            => GetRandomTime(_swimTimeMax);
        private float GetRandomRotationTime()
            => GetRandomTime(_flipTimeMax);
        private float GetRandomTime(Vector2 time)
        {
            float randomValue = (float)_rng.NextDouble();
            float randomTime = Mathf.Lerp(time.x, time.y, randomValue);
            float decimalPrecision = 100f;
            return Mathf.Round(randomTime * decimalPrecision) / decimalPrecision;
        }

        private Vector2 GetRandomMovePosition()
        {
            Vector3[] worldCorners = _hydriousUI.GetParentWorldCorners();
            Vector2 hydriousSize = _hydriousUI.GetSize();

            // 0 = Bottom-Left, 2 = Top-Right
            Vector2 worldCornerStart = (Vector2)worldCorners[0];
            Vector2 worldCornerEnd = (Vector2)worldCorners[2];

            Vector2 posMin;
            Vector2 posMax;

            if (_hydriousUI.IsInOcean())
            {
                int maxValue = 100;
                int randomValue = GetRandomInt(maxValue);
                int outOfBoundThreshold = 50;

                if (randomValue < outOfBoundThreshold)
                    GetOutboundPosition(out posMin, out posMax, worldCornerStart, worldCornerEnd, hydriousSize);
                else
                    GetInboundPosition(out posMin, out posMax, worldCornerStart, worldCornerEnd, hydriousSize);
            }
            else if (_hydriousUI.IsInTank())
                GetInboundPosition(out posMin, out posMax, worldCornerStart, worldCornerEnd, hydriousSize);
            else
                GetInboundPosition(out posMin, out posMax, worldCornerStart, worldCornerEnd, hydriousSize);

            float randomPosX = GetRandomFloat(posMin.x, posMax.x);
            float randomPosY = GetRandomFloat(posMin.y, posMax.y);

            return new Vector2(randomPosX, randomPosY);
        }

        private int GetRandomInt(int maxValue)
            => _rng.Next(maxValue);

        private float GetRandomFloat(float minValue, float maxValue)
        {
            float randomValue = (float)_rng.NextDouble();
            return Mathf.Lerp(minValue, maxValue, randomValue);
        }

        private void GetOutboundPosition(out Vector2 posMin, out Vector2 posMax, Vector2 worldCornerStart, Vector2 worldCornerEnd, Vector2 hydriousSize)
        {
            posMin = GetPositionFromWorldCorner(worldCornerStart, -hydriousSize);
            posMax = GetPositionFromWorldCorner(worldCornerEnd, hydriousSize);
        }

        private void GetInboundPosition(out Vector2 posMin, out Vector2 posMax, Vector2 worldCornerStart, Vector2 worldCornerEnd, Vector2 hydriousSize)
        {
            Vector2 halvedHydriousSize = Utils.Halve(hydriousSize);
            posMin = GetPositionFromWorldCorner(worldCornerStart, halvedHydriousSize);
            posMax = GetPositionFromWorldCorner(worldCornerEnd, -halvedHydriousSize);
        }

        private Vector2 GetPositionFromWorldCorner(Vector2 worldCorner, Vector2 hydriousSize)
            => worldCorner + hydriousSize;

        private void Swim()
        {
            if (_hydriousUI.IsInOcean())
                _hydriousUI.SetAsLastSibling();

            _sequence = DOTween.Sequence();

            if (TryFlip(out Sequence flipSequence))
                _sequence.Append(flipSequence);

            _sequence.Append(_hydriousUI.GetRectTransform().DOMove(_movePosition, _swimTimer, true));
            _sequence.AppendCallback(() => _hydriousUI.Float());
            _sequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        private bool TryFlip(out Sequence flipSequence)
        {
            Vector2 hydriousWorldPosition = _hydriousUI.GetWorldPosition();
            RectTransform visualRectTransform = _visual.rectTransform;

            flipSequence = DOTween.Sequence();

            if (_movePosition.x > hydriousWorldPosition.x)
            {
                if (IsLookingLeft())
                    LookRight();
                else if (IsLookingRight())
                    return false;
            }
            else if (_movePosition.x < hydriousWorldPosition.x)
            {
                if (IsLookingRight())
                    LookLeft();
                else if (IsLookingLeft())
                    return false;
            }

            flipSequence.Append(visualRectTransform.DOLocalRotate(new Vector3(0f, 90f, 0f), _flipTimer));
            flipSequence.AppendCallback(() => SetVisualSprite());
            flipSequence.Append(visualRectTransform.DOLocalRotate(Vector3.zero, _flipTimer));
            flipSequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);

            return true;
        }

        private void Wait()
            => ResetHydriousMovement();

        private void Capture()
        {
            Vector3 intensity = Vector3.forward * 45f;   // max random tilt angle
            float duration = 0.3f;   // how long before repeating
            int vibrato = 10;        // number of shakes
            float randomness = 90f;
            ShakeRandomnessMode shakeRandomnessMode = ShakeRandomnessMode.Full;

            _visual.rectTransform
                .DOShakeRotation(duration, intensity, vibrato, randomness, false, shakeRandomnessMode)
                .SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear)
                .SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }

        private void Release()
        {
            ResetHydriousMovement();
            _swimTimer = GetRandomMoveTime();
            _flipTimer = GetRandomRotationTime();
            _movePosition = _hydriousUI.GetSpawnPosition();

            if (_hydriousUI.IsInOcean())
                _hydriousUI.SetAsLastSibling();

            _sequence = DOTween.Sequence();

            if (TryFlip(out Sequence flipSequence))
                _sequence.Append(flipSequence);

            _sequence.Append(_hydriousUI.GetRectTransform().DOMove(_movePosition, _swimTimer, true));
            _sequence.AppendCallback(() => _hydriousUI.DestroySelf());
            _sequence.SetLink(gameObject, LinkBehaviour.KillOnDestroy);
        }
        #endregion
    }
}