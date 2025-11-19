using UnityEngine;
using UnityEngine.SceneManagement;

namespace Stellarfarer
{
    public class Hydros7GameManager : SingletonBehaviour<Hydros7GameManager>
    {
        private enum State
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameWin,
            GameLose
        }

        private const string STAGE_ID_NAME = "StageID";

        public event System.Action OnStateChanged;

        [SerializeField] private World2StageDictSO _stageDataDictSO;
#if UNITY_EDITOR
        [SerializeField, Range(1, 10)] private int _testStageID = 1;
#endif
        private World2StageSO _stageSO;
        private State _state;
        private float _countdownToStartTimer = 3f;

        private void Start()
            => Wait();

        private void UpdateStageData()
        {
#if UNITY_EDITOR
            int baseStageID = _testStageID; // TODO: Change to saved stage
#else
            int baseStageID = 1; // TODO: Change to saved stage
#endif
            int stageID = PlayerPrefs.HasKey(STAGE_ID_NAME) ? PlayerPrefs.GetInt(STAGE_ID_NAME) : baseStageID;
            _stageSO = _stageDataDictSO[stageID];
        }

        private void OnDestroy()
            => PlayerPrefs.DeleteKey("STAGE_ID_NAME");

        private void Update()
        {
            switch (_state)
            {
                case State.WaitingToStart:
                    UpdateStageData();
                    HydriousSpawnManager.Instance.InitializeSpawnManager(_stageSO.CleansingType);
                    GridManager.Instance.SetGrid();
                    Countdown();
                    break;
                case State.CountdownToStart:
                    _countdownToStartTimer -= Time.deltaTime;

                    if (_countdownToStartTimer <= 0f)
                        PlayGame();

                    break;
                case State.GamePlaying:

                    break;
                case State.GameWin:
                    SceneManager.LoadScene("Scenes/WorldSelection");
                    Debug.Log("Game Won");
                    break;
                case State.GameLose:
                    SceneManager.LoadScene("Scenes/WorldSelection");
                    Debug.Log("Game Lost");
                    break;
            }
        }

        private void Wait()
            => UpdateState(State.WaitingToStart);

        private void Countdown()
            => UpdateState(State.CountdownToStart);
        public bool IsCountingDown()
            => CompareState(State.CountdownToStart);
        public float GetCountdownTime()
            => _countdownToStartTimer;

        private void PlayGame()
            => UpdateState(State.GamePlaying);
        public bool IsGamePlaying()
            => CompareState(State.GamePlaying);

        public void WinGame()
            => UpdateState(State.GameWin);
        public bool IsGameWon()
            => CompareState(State.GameWin);

        public void LoseGame()
            => UpdateState(State.GameLose);
        public bool IsGameLost()
            => CompareState(State.GameLose);

        public bool IsGameOver()
            => IsGameWon() || IsGameLost();

        private void UpdateState(State state)
        {
            _state = state;
            OnStateChanged?.Invoke();
        }
        private bool CompareState(State state)
            => _state == state;

        public float GetSpawnTime()
            => _stageSO.SpawnTime;

        public int GetRequiredPoints()
            => _stageSO.HydrionEnergyAmountRequired;
    }
}