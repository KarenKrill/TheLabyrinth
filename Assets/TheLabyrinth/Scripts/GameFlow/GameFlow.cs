using System;
using UnityEngine;
using Zenject;
using KarenKrill.Common.Logging;
using KarenKrill.TheLabyrinth.StateMachine.Abstractions;
using KarenKrill.TheLabyrinth.GameFlow.Abstractions;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    public class GameFlow : IGameFlow, IDisposable
    {
        [Inject]
        ILogger _logger;
        [Inject]
        IStateMachine<GameState> _stateMachine;
        public GameState State => _stateMachine.State;
        IStateSwitcher<GameState> _stateSwitcher => _stateMachine.StateSwitcher;
        public event Action LevelFinish;
        public event Action GameStart;
        public event Action LevelPause;
        public event Action GameEnd;
        public event Action MainMenuLoad;
        public event Action LevelLoad;
        public event Action PlayerWin;
        public event Action PlayerLoose;
        public event Action LevelPlay;

        private void OnStateEnter(GameState state)
        {
            _logger.Log($"{nameof(GameFlow)} Entered to {state} state");
            switch (state)
            {
                case GameState.GameStart:
                    GameStart?.Invoke();
                    break;
                case GameState.MainMenu:
                    MainMenuLoad?.Invoke();
                    break;
                case GameState.LevelLoad:
                    LevelLoad?.Invoke();
                    break;
                case GameState.LevelFinish:
                    LevelFinish?.Invoke();
                    break;
                case GameState.PauseMenu:
                    LevelPause?.Invoke();
                    break;
                case GameState.WinMenu:
                    PlayerWin?.Invoke();
                    break;
                case GameState.LooseMenu:
                    PlayerLoose?.Invoke();
                    break;
                case GameState.GameEnd:
                    GameEnd?.Invoke();
                    break;
                case GameState.LevelPlay:
                    LevelPlay?.Invoke();
                    break;
                default:
                    _logger.LogWarning($"{state} state not implemented!");
                    break;
            }
        }
        private void OnStateExit(GameState state)
        {
            //throw new NotImplementedException();
        }
        public void FinishLevel() => _stateSwitcher.TransitTo(GameState.LevelFinish);
        public void LoadMainMenu() => _stateSwitcher.TransitTo(GameState.MainMenu);
        public void LoadLevel() => _stateSwitcher.TransitTo(GameState.LevelLoad);
        public void PauseLevel() => _stateSwitcher.TransitTo(GameState.PauseMenu);
        public void StartGame() => _stateSwitcher.TransitTo(GameState.GameStart);
        public void EndGame() => _stateSwitcher.TransitTo(GameState.GameEnd);
        public void WinGame() => _stateSwitcher.TransitTo(GameState.WinMenu);
        public void LooseGame() => _stateSwitcher.TransitTo(GameState.LooseMenu);
        public void PlayLevel() => _stateSwitcher.TransitTo(GameState.LevelPlay);
        [Inject]
        private void Initialize()
        {
            _stateMachine.StateEnter += OnStateEnter;
            _stateMachine.StateExit += OnStateExit;
            _logger.Log($"GameFlow()");
        }
        public void Dispose()
        {
            _stateMachine.StateEnter -= OnStateEnter;
            _stateMachine.StateExit -= OnStateExit;
            _logger.Log($"GameFlow.Dispose()");
        }
    }
}