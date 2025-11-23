using System;
using UnityEngine;

namespace Stellarfarer
{
    public class PointsManager : SingletonBehaviour<PointsManager>
    {
        public event Action OnPointsGenerated;

        private Vector2 _pointA;
        private Vector2 _pointB;
        private System.Random _rng;

        protected override void Awake()
        {
            base.Awake();

            InitializeRandom();
        }

        private void InitializeRandom()
        {
            int seed = (int)((DateTime.Now.Ticks + UnityEngine.Random.Range(0, 100000)) & 0x0000FFFF) ^ GetInstanceID();
            _rng = new System.Random(seed);
        }

        private int GetRandomInt(int minValue, int maxValue)
            => _rng.Next(minValue, maxValue);

        public void GeneratePoints()
        {
            int minValue;
            int maxValue;

            HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();

            switch (hydriousUI.GetSpecies())
            {
                default:
                case Species.Chelon:
                case Species.Selar:
                    minValue = -25;
                    maxValue = 25;
                    break;
                case Species.Carcinus:
                case Species.Valyn:
                    minValue = -50;
                    maxValue = 50;
                    break;
            }

            _pointA = new Vector2(GetRandomInt(minValue, maxValue), GetRandomInt(minValue, maxValue));
            _pointB = new Vector2(GetRandomInt(minValue, maxValue), GetRandomInt(minValue, maxValue));

            OnPointsGenerated?.Invoke();
        }

        public void GetPoints(out Vector2 pointA, out Vector2 pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}