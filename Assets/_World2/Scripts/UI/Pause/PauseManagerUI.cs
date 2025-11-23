using UnityEngine;
using UnityEngine.UI;

namespace Stellarfarer
{
    public class PauseManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private Button _retrybtn;
        [SerializeField] private Button _returnToWorlSelectionBtn;

        private Hydros7GameManager _hydros7GameManager;

        private void Start()
        {
            if (Hydros7GameManager.Instance != null)
            {
                _hydros7GameManager = Hydros7GameManager.Instance;

                _hydros7GameManager.OnGamePauseTToggled
                    += Hydros7GameManager_OnGamePauseTToggled;
            }

            HidePauseMenu();
        }

        private void OnDestroy()
        {
            if (_hydros7GameManager != null)
            {
                _hydros7GameManager.OnGamePauseTToggled
                    -= Hydros7GameManager_OnGamePauseTToggled;
            }
        }

        private void Hydros7GameManager_OnGamePauseTToggled()
        {
            if (_hydros7GameManager.IsGamePaused())
                ShowPauseMenu();
            else
                HidePauseMenu();
        }

        private void ShowPauseMenu()
            => SetPauseMenuVisibility(true);

        private void HidePauseMenu()
            => SetPauseMenuVisibility(false);

        private void SetPauseMenuVisibility(bool isVisible)
            => _pauseMenu.SetActive(isVisible);
    }
}