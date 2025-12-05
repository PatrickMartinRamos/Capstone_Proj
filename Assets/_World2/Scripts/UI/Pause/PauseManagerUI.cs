using UnityEngine;

namespace Stellarfarer
{
    public class PauseManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private BackgroundGroupResponsiveUI _backgroundGroupResponsiveUI;

        private Hydros7WorldManager _hydros7WorldManager;

        private void Start()
        {
            if (Hydros7WorldManager.Instance != null)
            {
                _hydros7WorldManager = Hydros7WorldManager.Instance;

                _hydros7WorldManager.OnGamePauseToggled
                    += Hydros7GameManager_OnGamePauseToggled;
            }

            HidePauseMenu();
        }

        private void OnDestroy()
        {
            if (_hydros7WorldManager != null)
            {
                _hydros7WorldManager.OnGamePauseToggled
                    -= Hydros7GameManager_OnGamePauseToggled;
            }
        }

        private void Hydros7GameManager_OnGamePauseToggled()
        {
            if (_hydros7WorldManager.IsGamePaused())
            {
                ShowPauseMenu();
                _backgroundGroupResponsiveUI.RefreshLayout();
            }
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