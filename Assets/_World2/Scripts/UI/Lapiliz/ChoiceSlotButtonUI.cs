using System.Collections;
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

        private string _textMessage;

        private void Awake()
        {
            if (_visual == null)
                _visual = GetComponent<Image>();
        }

        private void Start()
            => _visual.ConfigureImageFromFullToVisibleSprite(
                full: _full,
                visible: _visible
            );

        public void OnEnable()
            => StartCoroutine(SetText());

        private IEnumerator SetText()
        {
            yield return new WaitForEndOfFrame();
            _text.text = _textMessage;

            RectTransform rectTransform = transform as RectTransform;
            Vector2 size = rectTransform.rect.size;
            float paddingFactor = 0.1f;
            float paddingX = size.x * paddingFactor;
            float paddingY = size.y * paddingFactor;

            _text.margin = new Vector4(
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
                StartCoroutine(SetText());
        }

        public void ClearText()
            => SetTextMesssage("");
    }
}