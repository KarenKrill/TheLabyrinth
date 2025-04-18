using System;
using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;
    using Common.StateSystem.Abstractions;

    public class GameplayController : MonoBehaviour, IGameController, IGameInfoProvider
    {
#nullable enable
        private LevelInfo _currentLevelInfo;
        public LevelInfo CurrentLevel
        {
            get => _currentLevelInfo;
            set
            {
                _currentLevelInfo = value;
                CurrentLevelChanged?.Invoke(_currentLevelInfo);
            }
        }
        public event Action<LevelInfo>? CurrentLevelChanged;
#nullable restore

        [Inject]
        public void Initialize(IGameFlow gameFlow, IManagedStateMachine<GameState> managedStateMachine)
        {
            _gameFlow = gameFlow;
            _managedStateMachine = managedStateMachine;
        }
        public void StartGame() => ResetToDefaults();
        public void FinishLevel()
        {
            if (CurrentLevel.Index < _gameLevelsCount)
            {
                CurrentLevel = GetNextLevelInfo();
                _gameFlow.LoadLevel();
            }
            else
            {
                _gameFlow.WinGame();
            }
        }

        [SerializeField]
        private LoadLevelManager _loadLevelManager;
        [SerializeField]
        private int _gameLevelsCount = 13;
        [SerializeField]
        private int _mazeMinLevelsCount = 4;
        [SerializeField]
        private int _mazeMaxLevelsCount = 13;

        private IGameFlow _gameFlow;
        private IManagedStateMachine<GameState> _managedStateMachine;

        private void Awake()
        {
            _managedStateMachine.Start();
        }
        private void ResetToDefaults()
        {
            CurrentLevel = new(1, string.Empty, MazeShape.Circle, _mazeMinLevelsCount);
        }
        private LevelInfo GetNextLevelInfo()
        {
            var levelsCount = CurrentLevel.MazeLevelsCount < _mazeMaxLevelsCount ? CurrentLevel.MazeLevelsCount + 1 : _mazeMaxLevelsCount;
            return new(CurrentLevel.Index + 1, string.Empty, MazeShape.Circle, levelsCount);
        }
    }
}