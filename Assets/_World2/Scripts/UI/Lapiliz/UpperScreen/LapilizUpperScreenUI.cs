using UnityEngine;

namespace Stellarfarer
{
    public class LapilizUpperScreenUI : MonoBehaviour
    {
        [SerializeField] private PlottingUI _plottingUI;
        [SerializeField] private MidpointUI _midpointUI;
        [SerializeField] private DistanceUI _distanceUI;

        public void SetPlottingMode()
        {
            _midpointUI.Hide();
            _distanceUI.Hide();
            _plottingUI.Show();
            _plottingUI.SetPlottingUI(CaptureManager.Instance.GetHydriousUITarget());
        }

        public void ResetPlottingMode()
            => _plottingUI.ResetPlottingUI();

        public void SetMidpointMode()
        {
            _plottingUI.Hide();
            _distanceUI.Hide();
            _midpointUI.Show();
        }

        public void ResetMidpointMode()
            => _midpointUI.ResetMidpointUI();

        public void SetDistanceMode()
        {
            _plottingUI.Hide();
            _midpointUI.Hide();
            _distanceUI.Show();
        }

        public void ResetDistanceMode()
            => _distanceUI.ResetDistanceUI();
    }
}