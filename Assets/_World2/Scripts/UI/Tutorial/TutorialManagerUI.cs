using TMPro;
using UnityEngine;

namespace Stellarfarer
{
    public class TutorialManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private TutorialOverlayUI _maskAreaUI;
        [SerializeField] private DarkOverlayUI _darkOverlayUI;
        [SerializeField] private RectTransform _container;
        [SerializeField] private TextMeshProUGUI _tutorialMessage;

        private TutorialManager _tutorialManager;

        private void Start()
        {
            if (TutorialManager.Instance != null)
            {
                _tutorialManager = TutorialManager.Instance;

                _tutorialManager.OnTutorialStateChanged
                    += TutorialManager_OnTutorialStateChanged;
            }

            HideCanvas();
        }

        private void OnDestroy()
        {
            if (_tutorialManager != null)
            {
                _tutorialManager.OnTutorialStateChanged
                    -= TutorialManager_OnTutorialStateChanged;
            }
        }

        private void TutorialManager_OnTutorialStateChanged()
        {
            ShowCanvas();

            Vector2 position = Vector2.zero;
            Vector2 size = Vector2.one * 100f;
            string tutorialMessage = "";

            if (_tutorialManager.HasChosenAHydrious())
            {
                position = new Vector2(0f, -140f);
                size = new Vector2(745f, 360);
                tutorialMessage = "Click on a Hydrious!";

                _maskAreaUI.DisplayChooseHydriousTutorial();
                _darkOverlayUI.DisplayChooseHydriousTutorial();
            }
            else if (_tutorialManager.HasClickedDashboard())
            {
                position = new Vector2(0f, -140f);
                size = new Vector2(745f, 360);
                tutorialMessage = "Click on the Dashboard!";
            }

            _container.anchoredPosition = position;
            _container.sizeDelta = size;
            _tutorialMessage.text = tutorialMessage;
        }

        private void ShowCanvas()
            => SetCanvasVisibility(true);

        private void HideCanvas()
            => SetCanvasVisibility(false);

        private void SetCanvasVisibility(bool isVisible)
            => _canvas.SetActive(isVisible);
    }
}