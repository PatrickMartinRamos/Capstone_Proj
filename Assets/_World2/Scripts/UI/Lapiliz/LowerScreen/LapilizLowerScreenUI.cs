using UnityEngine;

namespace Stellarfarer
{
    public class LapilizLowerScreenUI : MonoBehaviour
    {
        [SerializeField] private TabGroupUI _tabGroupUI;
        [SerializeField] private ActionGroupUI _actionGroupUI;
        [SerializeField] private GameModeGroupUI _gameModeGroupUI;

        private void Awake()
        {
            _tabGroupUI.OnTabSelectionChanged
                += TabGroupUI_OnTabSelectionChanged;
        }

        private void OnDestroy()
        {
            _tabGroupUI.OnTabSelectionChanged
                -= TabGroupUI_OnTabSelectionChanged;
        }

        private void TabGroupUI_OnTabSelectionChanged(bool tab1IsOn, bool tab2IsOn)
        {
            if (tab1IsOn)
                _actionGroupUI.Show();
            else
                _actionGroupUI.Hide();

            if (tab2IsOn)
                _gameModeGroupUI.Show();
            else
                _gameModeGroupUI.Hide();
        }

        public void SetPlottingMode()
        {
            _gameModeGroupUI.ShowPlottingMode();
            _actionGroupUI.EnableCartesianPlaneToggle();
        }

        public void SetMidpointDistanceMode()
        {
            _gameModeGroupUI.ShowMidpointDistanceMode();
            _actionGroupUI.DisableCartesianPlaneToggle();
        }
    }
}