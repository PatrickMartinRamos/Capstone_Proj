using TMPro;
using UnityEngine;

namespace Stellarfarer
{
    public class TutorialManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject _tutorialCanvas;
        [SerializeField] private GameObject _darkOverlayCanvas;
        [SerializeField] private GameObject _maskAreaCavnas;
        [SerializeField] private TutorialOverlayUI _maskAreaUI_1;
        [SerializeField] private TutorialOverlayUI _maskAreaUI_2;
        [SerializeField] private DarkOverlayUI _darkOverlayUI_1;
        [SerializeField] private DarkOverlayUI _darkOverlayUI_2;
        [SerializeField] private RectTransform _container_1;
        [SerializeField] private RectTransform _container_2;
        [SerializeField] private TextMeshProUGUI _tutorialMessage_1;
        [SerializeField] private RectTransform _tutorialMessage_2;

        private TutorialManager _tutorialManager;
        private Hydros7WorldManager _hydros7WorldManager;

        private void Start()
        {
            if (TutorialManager.Instance != null)
            {
                _tutorialManager = TutorialManager.Instance;

                _tutorialManager.OnTutorialStateChanged
                    += TutorialManager_OnTutorialStateChanged;
            }

            if (Hydros7WorldManager.Instance != null)
            {
                _hydros7WorldManager = Hydros7WorldManager.Instance;

                _hydros7WorldManager.OnGamePauseToggled
                    += Hydros7WorldManager_OnGamePauseToggled;
            }

            HideAllCanvas();
        }

        private void OnDestroy()
        {
            if (_tutorialManager != null)
            {
                _tutorialManager.OnTutorialStateChanged
                    -= TutorialManager_OnTutorialStateChanged;
            }

            if (_hydros7WorldManager != null)
            {
                _hydros7WorldManager.OnGamePauseToggled
                    -= Hydros7WorldManager_OnGamePauseToggled;
            }
        }

        private void Hydros7WorldManager_OnGamePauseToggled()
        {
            if (!_hydros7WorldManager.IsDisplayingTutorial())
                return;

            if (_hydros7WorldManager.IsGamePaused())
                HideAllCanvas();
            else
                ShowAllCanvas();
        }

        private void HideAllCanvas()
        {
            HideCanvas(_tutorialCanvas);
            HideCanvas(_darkOverlayCanvas);
            HideCanvas(_maskAreaCavnas);
        }

        private void ShowAllCanvas()
        {
            ShowCanvas(_tutorialCanvas);
            ShowCanvas(_darkOverlayCanvas);
            ShowCanvas(_maskAreaCavnas);
        }

        private const float REF_WIDTH = 1080f;
        private const float REF_HEIGHT = 1920f;

        private float ScaleX(float x)
        {
            RectTransform tutorialCanvasRect = _tutorialCanvas.transform as RectTransform;
            return tutorialCanvasRect.rect.width * (x / REF_WIDTH);
        }
        private float ScaleY(float y)
        {
            RectTransform tutorialCanvasRect = _tutorialCanvas.transform as RectTransform;
            return tutorialCanvasRect.rect.height * (y / REF_HEIGHT);
        }

        private void TutorialManager_OnTutorialStateChanged()
        {
            ShowAllCanvas();

            Vector2 pos_1 = Vector2.zero;
            Vector2 size_1 = Vector2.one * 100f;
            Vector2 pos_2 = Vector2.zero;
            Vector2 size_2 = Vector2.one * 100f;
            string tutorialMessage = "";
            bool c2 = false;
            RectTransform tutorialCanvasRect = _tutorialCanvas.transform as RectTransform;
            float canvasHeight = tutorialCanvasRect.rect.height;
            float halvedCanvasHeight = Utils.Halve(canvasHeight);

            if (_tutorialManager.IsDisplayingChooseHydriousTutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(-760f));
                size_1 = new Vector2(ScaleX(750), ScaleY(360f));
                tutorialMessage = "Click on a Hydrious!";

                _maskAreaUI_1.DisplayChooseHydriousTutorial();
                _darkOverlayUI_1.DisplayChooseHydriousTutorial();
                _maskAreaUI_2.Hide();
                _darkOverlayUI_2.Hide();
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingClickDashboardTutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(-760f));
                size_1 = new Vector2(ScaleX(750), ScaleY(360f));
                tutorialMessage = "Click on the Dashboard!";

