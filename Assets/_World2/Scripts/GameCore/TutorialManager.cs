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
        }

        public event Action OnTutorialStateChanged;

        private TutorialState _tutorialState;

        public void ChooseHydrious()
            => SetTutorialState(TutorialState.ChooseHydrious);
        public bool HasChosenAHydrious()
            => IsTutorialState(TutorialState.ChooseHydrious);

        public void ClickDashboard()
            => SetTutorialState(TutorialState.ClickDashboard);
        public bool HasClickedDashboard()
            => IsTutorialState(TutorialState.ClickDashboard);

        private void SetTutorialState(TutorialState tutorialState)
        {
            _tutorialState = tutorialState;

            // TODO: FIX
            string keyName = tutorialState.ToString();
            PlayerPrefs.DeleteKey(keyName);
            if (!HasKeyInPlayerPref(keyName))
            {
                SetIntInPlayerPref(tutorialState.ToString(), 1);
                OnTutorialStateChanged?.Invoke();
            }
        }
        private bool IsTutorialState(TutorialState tutorialState)
            => _tutorialState == tutorialState;

        public void TryDisplay()
        {
            int stageID = Hydros7WorldManager.Instance.GetStageID();

            if (stageID != 1 && stageID != 3 && stageID != 5)
            {
                Hydros7WorldManager.Instance.Countdown();
                return;
            }

            string keyName = TutorialState.ChooseHydrious.ToString();
            PlayerPrefs.DeleteKey(keyName);
            if (HasKeyInPlayerPref(keyName))
            {
                if (GetIntFromPlayerPref(keyName) == 1)
                    return;
                else
                    ChooseHydrious();
            }
            else
            {
                SetIntInPlayerPref(keyName, 0);
                ChooseHydrious();
            }
        }

        private void SetIntInPlayerPref(string key, int value)
            => PlayerPrefs.SetInt(key, value);

        private int GetIntFromPlayerPref(string key)
            => PlayerPrefs.GetInt(key);

        private bool HasKeyInPlayerPref(string key)
            => PlayerPrefs.HasKey(key);
    }
}