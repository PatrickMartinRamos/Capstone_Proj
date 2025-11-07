using UnityEngine;

namespace Stellarfarer
{
    public class World2_GameManager : SingletonBehaviour<World2_GameManager>
    {
        private enum State
        {
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameWin,
            GameLose
        }

        public event System.Action OnStateChanged;

        [SerializeField] private World2StageDictSO _stageDataDictSO;
        [SerializeField] private World2StageSO _stageSO;

        private State _state;
        private float _countdownToStartTimer = 3f;

        protected override void Awake()
        {
            base.Awake();

            InitializeRandomness();

            Wait();
        }

        private void InitializeRandomness()
        {
            int seed = (int)System.DateTime.Now.Ticks;

            Random.InitState(seed);
        }

        private void UpdateStageData()
        {
            int baseStageID = 1; // TODO: Change to saved stage
            string stageIDName = "StageID";
            int stageID = PlayerPrefs.HasKey(stageIDName) ? PlayerPrefs.GetInt(stageIDName) : baseStageID;
            _stageSO = _stageDataDictSO[stageID];
        }

        private void Update()
        {
            switch (_state)
            {
                case State.WaitingToStart:
                    UpdateStageData();
                    World2_SpawnManager.Instance.InitializeSpawnManager(_stageSO.Species);
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

                    break;
                case State.GameLose:
                    Debug.Log("Owo");
                    break;
            }
        }

        private void Wait()
            => UpdateState(State.WaitingToStart);

        private void Countdown()
            => UpdateState(State.CountdownToStart);
        public bool IsCountingDown()
            => CompareState(State.CountdownToStart);
        public float CountdownTime()
            => _countdownToStartTimer;

        private void PlayGame()
            => UpdateState(State.GamePlaying);
        public bool IsGamePlaying()
            => CompareState(State.GamePlaying);

        private void WinGame()
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
    }
}