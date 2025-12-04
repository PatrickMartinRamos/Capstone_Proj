using DG.Tweening;
using UnityEngine;

namespace Stellarfarer
{
    public class LapilizManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private LapilizScreenGroupUI _lapilizScreenGroupUI;
        [SerializeField] private CleanseButtonUI _cleanseButtonUI;

        private LapilizManager _lapilizManager;

        private void Start()
        {
            if (LapilizManager.Instance != null)
            {
                _lapilizManager = LapilizManager.Instance;

                _lapilizManager.OnActivationChanged
                    += LapilizManager_OnActivationChanged;
                _lapilizManager.OnExtractStarted
                    += LapilizManager_OnExtractStarted;
            }

            HideCanvas();
        }

        private void OnDestroy()
        {
            if (_lapilizManager != null)
            {
                _lapilizManager.OnActivationChanged
                    -= LapilizManager_OnActivationChanged;
                _lapilizManager.OnExtractStarted
                    -= LapilizManager_OnExtractStarted;
            }
        }

        private void LapilizManager_OnExtractStarted()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.Join(_lapilizScreenGroupUI.MoveLeft());
            sequence.Join(_cleanseButtonUI.MoveCenter());
            sequence.AppendCallback(() => Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplayCleanseTutorial));
        }

        private void LapilizManager_OnActivationChanged()
        {
            HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();

            if (_lapilizManager.IsActivated())
            {
                ShowCanvas();

                switch (hydriousUI.GetCleansingType())
                {
                    case CleansingType.Plotting:
                        _lapilizScreenGroupUI.SetPlottingMode();
                        break;
                    case CleansingType.Midpoint:
                        _lapilizScreenGroupUI.SetMidpointMode();
                        break;
                    case CleansingType.Distance:
                        _lapilizScreenGroupUI.SetDistanceMode();
                        break;
                }

                _cleanseButtonUI.MoveRight();
            }
            else if (_lapilizManager.IsDeactivated())
            {
                switch (hydriousUI.GetCleansingType())
                {
                    case CleansingType.Plotting:
                        _lapilizScreenGroupUI.ResetPlottingMode();
                        break;
                    case CleansingType.Midpoint:
                        _lapilizScreenGroupUI.ResetMidpoindMode();
                        break;
                    case CleansingType.Distance:
                        _lapilizScreenGroupUI.ResetDistanceMode();
                        break;
                }

                _lapilizScreenGroupUI.MoveCenter();
                _cleanseButtonUI.MoveCenter();
                HideCanvas();
            }
        }

        private void ShowCanvas()
            => SetCanvasVisibility(true);
        private void HideCanvas()
            => SetCanvasVisibility(false);
        private void SetCanvasVisibility(bool isVisible)
            => _canvas.SetActive(isVisible);
    }
}