using UnityEngine;

namespace Stellarfarer
{
    public class PointsManagerUI : MonoBehaviour
    {
        [SerializeField] private ChoiceSlotButtonUI choice_X1;
        [SerializeField] private ChoiceSlotButtonUI choice_Y1;
        [SerializeField] private ChoiceSlotButtonUI choice_X2;
        [SerializeField] private ChoiceSlotButtonUI choice_Y2;

        private PointsManager _pointsManager;

        private void Start()
        {
            if (PointsManager.Instance != null)
            {
                _pointsManager = PointsManager.Instance;

                _pointsManager.OnPointsGenerated
                    += PointsManager_OnPointsGenerated;
            }
        }

        private void OnDestroy()
        {
            if (_pointsManager != null)
            {
                _pointsManager.OnPointsGenerated
                    -= PointsManager_OnPointsGenerated;
            }
        }

        private void PointsManager_OnPointsGenerated()
        {
            _pointsManager.GetPoints(out Vector2 pointA, out Vector2 pointB);

            choice_X1.SetText(pointA.x.ToString());
            choice_Y1.SetText(pointA.y.ToString());
            choice_X2.SetText(pointB.x.ToString());
            choice_Y2.SetText(pointB.y.ToString());
        }
    }
}