using System;
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

        private Color _placeholderFontColor;

        private void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();

            if (_inputField == null)
                _inputField = GetComponent<TMP_InputField>();

            ClearText();
            _placeholderFontColor = _inputField.placeholder.color;
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

        private bool TryGetValue(out int value)
            => int.TryParse(_inputField.text.Trim(), out value);

        private void SetColor(Color visualColor, Color textFontColor, Color placeholderFontColor)
        {
            _visual.color = visualColor;
            _inputField.textComponent.color = textFontColor;
            _inputField.placeholder.color = placeholderFontColor;
        }

        private void Correct()
            => SetColor(Color.white, Color.black, _placeholderFontColor);

        private void Wrong()
            => SetColor(Color.red, Color.white, new Color(1f, 1f, 1f, _placeholderFontColor.a));

        public bool Grade(Func<int, bool> condition)
        {
            bool isCorrect = false;

            if (TryGetValue(out int answer))
            {
                isCorrect = condition.Invoke(answer);

                if (isCorrect)
                    Correct();
                else
                    Wrong();
            }
            else
                Wrong();

            return isCorrect;
        }
    }
}