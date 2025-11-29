using System;
using UnityEngine;

namespace Stellarfarer
{
    public class ProgressionManager : SingletonBehaviour<ProgressionManager>
    {
        public event Action OnPointsChanged;
        public event Func<Rect> OnProgressBarRectGot;

        private int _points;
        private Hydros7WorldManager _hydros7WorldManager;

        protected override void Awake()
        {
            base.Awake();

            _points = 0;
        }

        private void Start()
        {
            if (Hydros7WorldManager.Instance != null)
                _hydros7WorldManager = Hydros7WorldManager.Instance;
        }

        public void AddPoints(int pointsToAdd)
        {
            _points += pointsToAdd;
            OnPointsChanged?.Invoke();

            if (_points >= _hydros7WorldManager.GetRequiredPoints())
                _hydros7WorldManager.WinGame();
        }

        public float GetProgression()
        {
            int requiredPoints = _hydros7WorldManager.GetRequiredPoints();
            return (float)_points / requiredPoints;
        }

        public Rect GetProgressBarRect()
            => OnProgressBarRectGot?.Invoke() ?? Rect.zero;
    }
}