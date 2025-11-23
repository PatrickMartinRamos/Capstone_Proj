using System;
using UnityEngine;

namespace Stellarfarer
{
    [RequireComponent(typeof(RectTransform))]
    public class HydriousUI : MonoBehaviour
    {
        private enum LocationState
        {
            Ocean,
            Tank,
        }

        private enum MovementState
        {
            Float,
            Swim,
            Wait,
            Capture,
            Release
        }

        private enum SpriteState
        {
            Mutate,
            Stun,
            Cleanse
        }

        public event Action OnMovementStateChanged;
        public event Action OnSpriteStateChanged;

        [SerializeField] private HydriousSO _hydriousSO;

        private RectTransform _rectTransform;
        private RectTransform _parent;
        private LocationState _locationState = LocationState.Ocean;
        private MovementState _movementState = MovementState.Float;
        private SpriteState _spriteState = SpriteState.Mutate;
        private (Sprite left, Sprite right) _overWorldSprite;
        private Vector2 _spawnPosition;

        private void Awake()
            => _rectTransform = (RectTransform)transform;

        private void Start()
        {
            Mutate();
            PlaceInOcean();
            Float();
        }

        public static HydriousUI SpawnHydriousUI(
            HydriousSO hydriousSO,
            RectTransform spawnArea,
            Vector2 spawnPosition)
        {
            RectTransform hydriousRectTransform = Instantiate(hydriousSO.Prefab);
            hydriousRectTransform.SetAsFirstSibling();

            HydriousUI hydriousUI = hydriousRectTransform.GetComponent<HydriousUI>();
            hydriousUI.SetParent(spawnArea, spawnPosition);
            hydriousUI.SetSpawnPosition(spawnPosition);

            return hydriousUI;
        }

        private void SetSpawnPosition(Vector2 spawnPosition)
            => _spawnPosition = spawnPosition;

        public void SetParent(RectTransform parent, Vector2 worldPosition)
        {
            _parent = parent;
            _rectTransform.SetParent(parent, false);
            SetPositionAndRotation(worldPosition, Quaternion.identity);
        }

        public Vector3[] GetParentWorldCorners()
        {
            Vector3[] worldCorners = new Vector3[4];
            _parent.GetWorldCorners(worldCorners);
            return worldCorners;
        }

        public void SetAsLastSibling()
            => _rectTransform.SetAsLastSibling();

        #region Location State Management
        public void PlaceInOcean()
            => SetLocationState(LocationState.Ocean);
        public bool IsInOcean()
            => IsLocationState(LocationState.Ocean);

        public void PlaceInTank()
            => SetLocationState(LocationState.Tank);
        public bool IsInTank()
            => IsLocationState(LocationState.Tank);

        private void SetLocationState(LocationState locationState)
            => _locationState = locationState;
        private bool IsLocationState(LocationState locationState)
            => _locationState == locationState;
        #endregion

        #region Movement State Management
        public void Float()
            => SetMovementState(MovementState.Float);
        public bool IsFloating()
            => IsMovementState(MovementState.Float);

        public void Swim()
            => SetMovementState(MovementState.Swim);
        public bool IsSwimming()
            => IsMovementState(MovementState.Swim);

        public void Wait()
            => SetMovementState(MovementState.Wait);
        public bool IsWaiting()
            => IsMovementState(MovementState.Wait);

        public void Capture()
            => SetMovementState(MovementState.Capture);
        public bool IsCapturing()
            => IsMovementState(MovementState.Capture);

        public void Release()
            => SetMovementState(MovementState.Release);
        public bool IsReleasing()
            => IsMovementState(MovementState.Release);

        private void SetMovementState(MovementState movementState)
        {
            _movementState = movementState;
            OnMovementStateChanged?.Invoke();
        }
        private bool IsMovementState(MovementState movementState)
            => _movementState == movementState;
        #endregion

        #region Sprite State Management
        public void Mutate()
        {
            SetOverworldSprite(
                _hydriousSO.MutatedSpriteLeft,
                _hydriousSO.MutatedSpriteRight);
            SetSpriteState(SpriteState.Mutate);
        }
        public bool IsMutated()
            => IsSpriteState(SpriteState.Mutate);

        public void Stun()
        {
            SetOverworldSprite(
                _hydriousSO.DizzySpriteLeft,
                _hydriousSO.DizzySpriteRight);
            SetSpriteState(SpriteState.Stun);
        }
        public bool IsStunned()
            => IsSpriteState(SpriteState.Stun);

        public void Cleanse()
        {
            SetOverworldSprite(
                _hydriousSO.CleansedSpriteLeft,
                _hydriousSO.CleansedSpriteRight);
            SetSpriteState(SpriteState.Cleanse);
        }
        public bool IsCleansed()
            => IsSpriteState(SpriteState.Cleanse);

        private void SetSpriteState(SpriteState spriteState)
        {
            _spriteState = spriteState;
            OnSpriteStateChanged?.Invoke();
        }
        private bool IsSpriteState(SpriteState spriteState)
            => _spriteState == spriteState;

        private void SetOverworldSprite(Sprite leftSprite, Sprite rightSprite)
        {
            _overWorldSprite.left = leftSprite;
            _overWorldSprite.right = rightSprite;
        }
        public Sprite GetOverWorldSpriteLeft()
            => _overWorldSprite.left;
        public Sprite GetOverWorldSpriteRight()
            => _overWorldSprite.right;

        public Sprite GetXRaySprite()
            => _hydriousSO.XRaySprite;
        #endregion

        public CleansingType GetCleansingType()
            => _hydriousSO.CleansingType;

        public int GetHydrionEnergyAmountGiven()
            => _hydriousSO.HydrionEnergyAmountGiven;

        public Species GetSpecies()
            => _hydriousSO.Species;

        public Color GetColor()
            => _hydriousSO.Color;

        public void DestroySelf()
            => Destroy(gameObject); // TODO: REMOVE NULL IN LSIT AND ADD TO PROGRESSION

        public RectTransform GetRectTransform()
            => _rectTransform;

        public Vector2 GetSize()
            => _rectTransform.rect.size;

        public Vector2 GetWorldPosition()
            => _rectTransform.position;

        public Vector2 GetSpawnPosition()
            => _spawnPosition;

        public void SetPositionAndRotation(Vector2 position, Quaternion rotation)
            => _rectTransform.SetPositionAndRotation(position, rotation);
    }
}