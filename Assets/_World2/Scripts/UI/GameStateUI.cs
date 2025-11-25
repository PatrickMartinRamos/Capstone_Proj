using TMPro;
using UnityEngine;

namespace Stellarfarer
{
    public class GameStateUI : MonoBehaviour
    {
        private const string WIN_MESSAGE = "YOU WIN!";
        private const string LOSE_MESSAGE = "YOU LOSE!";

        [SerializeField] private TextMeshProUGUI _stateLabel;
        [SerializeField] private NextStageButtonUI _nextStageButtonUI;
        [SerializeField] private RetryButtonUI _retryButtonUI;

        private Hydros7WorldManager _hydros7WorldManager;

        private void Start()
        {
            if (Hydros7WorldManager.Instance != null)
            {
                _hydros7WorldManager = Hydros7WorldManager.Instance;

                _hydros7WorldManager.OnGamePauseToggled
                    += Hydros7WorldManager_OnGamePauseToggled;
            }

            Hide();
        }

        private void OnDestroy()
        {
            if (_hydros7WorldManager != null)
            {
                _hydros7WorldManager.OnGamePauseToggled
                    -= Hydros7WorldManager_OnGamePauseToggled;
            }
        }

        private void Hydros7WorldManager_OnGamePauseToggled()
        {
            if (_hydros7WorldManager.IsGameOver())
            {
                Show();

                if (_hydros7WorldManager.IsGameWon())
                    Win();
                else if (_hydros7WorldManager.IsGameLost())
                    Lose();
            }
        }

        private void Win()
        {
            SetStateLabelText(WIN_MESSAGE);
            ShowNextStageButton();
        }

        private void Lose()
        {
            SetStateLabelText(LOSE_MESSAGE);
            ShowReturnButton();
        }

        private void ShowNextStageButton()
        {
            _nextStageButtonUI.Show();
            _retryButtonUI.Hide();
        }

        private void ShowReturnButton()
        {
            _retryButtonUI.Show();
            _nextStageButtonUI.Hide();
        }

        private void Show()
            => SetVisibility(true);

        private void Hide()
            => SetVisibility(false);

        private void SetVisibility(bool isVisible)
            => gameObject.SetActive(isVisible);

        private void SetStateLabelText(string state)
            => _stateLabel.text = state;
    }
}