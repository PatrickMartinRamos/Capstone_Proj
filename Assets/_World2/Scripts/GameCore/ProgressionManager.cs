using System;

namespace Stellarfarer
{
    public class ProgressionManager : SingletonBehaviour<ProgressionManager>
    {
        public event Action OnPointsChanged;

        private int _points;
        private Hydros7GameManager _hydros7GameManager;

        protected override void Awake()
        {
            base.Awake();

            _points = 0;
        }

        private void Start()
        {
            if (Hydros7GameManager.Instance != null)
                _hydros7GameManager = Hydros7GameManager.Instance;
        }

        public void AddPoints(int pointsToAdd)
        {
            _points += pointsToAdd;
            OnPointsChanged?.Invoke();

            if (_points >= _hydros7GameManager.GetRequiredPoints())
                _hydros7GameManager.WinGame();
        }

        public float GetProgression()
        {
            int requiredPoints = _hydros7GameManager.GetRequiredPoints();
            return (float)_points / requiredPoints;
        }
    }
}