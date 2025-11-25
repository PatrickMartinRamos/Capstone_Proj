using UnityEngine;

namespace Stellarfarer
{
    [RequireComponent(typeof(CanvasGroup))]
    public class ObjectiveUi : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private CleansingType _cleansingType;

        private void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            Hide();
        }

        public void TryDisplay(CleansingType cleansingType)
        {
            if (cleansingType.HasFlag(_cleansingType))
                Show();
            else
                Hide();
        }

        private void Show()
        {
            SetVisibility(true);
            transform.SetAsFirstSibling();
        }

        private void Hide()
        {
            SetVisibility(false);
            transform.SetAsLastSibling();
        }

        private void SetVisibility(bool isVisible)
            => _canvasGroup.alpha = isVisible ? 1 : 0;
    }
}