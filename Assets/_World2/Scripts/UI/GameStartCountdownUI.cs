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

        private World2_GameManager _gameManager;
        private int _previousCountdownTime;
        private RectTransform _countdownTextRectTransform;

        private void Awake()
        {
            _countdownTextRectTransform = _countdownText.rectTransform;

            ResetCountdownRectTransformAnimation();
        }

        private void Start()
        {
            if (World2_GameManager.Instance != null)
            {
                _gameManager = World2_GameManager.Instance;

                _gameManager.OnStateChanged
                    += GameManager_OnStateChanged;
            }

            Hide();
        }

        private void OnDestroy()
        {
            if (_gameManager != null)
            {
                _gameManager.OnStateChanged
                    -= GameManager_OnStateChanged;
            }
        }

        private void Update()
        {
            if (!_gameManager.IsCountingDown())
                return;

            int countdownTime = Mathf.CeilToInt(_gameManager.CountdownTime());
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

        private void GameManager_OnStateChanged()
        {
            if (_gameManager.IsCountingDown())
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