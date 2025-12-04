using System;
using UnityEngine;

namespace Stellarfarer
{
    public class TabGroupUI : MonoBehaviour
    {
        public event Action<bool, bool> OnTabSelectionChanged;

        [SerializeField] private TabUI _tab1;
        [SerializeField] private TabUI _tab2;

        private LapilizManager _lapilizManager;

        private void Awake()
        {
            _tab1.OnIsOnChanged
                += HandleTabChange;
            _tab2.OnIsOnChanged
                += HandleTabChange;
        }

        private void Start()
        {
            _tab1.Off();
            _tab1.On();

            if (LapilizManager.Instance != null)
            {
                _lapilizManager = LapilizManager.Instance;

                _lapilizManager.OnTab2RectGot
                    += LapilizManager_OnTab2RectGot;
                _lapilizManager.OnTab1RectGot
                    += LapilizManager_OnTab1RectGot;
            }
        }

        private void OnDestroy()
        {
            _tab1.OnIsOnChanged
                -= HandleTabChange;
            _tab2.OnIsOnChanged
                -= HandleTabChange;

            if (_lapilizManager != null)
            {
                _lapilizManager.OnTab2RectGot
                    -= LapilizManager_OnTab2RectGot;
                _lapilizManager.OnTab1RectGot
                    -= LapilizManager_OnTab1RectGot;
            }
        }

        private Rect LapilizManager_OnTab2RectGot()
            => _tab2.GetRect();

        private Rect LapilizManager_OnTab1RectGot()
            => _tab1.GetRect();

        private void HandleTabChange()
        {
            if (_tab1.IsOn())
            {
                OnTabSelectionChanged?.Invoke(true, false);

                Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplayConfirmTutorial);
            }
            else if (_tab2.IsOn())
            {
                OnTabSelectionChanged?.Invoke(false, true);

                HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();

                if (hydriousUI == null)
                    return;

                switch (hydriousUI.GetCleansingType())
                {
                    case CleansingType.Plotting:
                        Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplayPlottingTutorial);
                        break;
                    case CleansingType.Midpoint:
                    case CleansingType.Distance:
                        Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplayMidpointDistanceTutorial);
                        break;
                }
            }

        }
    }
}