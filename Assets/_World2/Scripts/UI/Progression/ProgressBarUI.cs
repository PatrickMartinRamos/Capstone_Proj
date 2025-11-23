using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class ProgressBarUI : MonoBehaviour
    {
        [SerializeField] private Image _fill;

        private void Awake()
            => _fill.fillAmount = 0f;

        public void Progress(float progression)
            => _fill.DOFillAmount(progression, 1f);
    }
}