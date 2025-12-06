using System;
using System.Collections;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(TMP_InputField))]
    public class SolutionSlotButtonUI : MonoBehaviour
    {
        public event Action OnValueChanged;

        [SerializeField] private Image _visual;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private TMP_InputField _inputField;

        private string _textMessage;
        private Color _placeholderFontColor;
        private static readonly Regex ValidPattern = new Regex(
            @"^-?$|^-?\d{1,2}$");

        private void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();

            if (_inputField == null)
                _inputField = GetComponent<TMP_InputField>();

            _inputField.onValueChanged.AddListener(Validate);
            _placeholderFontColor = _inputField.placeholder.color;
        }

        private void Validate(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            if (!ValidPattern.IsMatch(text))
            {
                // Remove last typed character
                _inputField.text = text[..^1];
            }

            OnValueChanged?.Invoke();
        }

        public bool IsNotEmpty()
            => !string.IsNullOrWhiteSpace(_inputField.text);

        private void Start()
            => _visual.ConfigureImageFromFullToVisibleSprite(
                    full: _full,
                    visible: _visible
                );

        public void OnEnable()
        {
            StartCoroutine(SetText(_inputField.textComponent));
            StartCoroutine(SetText(_inputField.placeholder as TMP_Text));
        }

        private IEnumerator SetText(TMP_Text text)
        {
            yield return new WaitForEndOfFrame();
            _inputField.text = _textMessage;

            RectTransform rectTransform = transform as RectTransform;
            Vector2 size = rectTransform.rect.size;
            float paddingFactor = 0.1f;
            float paddingX = size.x * paddingFactor;
            float paddingY = size.y * paddingFactor;

            text.margin = new Vector4(
                paddingX,
                paddingY,
                paddingX,
                paddingY
            );
        }

        public void SetTextMesssage(string text)
        {
            _textMessage = text;
            if (isActiveAndEnabled)
                StartCoroutine(SetText(_inputField.textComponent));
        }

        public void ClearText()
            => SetTextMesssage("");

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