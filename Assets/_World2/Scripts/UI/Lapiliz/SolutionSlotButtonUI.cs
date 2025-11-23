using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(TMP_InputField))]
    public class SolutionSlotButtonUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private TMP_InputField _inputField;

        private void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();

            if (_inputField == null)
                _inputField = GetComponent<TMP_InputField>();

            ClearText();
        }

        private void Start()
            => _visual.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );

        public void SetText(string text)
            => _inputField.text = text;

        public void ClearText()
            => SetText("");

        public bool TryGetValue(out int value)
            => int.TryParse(_inputField.text.Trim(), out value);
    }
}