                _maskAreaUI_1.DisplayClickDashboardTutorial();
                _darkOverlayUI_1.DisplayClickDashboardTutorial();
                _maskAreaUI_2.Hide();
                _darkOverlayUI_2.Hide();
                c2 = false;
            }
            else if (_tutorialManager.IsWaiting())
            {
                _maskAreaUI_1.Hide();
                _darkOverlayUI_1.Hide();
                _maskAreaUI_2.Hide();
                _darkOverlayUI_2.Hide();
                HideAllCanvas();
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingPlottingCartesianPlaneTutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(-760f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(360f));
                tutorialMessage = "Toggle to Show and Hide the Cartesian Plane!";

                _maskAreaUI_1.DisplayPlottingCartesianPlaneTutorial();
                _darkOverlayUI_1.DisplayBottom(halvedCanvasHeight);
                _maskAreaUI_2.DisplayTop(halvedCanvasHeight);
                _darkOverlayUI_2.DisplayTop(halvedCanvasHeight);
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingSwitchToTab2Tutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(160f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(360f));
                tutorialMessage = "Switch to Tab 2!";

                _maskAreaUI_1.DisplaySwitchToTab2Tutorial();
                _darkOverlayUI_1.DisplaySwitchToTab2Tutorial();
                _maskAreaUI_2.DisplayLapilizLowerScreen();
                _darkOverlayUI_2.Hide();
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingPlottingTutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(-760f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(360f));
                tutorialMessage = "Navigate the target using the arrows and hover over an Azuliuz!";

                _maskAreaUI_1.DisplayLapilizLowerScreen();
                _darkOverlayUI_1.DisplayBottom(halvedCanvasHeight);
                _maskAreaUI_2.DisplayTop(halvedCanvasHeight);
                _darkOverlayUI_2.DisplayTop(halvedCanvasHeight);
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingSwitchToTab1Tutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(160f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(360f));
                tutorialMessage = "Switch to Tab 1!";

                _maskAreaUI_1.DisplaySwitchToTab1Tutorial();
                _darkOverlayUI_1.DisplaySwitchToTab1Tutorial();
                _maskAreaUI_2.DisplayLapilizLowerScreen();
                _darkOverlayUI_2.Hide();
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingConfirmTutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(-760f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(360f));

                HydriousUI hydriousUI = CaptureManager.Instance.GetHydriousUITarget();

                tutorialMessage = hydriousUI.GetCleansingType() == CleansingType.Plotting ?
                "If target's tile has an Azuliuz, then Mark. If all Azuliuz are marked, then Cleanse!" :
                "Check if the values are correct!";

                _maskAreaUI_1.DisplayConfirmTutorial();
                _darkOverlayUI_1.DisplayBottom(halvedCanvasHeight);
                _maskAreaUI_2.DisplayTop(halvedCanvasHeight);
                _darkOverlayUI_2.DisplayTop(halvedCanvasHeight);
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingCleanseTutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(680f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(360f));
                tutorialMessage = "Confirm Cleanse!";

                _maskAreaUI_1.DisplayCleanseTutorial();
                _darkOverlayUI_1.DisplayCleanseTutorial();
                _maskAreaUI_2.Hide();
                _darkOverlayUI_2.Hide();
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingProgressionTutorial())
            {
                pos_1 = new Vector2(ScaleX(0f), ScaleY(250f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(360f));
                tutorialMessage = "Keep on cleansing Hydrious to fill up the Power Meter and Win!";

                _maskAreaUI_1.DisplayProgressionTutorial();
                _darkOverlayUI_1.DisplayProgressionTutorial();
                _maskAreaUI_2.Hide();
                _darkOverlayUI_2.Hide();
                c2 = false;
            }
            else if (_tutorialManager.IsDisplayingMidpointDistanceTutorial())
            {
                pos_1 = new Vector2(ScaleX(50f), ScaleY(800f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(300f));
                tutorialMessage = "Input the corresponding value of the ordered pairs to the empty slots and solve.";

                pos_2 = new Vector2(ScaleX(0f), ScaleY(-765f));
                size_2 = new Vector2(ScaleX(750f), ScaleY(360f));

                _maskAreaUI_1.DisplayLapilizLowerScreen();
                _darkOverlayUI_1.DisplayMidpointTutorial();
                _maskAreaUI_2.DisplayLapilizUpperScreen();
                _darkOverlayUI_2.Hide();
                c2 = true;
            }
            else if (_tutorialManager.IsDisplayingIncorrectTutorial())
            {
                pos_1 = new Vector2(ScaleX(50f), ScaleY(800f));
                size_1 = new Vector2(ScaleX(750f), ScaleY(300f));
                tutorialMessage = "Incorrect values, please input again";

                pos_2 = new Vector2(ScaleX(0f), ScaleY(-765f));
                size_2 = new Vector2(ScaleX(750f), ScaleY(360f));

                _maskAreaUI_1.DisplayLapilizLowerScreen();
                _darkOverlayUI_1.DisplayMidpointTutorial();
                _maskAreaUI_2.DisplayLapilizUpperScreen();
                _darkOverlayUI_2.Hide();
                c2 = true;
            }

            if (!_tutorialManager.IsWaiting())
            {
                _container_1.anchoredPosition = pos_1;
                _container_1.sizeDelta = size_1;
                _tutorialMessage_1.text = tutorialMessage;
                _tutorialMessage_1.margin = new Vector4(
                    size_1.x * 0.1f,
                    size_1.y * 0.125f,
                    size_1.x * 0.1f,
                    size_1.y * 0.125f);

                _container_2.gameObject.SetActive(c2);
                if (c2)
                {
                    _container_2.anchoredPosition = pos_2;
                    _container_2.sizeDelta = size_2;
                    _tutorialMessage_2.offsetMin = new Vector2(
                        size_2.x * 0.1f,
                        size_2.y * 0.125f);
                    _tutorialMessage_2.offsetMax = new Vector2(
                        -size_2.x * 0.1f,
                        -size_2.y * 0.125f);
                }
            }
        }

        private void ShowCanvas(GameObject canvas)
            => SetCanvasVisibility(canvas, true);

        private void HideCanvas(GameObject canvas)
            => SetCanvasVisibility(canvas, false);

        private void SetCanvasVisibility(GameObject canvas, bool isVisible)
            => canvas.SetActive(isVisible);
    }
}