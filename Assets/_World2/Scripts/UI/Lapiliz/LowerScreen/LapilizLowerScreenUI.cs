using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Image))]
    public class LapilizLowerScreenUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private TabGroupUI _tabGroupUI;
        [SerializeField] private ActionGroupUI _actionGroupUI;
        [SerializeField] private GameModeGroupUI _gameModeGroupUI;

        private LapilizManager _lapilizManager;

        private void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();

            _tabGroupUI.OnTabSelectionChanged
                += TabGroupUI_OnTabSelectionChanged;
        }

        private void Start()
        {
            if (LapilizManager.Instance != null)
            {
                _lapilizManager = LapilizManager.Instance;

                _lapilizManager.OnLowerScreenRectGot
                    += LapilizManager_OnLowerScreenRectGot;
            }
        }

        private void OnDestroy()
        {
            _tabGroupUI.OnTabSelectionChanged
                -= TabGroupUI_OnTabSelectionChanged;

            if (_lapilizManager != null)
            {
                _lapilizManager.OnLowerScreenRectGot
                    -= LapilizManager_OnLowerScreenRectGot;
            }
        }

        private Rect LapilizManager_OnLowerScreenRectGot()
        {
            _visual.ConfigureImageFromFullToVisibleSprite(_full, _visible);

            RectTransform visualRectTransform = _visual.rectTransform;
            Rect rect = new Rect(visualRectTransform.position, visualRectTransform.sizeDelta);

            _visual.ConfigureImageAsFullSprite(_full);
            return rect;
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