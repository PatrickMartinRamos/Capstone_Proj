using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class ProgressShellUI : MonoBehaviour
    {
        [SerializeField] private Image _visual;

        private void Awake()
            => ResetShell();

        private void ResetShell()
        {
            Color color = _visual.color;
            color.a = 0;
            _visual.color = color;
            Hide();
        }

        public void FadeIn()
        {
            Show();
            _visual.DOFade(1f, 1f);
        }

        private void Show()
            => SetVisiblity(true);

        private void Hide()
            => SetVisiblity(false);

        private void SetVisiblity(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}