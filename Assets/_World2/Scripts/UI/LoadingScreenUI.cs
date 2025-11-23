using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class LoadingScreenUI : SingletonBehaviour<LoadingScreenUI>
    {
        [SerializeField] private Image _visual;

        protected override void Awake()
        {
            base.Awake();

            Hide();
        }

        public void SetColor(Color color)
        {
            color.a = 0f;
            _visual.color = color;
        }

        public Sequence LoadScreen(Action callback)
        {
            float duration = 0.5f;

            Sequence sequence = DOTween.Sequence();
            sequence.AppendCallback(Show);
            sequence.Append(_visual.DOFade(1f, duration));
            sequence.AppendCallback(() => callback?.Invoke());
            sequence.AppendInterval(1f);
            sequence.Append(_visual.DOFade(0f, duration));
            sequence.AppendCallback(Hide);
            return sequence;
        }

        public void Show()
            => SetVisibility(true);
        public void Hide()
            => SetVisibility(false);
        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}