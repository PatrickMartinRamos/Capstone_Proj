using System;
using UnityEngine;

namespace Stellarfarer
{
    public class TutorialManager : SingletonBehaviour<TutorialManager>
    {
        private enum TutorialState
        {
            ChooseHydrious,
            ClickDashboard,
            Wait,
            PlottingCartesianPlane,
            SwitchToTab2,
            Plotting,
            SwitchToTab1,
            Confirm,
            Cleanse,
            Progression,
            Midpoint_Distance,
            Incorrect,
        }

        public event Action OnTutorialStateChanged;

        private TutorialState _tutorialState;
#if UNITY_EDITOR        
        [SerializeField] private bool _deleteKey;
#endif

        public void TryDisplayChooseHydriousTutorial()
            => SetTutorialState(TutorialState.ChooseHydrious);
        public bool IsDisplayingChooseHydriousTutorial()
            => IsTutorialState(TutorialState.ChooseHydrious);

        public void TryDisplayClickDashboardTutorial()
            => SetTutorialState(TutorialState.ClickDashboard);
        public bool IsDisplayingClickDashboardTutorial()
            => IsTutorialState(TutorialState.ClickDashboard);

        public void Wait()
            => SetTutorialState(TutorialState.Wait);
        public bool IsWaiting()
            => IsTutorialState(TutorialState.Wait);

        public void TryDisplayPlottingCartesianPlaneTutorial()
            => SetTutorialState(TutorialState.PlottingCartesianPlane);
        public bool IsDisplayingPlottingCartesianPlaneTutorial()
            => IsTutorialState(TutorialState.PlottingCartesianPlane);

        public void TryDisplaySwitchToTab2Tutorial()
            => SetTutorialState(TutorialState.SwitchToTab2);
        public bool IsDisplayingSwitchToTab2Tutorial()
            => IsTutorialState(TutorialState.SwitchToTab2);

        public void TryDisplayPlottingTutorial()
            => SetTutorialState(TutorialState.Plotting);
        public bool IsDisplayingPlottingTutorial()
            => IsTutorialState(TutorialState.Plotting);

        public void TryDisplaySwitchToTab1Tutorial()
            => SetTutorialState(TutorialState.SwitchToTab1);
        public bool IsDisplayingSwitchToTab1Tutorial()
            => IsTutorialState(TutorialState.SwitchToTab1);

        public void TryDisplayConfirmTutorial()
            => SetTutorialState(TutorialState.Confirm);
        public bool IsDisplayingConfirmTutorial()
            => IsTutorialState(TutorialState.Confirm);

        public void TryDisplayCleanseTutorial()
            => SetTutorialState(TutorialState.Cleanse);
        public bool IsDisplayingCleanseTutorial()
            => IsTutorialState(TutorialState.Cleanse);

        public void TryDisplayProgressionTutorial()
            => SetTutorialState(TutorialState.Progression);
        public bool IsDisplayingProgressionTutorial()
            => IsTutorialState(TutorialState.Progression);

        public void TryDisplayMidpointDistanceTutorial()
            => SetTutorialState(TutorialState.Midpoint_Distance);
        public bool IsDisplayingMidpointDistanceTutorial()
            => IsTutorialState(TutorialState.Midpoint_Distance);

        public void TryDisplayIncorrectTutorial()
            => SetTutorialState(TutorialState.Incorrect);
        public bool IsDisplayingIncorrectTutorial()
            => IsTutorialState(TutorialState.Incorrect);

        private void SetTutorialState(TutorialState tutorialState)
        {
            _tutorialState = tutorialState;
            OnTutorialStateChanged?.Invoke();
        }
        private bool IsTutorialState(TutorialState tutorialState)
            => _tutorialState == tutorialState;
    }
}