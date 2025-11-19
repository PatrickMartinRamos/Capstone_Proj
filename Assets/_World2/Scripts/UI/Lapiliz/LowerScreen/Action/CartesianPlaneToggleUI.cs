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

        private void Awake()
        {
            if (_toggle == null)
                _toggle = GetComponent<Toggle>();

            _toggle.isOn = false;
            _toggle.onValueChanged.AddListener(isOn =>
            {
                GridManager.Instance.ToggleCartesianPlane(isOn);
                _toggle.image.color = isOn ? _toggledColor : UNTOGGLED_COLOR;
            });
        }

        private void Start()
            => _toggle.image.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );

        public void EnableToggleInteractability()
            => SetToggleInteractability(true);

        public void DisableToggleInteractability()
            => SetToggleInteractability(false);

        private void SetToggleInteractability(bool isInteratable)
            => _toggle.interactable = isInteratable;
    }
}