using UnityEngine;

namespace Stellarfarer
{
    public class ProgressionManagerUI : MonoBehaviour
    {
        [SerializeField] private ProgressBarUI _progressBarUI;
        [SerializeField] private ProgressShellGroupUI _progressShellGroupUI;

        private ProgressionManager _progressionManager;

        private void Start()
        {
            if (ProgressionManager.Instance != null)
            {
                _progressionManager = ProgressionManager.Instance;

                _progressionManager.OnPointsChanged
                    += ProgressionManager_OnPointsChanged;
            }
        }

        private void OnDestroy()
        {
            if (_progressionManager != null)
            {
                _progressionManager.OnPointsChanged
                    -= ProgressionManager_OnPointsChanged;
            }
        }

        private void ProgressionManager_OnPointsChanged()
        {
            float progression = _progressionManager.GetProgression();
            _progressBarUI.Progress(progression);
            _progressShellGroupUI.Progress(progression);
        }
    }
}