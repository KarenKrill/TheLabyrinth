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
        public int CurrentLevelNumber => _PassedLevels;
        public string? CurrentLevelName => null;

        public event Action? CurrentLevelChanged;
#nullable restore

        [Inject]
        public void Initialize(IGameFlow gameFlow, IManagedStateMachine<GameState> managedStateMachine)
        {
            _gameFlow = gameFlow;
            _managedStateMachine = managedStateMachine;
        }
        public void OnGameStart()
        {
            ResetToDefaults();
        }
        public void OnLevelFinish()
        {
            if (_PassedLevels < _gameLevelsCount)
            {
                _PassedLevels++;
                OnCurrentLevelChanged();
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

        private IGameFlow _gameFlow;
        private IManagedStateMachine<GameState> _managedStateMachine;

        private int _passedLevels = 0;
        private int _PassedLevels
        {
            get => _passedLevels;
            set
            {
                if (value == 0 || _passedLevels != value)
                {
                    _passedLevels = value;
                }
            }
        }

        private void Awake()
        {
            _managedStateMachine.Start();
        }
        private void Update()
        {
            if (_gameFlow.State == GameState.LevelPlay)
            {
                UpdateLeftLevelTime();
            }
        }

        private void UpdateLeftLevelTime()
        {
        }
        private void ResetToDefaults()
        {
            _PassedLevels = 1;
            OnCurrentLevelChanged();
        }
        
        private void OnCurrentLevelChanged()
        {
            CurrentLevelChanged?.Invoke();
        }
    }
}