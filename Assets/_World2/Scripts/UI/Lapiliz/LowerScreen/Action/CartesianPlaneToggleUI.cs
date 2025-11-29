using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Toggle))]
    public class CartesianPlaneToggleUI : MonoBehaviour
    {
        private readonly Color UNTOGGLED_COLOR = Color.white;

        [SerializeField] private Toggle _toggle;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private Color _toggledColor;

        private LapilizManager _lapilizManager;

        private void Awake()
        {
            if (_toggle == null)
                _toggle = GetComponent<Toggle>();

            _toggle.isOn = false;
            _toggle.onValueChanged.AddListener(isOn =>
            {
                if (Hydros7WorldManager.Instance.IsGamePlaying() ||
                (Hydros7WorldManager.Instance.IsDisplayingTutorial() &&
                TutorialManager.Instance.IsDisplayingPlottingCartesianPlaneTutorial()))
                {
                    GridManager.Instance.ToggleCartesianPlane(isOn);
                    _toggle.image.color = isOn ? _toggledColor : UNTOGGLED_COLOR;
                    Hydros7WorldManager.Instance.TryDisplayTutorial(TutorialManager.Instance.TryDisplaySwitchToTab2Tutorial);
                }
            });
        }

        private void Start()
        {
            _toggle.image.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );

            if (LapilizManager.Instance != null)
            {
                _lapilizManager = LapilizManager.Instance;

                _lapilizManager.OnCartesianPlaneToggleRectGot
                    += LapilizManager_OnCartesianPlaneToggleRectGot;
            }
        }

        private void OnDestroy()
        {
            if (_lapilizManager != null)
            {
                _lapilizManager.OnCartesianPlaneToggleRectGot
                    -= LapilizManager_OnCartesianPlaneToggleRectGot;
            }
        }

        private Rect LapilizManager_OnCartesianPlaneToggleRectGot()
        {
            RectTransform toggleRectTransform = _toggle.image.rectTransform;
            return new Rect(toggleRectTransform.position, toggleRectTransform.sizeDelta);
        }

        public void EnableToggleInteractability()
            => SetToggleInteractability(true);

        public void DisableToggleInteractability()
            => SetToggleInteractability(false);

        private void SetToggleInteractability(bool isInteratable)
            => _toggle.interactable = isInteratable;
    }
}