using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private Image _border;
        [SerializeField] private Sprite _borderFull;
        [SerializeField] private Sprite _borderVisible;
        [SerializeField] private Image _fill;

        private ProgressionManager _progressionManager;

        private void Awake()
            => _fill.fillAmount = 0f;

        private void Start()
        {
            if (ProgressionManager.Instance != null)
            {
                _progressionManager = ProgressionManager.Instance;

                _progressionManager.OnProgressBarRectGot
                    += ProgressionManager_OnProgressBarRectGot;
            }
        }

        private void OnDestroy()
        {
            if (_progressionManager != null)
            {
                _progressionManager.OnProgressBarRectGot
                    -= ProgressionManager_OnProgressBarRectGot;
            }
        }

        private Rect ProgressionManager_OnProgressBarRectGot()
        {
            _border.ConfigureImageFromFullToVisibleSprite(_borderFull, _borderVisible);

            RectTransform borderRectTransform = _border.rectTransform;
            Rect rect = new Rect(borderRectTransform.position, borderRectTransform.sizeDelta);

            _border.ConfigureImageAsFullSprite(_borderFull);

            return rect;
        }

        public void Progress(float progression)
            => _fill.DOFillAmount(progression, 1f);
    }
}