using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class CaptureManager : SingletonBehaviour<CaptureManager>
    {
        public event Action OnCapture;
        public event Action OnCleanse;

        [SerializeField] private Image _holdingArea;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        private HydriousUI _hydriousUITarget;

        protected override void Awake()
        {
            base.Awake();

            if (_holdingArea == null)
            {
                Debug.LogError($"Holding Area is null or uninitialized.");
                return;
            }
        }

        private void Start()
            => _holdingArea.ConfigureImageFromFullToVisibleSprite(
                    full: _full,
                    visible: _visible
                );

        private bool HasHydriousUITarget()
            => _hydriousUITarget != null;

        public bool TryCapture(HydriousUI hydriousUI)
        {
            if (HasHydriousUITarget())
                return false;

            SetHydriousUITarget(hydriousUI);
            OnCapture?.Invoke();

            return true;
        }

        private void SetHydriousUITarget(HydriousUI hydriousUI)
            => _hydriousUITarget = hydriousUI;

        public void ClearHydriousUITarget()
            => SetHydriousUITarget(null);

        public HydriousUI GetHydriousUITarget()
            => _hydriousUITarget;

        public RectTransform GetHoldingAreaRectTransform()
            => _holdingArea.rectTransform;

        public void Cleanse()
            => OnCleanse?.Invoke();
    }
}