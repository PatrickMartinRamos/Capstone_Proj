using DG.Tweening;
using UnityEngine;

namespace Stellarfarer
{
    public class HydriousSpawnTimerBarUI : MonoBehaviour
    {
        [SerializeField] private UnityEngine.UI.Image _fill;

        private Hydros7GameManager _gameManager;
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
            if (Hydros7GameManager.Instance != null)
            {
                _gameManager = Hydros7GameManager.Instance;

                _gameManager.OnStateChanged
                    += Hydros7GameManager_OnStateChanged;
            }
        }

        private void OnDestroy()
        {
            if (_gameManager != null)
            {
                _gameManager.OnStateChanged
                    -= Hydros7GameManager_OnStateChanged;
            }
        }

        private void Hydros7GameManager_OnStateChanged()
        {
            if (_gameManager.IsGamePlaying() && !_isRunning)
            {
                _isRunning = true;

                ResetSpawnTimerBar();

                float full = 1f;
                _fill.DOFillAmount(full, _gameManager.GetSpawnTime())
                    .SetLoops(-1, LoopType.Restart)
                    .OnStepComplete(() => HydriousSpawnManager.Instance.SpawnNextSpawn());
            }
            else if (_gameManager.IsGameLost())
                _fill.DOKill();
        }
    }
}