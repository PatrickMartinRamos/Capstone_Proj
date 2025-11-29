using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Toggle))]
    public class TabUI : MonoBehaviour
    {
        public event Action OnIsOnChanged;

        [SerializeField] private Toggle _toggle;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        private Color _toggledColor = Color.darkSlateGray;
        private Color _untoggledColor = Color.white;

        private void Awake()
        {
            if (_toggle == null)
                _toggle = GetComponent<Toggle>();

            _toggle.onValueChanged.AddListener(isOn =>
            {
                if (Hydros7WorldManager.Instance.IsGameInitializing() ||
                Hydros7WorldManager.Instance.IsGamePlaying() ||
                (Hydros7WorldManager.Instance.IsDisplayingTutorial() && (
                TutorialManager.Instance.IsDisplayingSwitchToTab1Tutorial() ||
                TutorialManager.Instance.IsDisplayingSwitchToTab2Tutorial() ||
                TutorialManager.Instance.IsWaiting() ||
                TutorialManager.Instance.IsDisplayingIncorrectTutorial()
                )))
                {
                    if (isOn)
                    {
                        ToggledColor();

                        OnIsOnChanged?.Invoke();
                    }
                    else
                        UntoggledColor();
                }
            });
        }

        private void Start()
            => _toggle.image.ConfigureImageFromFullToVisibleSprite(
                    full: _full,
                    visible: _visible
                );

        public bool IsOn()
            => _toggle.isOn;

        public void On()
            => SetIsOn(true);

        public void Off()
            => SetIsOn(false);

        private void SetIsOn(bool isOn)
            => _toggle.isOn = isOn;

        private void ToggledColor()
            => SetColor(_toggledColor);

        private void UntoggledColor()
            => SetColor(_untoggledColor);

        private void SetColor(Color color)
            => _toggle.image.color = color;

        public Rect GetRect()
        {
            RectTransform screenImageRectTransform = _toggle.image.rectTransform;
            return new Rect(screenImageRectTransform.position, screenImageRectTransform.sizeDelta);
        }
    }
}