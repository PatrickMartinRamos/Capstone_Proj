using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Image))]
    public class ChoiceSlotButtonUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private TextMeshProUGUI _text;

        private void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();

            ClearText();
        }

        private void Start()
            => _visual.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );

        public void SetText(string text)
            => _text.text = text;

        public void ClearText()
            => SetText("");
    }
}