using UnityEngine;

namespace Stellarfarer
{
    public class CaptureManager : SingletonBehaviour<CaptureManager>
    {
        public event System.Action OnCapture;

        [SerializeField] private UnityEngine.UI.Image _holdingArea;
        [SerializeField] private Bounds _holdingAreaBounds;
        private Hydrious _hydriousTarget;

        protected override void Awake()
        {
            base.Awake();

            if (_holdingArea == null)
            {
                Debug.LogError($"Holding Area is null or uninitialized.");
                return;
            }
            else
            {
                Vector2 holdingAreaCenter = _holdingArea.GetActualVisibleSpriteCenter();
                Vector2 holdingAreaSize = _holdingArea.GetScaledVisibleSpriteSizeInUIUnits();
                _holdingAreaBounds = new Bounds(holdingAreaCenter, holdingAreaSize);
            }
        }

        public void Capture(Hydrious hydrious)
        {
            _hydriousTarget = hydrious;
            OnCapture?.Invoke();
        }

        public Hydrious GetHydriousTarget()
            => _hydriousTarget;

        public Bounds GetHoldingAreaBounds()
            => _holdingAreaBounds;
    }
}