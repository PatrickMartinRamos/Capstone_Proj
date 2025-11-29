using System;
using UnityEngine;

namespace Stellarfarer
{
    public class Hydros7WorldManager : SingletonBehaviour<Hydros7WorldManager>
    {
        private enum GameState
        {
            InitializeGame,
            StageDescriptionDisplay,
            TutorialDisplay,
            WaitingToStart,
            CountdownToStart,
            GamePlaying,
            GameWin,
            GameLose
        }

        private const string STAGE_ID_NAME = "StageID";

        public event Action OnGameStateChanged;
        public event Action OnGamePauseToggled;

        [SerializeField] private World2StageDictSO _stageDataDictSO;
#if UNITY_EDITOR
        [SerializeField, Range(1, 10)] private int _testStageID = 1;
        [SerializeField] private bool _deleteKey;
        [SerializeField] private bool _winGame;
        [SerializeField] private bool _loseGame;
#endif
        private int _stageID;
        private World2StageSO _stageSO;
        private GameState _state;
        private float _countdownToStartTimer = 3f;
        private bool _isGamePaused;

        protected override void Awake()
        {
            base.Awake();

            _isGamePaused = false;
        }

        private void Start()
            => InitializeGame();

        private void UpdateStageData()
        {
#if UNITY_EDITOR
            if (_deleteKey)
                PlayerPrefs.DeleteKey(STAGE_ID_NAME);
            int baseStageID = _testStageID; // TODO: Change to saved stage
#else
            int baseStageID = 1; // TODO: Change to saved stage
#endif
            _stageID = PlayerPrefs.GetInt(STAGE_ID_NAME, baseStageID);
#if UNITY_EDITOR
            _testStageID = _stageID; // TODO: Change to saved stage
#endif
            _stageSO = _stageDataDictSO[_stageID];
        }

        private void OnDestroy()
            => PlayerPrefs.DeleteKey("STAGE_ID_NAME");

        private void Update()
        {
            switch (_state)
            {
                case GameState.InitializeGame:
                    UpdateStageData();
                    HydriousSpawnManager.Instance.InitializeSpawnManager(GetCleansingType());
                    GridManager.Instance.SetGrid();
                    DisplayStageDescription();
                    break;
                case GameState.StageDescriptionDisplay:
                    break;
                case GameState.WaitingToStart:
#if UNITY_EDITOR
                    if (_deleteKey)
                        PlayerPrefs.DeleteKey($"Stage {_stageID}");
#endif
                    if (_stageID == 1 ||
                    _stageID == 3 ||
                    _stageID == 5)
                    {
                        if (PlayerPrefs.GetInt($"Stage {_stageID}", 0) == 0)
                            TryDisplayTutorial(TutorialManager.Instance.TryDisplayChooseHydriousTutorial);
                        else
                            Countdown();
                    }
                    else
                        Countdown();

                    break;
                case GameState.TutorialDisplay:
                    break;
                case GameState.CountdownToStart:
                    _countdownToStartTimer -= Time.deltaTime;

                    if (_countdownToStartTimer <= 0f)
                        PlayGame();

                    break;
                case GameState.GamePlaying:
#if UNITY_EDITOR
                    if (_winGame)
                        WinGame();
                    else if (_loseGame)
                        LoseGame();
#endif
                    break;
                case GameState.GameWin:
#if UNITY_EDITOR
                    Debug.Log("Game Won");
#endif
                    break;
                case GameState.GameLose:
#if UNITY_EDITOR
                    Debug.Log("Game Lost");
#endif
                    break;
            }
        }

        private void InitializeGame()
            => SetGameState(GameState.InitializeGame);
        public bool IsGameInitializing()
            => IsGameState(GameState.InitializeGame);

        private void DisplayStageDescription()
            => SetGameState(GameState.StageDescriptionDisplay);
        public bool IsDisplayingStageDescription()
            => IsGameState(GameState.StageDescriptionDisplay);

        public void TryDisplayTutorial(Action tutorial)
        {
            if (IsWaiting() || IsDisplayingTutorial())
            {
                DisplayTutorial();
                tutorial.Invoke();
            }
        }
        private void DisplayTutorial()
            => SetGameState(GameState.TutorialDisplay);
        public bool IsDisplayingTutorial()
            => IsGameState(GameState.TutorialDisplay);

        public void Wait()
            => SetGameState(GameState.WaitingToStart);
        public bool IsWaiting()
            => IsGameState(GameState.WaitingToStart);

        public void Countdown()
            => SetGameState(GameState.CountdownToStart);
        public bool IsCountingDown()
            => IsGameState(GameState.CountdownToStart);
        public float GetCountdownTime()
            => _countdownToStartTimer;

        private void PlayGame()
            => SetGameState(GameState.GamePlaying);
        public bool IsGamePlaying()
            => IsGameState(GameState.GamePlaying);

        public void WinGame()
            => SetGameState(GameState.GameWin);
        public bool IsGameWon()
            => IsGameState(GameState.GameWin);

        public void LoseGame()
            => SetGameState(GameState.GameLose);
        public bool IsGameLost()
            => IsGameState(GameState.GameLose);

        public bool IsGameOver()
            => IsGameWon() || IsGameLost();

        private void SetGameState(GameState state)
        {
            _state = state;
            OnGameStateChanged?.Invoke();
        }
        private bool IsGameState(GameState state)
            => _state == state;

        public CleansingType GetCleansingType()
            => _stageSO.CleansingType;

        public float GetSpawnTime()
            => _stageSO.SpawnTime;

        public int GetRequiredPoints()
            => _stageSO.HydrionEnergyAmountRequired;

        public int GetStageID()
            => _stageID;

        public int GetStageCount()
            => _stageDataDictSO.Count();

        public void ToggleGamePause()
        {
            _isGamePaused = !_isGamePaused;

            Time.timeScale = _isGamePaused ? 0f : 1f;

            OnGamePauseToggled?.Invoke();
        }

        public bool IsGamePaused()
            => _isGamePaused;
    }
}