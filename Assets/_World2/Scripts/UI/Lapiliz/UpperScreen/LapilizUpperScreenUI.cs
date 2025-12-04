using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class LapilizUpperScreenUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private PlottingUI _plottingUI;
        [SerializeField] private MidpointUI _midpointUI;
        [SerializeField] private DistanceUI _distanceUI;

        private LapilizManager _lapilizManager;

        private void Start()
        {
            if (LapilizManager.Instance != null)
            {
                _lapilizManager = LapilizManager.Instance;

                _lapilizManager.OnUpperScreenRectGot
                    += LapilizManager_OnUpperScreenRectGot;
            }
        }

        private void OnDestroy()
        {
            if (_lapilizManager != null)
            {
                _lapilizManager.OnUpperScreenRectGot
                    -= LapilizManager_OnUpperScreenRectGot;
            }
        }

        private Rect LapilizManager_OnUpperScreenRectGot()
        {
            _visual.ConfigureImageFromFullToVisibleSprite(_full, _visible);

            RectTransform visualRectTransform = _visual.rectTransform;
            Rect rect = new Rect(visualRectTransform.position, visualRectTransform.sizeDelta);

            _visual.ConfigureImageAsFullSprite(_full);
            return rect;
        }

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