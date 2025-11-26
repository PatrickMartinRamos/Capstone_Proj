using DG.Tweening;
using UnityEngine;

namespace Stellarfarer
{
    public class HydriousSpawnTimerBarUI : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Image _fill;

        private Hydros7WorldManager _hydros7WorldManager;
        private bool _isRunning = false;

        private void Awake()
        {
            if (_fill == null)
            {
                Debug.LogError("Fill is not assigned");
                return;
            }

            ResetSpawnTimerBar();
        }

        private void ResetSpawnTimerBar()
        {
            _fill.DOKill();
            _fill.fillAmount = 0f;
        }

        private void Start()
        {
            if (Hydros7WorldManager.Instance != null)
            {
                _hydros7WorldManager = Hydros7WorldManager.Instance;

                _hydros7WorldManager.OnGameStateChanged
                    += Hydros7WorldManager_OnGameStateChanged;
            }
        }

        private void OnDestroy()
        {
            if (_hydros7WorldManager != null)
            {
                _hydros7WorldManager.OnGameStateChanged
                    -= Hydros7WorldManager_OnGameStateChanged;
            }
        }

        private void Hydros7WorldManager_OnGameStateChanged()
        {
            if (_hydros7WorldManager.IsGamePlaying() && !_isRunning)
            {
                _isRunning = true;

                ResetSpawnTimerBar();

                float full = 1f;
                _fill.DOFillAmount(full, _hydros7WorldManager.GetSpawnTime())
                    .SetLoops(-1, LoopType.Restart)
                    .OnStepComplete(() => HydriousSpawnManager.Instance.SpawnNextSpawn());
            }
            else if (_hydros7WorldManager.IsGameLost())
                _fill.DOKill();
        }
    }
}