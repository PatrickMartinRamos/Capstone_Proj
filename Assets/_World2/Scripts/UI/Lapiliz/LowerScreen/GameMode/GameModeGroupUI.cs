using UnityEngine;

namespace Stellarfarer
{
    public class GameModeGroupUI : MonoBehaviour
    {
        [SerializeField] private GameObject _plottingMode;
        [SerializeField] private GameObject _midpointDistanceMode;

        public void ShowPlottingMode()
        {
            _plottingMode.SetActive(true);
            _midpointDistanceMode.SetActive(false);
        }

        public void ShowMidpointDistanceMode()
        {
            PointsManager.Instance.GeneratePoints();
            _midpointDistanceMode.SetActive(true);
            _plottingMode.SetActive(false);
        }

        public void Show()
            => SetVisibility(true);

        public void Hide()
            => SetVisibility(false);

        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}