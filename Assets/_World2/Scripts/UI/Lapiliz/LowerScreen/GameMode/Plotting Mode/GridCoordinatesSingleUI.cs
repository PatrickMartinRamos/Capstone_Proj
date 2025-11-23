using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class GridCoordinatesSingleUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private Sprite _textContainer;
        [SerializeField] private TextMeshProUGUI _coordinateText;

        private void Awake()
            => _coordinateText.text = "0";

        private void Start()
        {
            _visual.sprite = _textContainer;

            Vector2 textContainerCenter = _visual.GetScaledVisibleSpriteCenter();
            Vector2 textContainerSize = _visual.GetScaledVisibleSpriteSizeInUIUnits();
            RectTransform coordinateTextRectTransform = _coordinateText.rectTransform;
            coordinateTextRectTransform.anchoredPosition = textContainerCenter;
            coordinateTextRectTransform.sizeDelta = textContainerSize;

            _visual.ConfigureImageFromFullToVisibleSprite(
                    full: _full,
                    visible: _visible
                );
        }

        public void SetCoordinateText(float value)
            => _coordinateText.text = value.ToString();
    }
}