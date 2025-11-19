using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class GridNavigatorSingleUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public event Func<bool> OnInteracted;

        [SerializeField] private Image _visual;
        [SerializeField] private Sprite _full;
        [SerializeField] private Sprite _visible;
        [SerializeField] private Image _held;
        [SerializeField] private float _delayDuration;

        private bool _isHeld = false;
        private float _delayTimer;

        private void Awake()
            => _held.fillAmount = 0f;

        private void Start()
        {
            _visual.ConfigureImageFromFullToVisibleSprite(
                    full: _full,
                    visible: _visible
                );
            _visual.alphaHitTestMinimumThreshold = 0.1f;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _held.DOKill();
            _held.DOFillAmount(0f, 0.5f);
            _isHeld = false;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!(bool)OnInteracted?.Invoke())
                return;

            _held.DOFillAmount(1f, 1.5f)
                .OnComplete(() =>
                {
                    _delayTimer = _delayDuration;
                    _isHeld = true;
                });
        }

        private void Update()
        {
            if (_isHeld)
            {
                _delayTimer -= Time.deltaTime;

                if (_delayTimer <= 0)
                {
                    _delayTimer = _delayDuration;

                    if (!(bool)OnInteracted?.Invoke())
                        return;
                }
            }
        }
    }
}