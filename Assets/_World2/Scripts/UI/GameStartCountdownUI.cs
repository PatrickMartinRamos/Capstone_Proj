using UnityEngine;
using DG.Tweening;
using TMPro;

namespace Stellarfarer
{
    public class GameStartCountdownUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private float _growDuration = 0.5f;
        [SerializeField] private float _punchZ = 25f;
        [SerializeField] private float _punchZDuration = 1.5f;
        [SerializeField] private int _punchZVibrato = 10;

        private Hydros7GameManager _hydros7GameManager;
        private int _previousCountdownTime;
        private RectTransform _countdownTextRectTransform;

        private void Awake()
        {
            _countdownTextRectTransform = _countdownText.rectTransform;

            ResetCountdownRectTransformAnimation();
        }

        private void Start()
        {
            if (Hydros7GameManager.Instance != null)
            {
                _hydros7GameManager = Hydros7GameManager.Instance;

                _hydros7GameManager.OnStateChanged
                    += Hydros7GameManager_OnStateChanged;
            }

            Hide();
        }

        private void OnDestroy()
        {
            if (_hydros7GameManager != null)
            {
                _hydros7GameManager.OnStateChanged
                    -= Hydros7GameManager_OnStateChanged;
            }
        }

        private void Update()
        {
            if (!_hydros7GameManager.IsCountingDown())
                return;

            int countdownTime = Mathf.CeilToInt(_hydros7GameManager.GetCountdownTime());
            _countdownText.text = countdownTime.ToString();

            if (_previousCountdownTime != countdownTime)
            {
                _previousCountdownTime = countdownTime;

                ResetCountdownRectTransformAnimation();

                Sequence sequence = DOTween.Sequence();

                sequence.Append(_countdownTextRectTransform.DOScale(Vector3.one, _growDuration));
                sequence.Append(_countdownTextRectTransform.DOPunchRotation(Vector3.forward * _punchZ, _punchZDuration, _punchZVibrato));

                // TODO: Add SFX
            }
        }

        private void ResetCountdownRectTransformAnimation()
        {
            _countdownTextRectTransform.DOKill();
            _countdownTextRectTransform.localScale = Vector3.zero;
            _countdownTextRectTransform.localRotation = Quaternion.identity;
        }

        private void Hydros7GameManager_OnStateChanged()
        {
            if (_hydros7GameManager.IsCountingDown())
                Show();
            else
                Hide();
        }

        private void Show()
            => SetVisibility(true);

        private void Hide()
            => SetVisibility(false);

        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);
    }
}