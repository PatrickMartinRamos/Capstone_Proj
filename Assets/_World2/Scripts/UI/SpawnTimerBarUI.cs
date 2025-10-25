using DG.Tweening;

namespace Stellarfarer
{
    public class SpawnTimerBarUI : UnityEngine.MonoBehaviour
    {
        [UnityEngine.SerializeField] private UnityEngine.UI.Image _fill;

        private World2_GameManager _gameManager;
        private bool _isRunning = false;

        private void Awake()
        {
            if (_fill == null)
            {
                UnityEngine.Debug.LogError("Fill is not assigned");
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
            if (World2_GameManager.Instance != null)
            {
                _gameManager = World2_GameManager.Instance;

                _gameManager.OnStateChanged
                    += GameManager_OnStateChanged;
            }
        }

        private void OnDestroy()
        {
            if (_gameManager != null)
            {
                _gameManager.OnStateChanged
                    -= GameManager_OnStateChanged;
            }
        }

        private void GameManager_OnStateChanged()
        {
            if (_gameManager.IsGamePlaying() && !_isRunning)
            {
                _isRunning = true;

                ResetSpawnTimerBar();

                float full = 1f;
                _fill.DOFillAmount(full, _gameManager.GetSpawnTime())
                    .SetLoops(-1, LoopType.Restart)
                    .OnStepComplete(() => World2_SpawnManager.Instance.SpawnNextSpawn());
            }
            else if (_gameManager.IsGameLost())
                _fill.DOKill();
        }
    }
}