using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class GameStateUI : MonoBehaviour
    {
        private const string WIN_MESSAGE = "YOU WIN!";
        private const string LOSE_MESSAGE = "YOU LOSE!";
        private const string STAGE_ID_NAME = "StageID";
        private const string WORLD2SCENE_FILEPATH = "Scenes/Game Scene/World2_GameScene";
        private const string WORLDSELECTION_FILEPATH = "Scenes/WorldSelection";

        [SerializeField] private TextMeshProUGUI _stateLabel;
        [SerializeField] private Button _nextStageButton;
        [SerializeField] private Button _retryButton;
        [SerializeField] private Button _returnButton;

        private Hydros7GameManager _hydros7GameManager;

        private void Awake()
        {
            _nextStageButton.onClick.AddListener(() =>
            {
                int stageID = Hydros7GameManager.Instance.GetStageID();
                stageID = PlayerPrefs.HasKey(STAGE_ID_NAME) ? stageID + 1 : 1;
                PlayerPrefs.SetInt(STAGE_ID_NAME, stageID);
                LoadScene(WORLD2SCENE_FILEPATH);
            });
            _retryButton.onClick.AddListener(() => LoadScene(WORLD2SCENE_FILEPATH));
            _returnButton.onClick.AddListener(() => LoadScene(WORLDSELECTION_FILEPATH));
        }

        private void LoadScene(string sceneName){
            SceneManager.LoadScene(sceneName);
            Time.timeScale = 1f;
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
                _hydros7GameManager = Hydros7GameManager.Instance;

                _hydros7GameManager.OnStateChanged
                    -= Hydros7GameManager_OnStateChanged;
            }
        }

        private void Hydros7GameManager_OnStateChanged()
        {
            if (_hydros7GameManager.IsGameOver())
            {
                Show();

                if (_hydros7GameManager.IsGameWon())
                    Win();
                else if (_hydros7GameManager.IsGameLost())
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
            => SetNextStageButtonAndRetryButtonVisibility(
                isNextStageButtonVisible: true,
                isReturnButtonVisible: false
            );

        private void ShowReturnButton()
            => SetNextStageButtonAndRetryButtonVisibility(
                isNextStageButtonVisible: false,
                isReturnButtonVisible: true
            );

        private void SetNextStageButtonAndRetryButtonVisibility(bool isNextStageButtonVisible, bool isReturnButtonVisible)
        {
            SetButtonVisibility(_nextStageButton, isNextStageButtonVisible);
            SetButtonVisibility(_retryButton, isReturnButtonVisible);
        }

        private void SetButtonVisibility(Button button, bool isVisible)
            => button.gameObject.SetActive(isVisible);

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