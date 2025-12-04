using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(TMP_InputField))]
    public class AnswerSlotUI : MonoBehaviour
    {
        public event Action OnValueChanged;

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

            _inputField.onValueChanged.AddListener(value => OnValueChanged?.Invoke());

            ClearText();
            _placeholderFontColor = _inputField.placeholder.color;
        }

        public bool IsNotEmpty()
            => !string.IsNullOrWhiteSpace(_inputField.text);

        private void Start()
            => _visual.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );

        public void SetText(string text)
            => _inputField.text = text;

        public void ClearText()
            => SetText("");

        private bool TryGetValue(out float value)
            => float.TryParse(_inputField.text.Trim(), out value);

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

        public bool Grade(Func<float, bool> condition)
        {
            bool isCorrect = false;

            if (TryGetValue(out float answer))
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