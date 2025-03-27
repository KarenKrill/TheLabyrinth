using System;
using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Common.Logging;
    using Abstractions;
    using StateMachine.Abstractions;

    public class GameFlow : IGameFlow, IDisposable
    {
        ILogger _logger;
        IStateMachine<GameState> _stateMachine;
        public GameFlow(ILogger logger, IStateMachine<GameState> stateMachine)
        {
            _logger = logger;
            _stateMachine = stateMachine;
            _stateMachine.StateEnter += OnStateEnter;
            _stateMachine.StateExit += OnStateExit;
            _logger.Log($"GameFlow()");
        }
        public GameState State => _stateMachine.State;
        public event Action LevelFinish;
        public event Action GameStart;
        public event Action LevelPause;
        public event Action GameEnd;
        public event Action MainMenuLoad;
        public event Action LevelLoad;
        public event Action PlayerWin;
        public event Action PlayerLoose;
        public event Action LevelPlay;

        private void OnStateEnter(IStateMachine<GameState> stateMachine, GameState state)
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
        private void OnStateExit(IStateMachine<GameState> stateMachine, GameState state)
        {
            //throw new NotImplementedException();
        }
        public void FinishLevel() => _stateMachine.TransitTo(GameState.LevelFinish);
        public void LoadMainMenu() => _stateMachine.TransitTo(GameState.MainMenu);
        public void LoadLevel() => _stateMachine.TransitTo(GameState.LevelLoad);
        public void PauseLevel() => _stateMachine.TransitTo(GameState.PauseMenu);
        public void StartGame() => _stateMachine.TransitTo(GameState.GameStart);
        public void EndGame() => _stateMachine.TransitTo(GameState.GameEnd);
        public void WinGame() => _stateMachine.TransitTo(GameState.WinMenu);
        public void LooseGame() => _stateMachine.TransitTo(GameState.LooseMenu);
        public void PlayLevel() => _stateMachine.TransitTo(GameState.LevelPlay);
        public void Dispose()
        {
            _stateMachine.StateEnter -= OnStateEnter;
            _stateMachine.StateExit -= OnStateExit;
            _logger.Log($"GameFlow.Dispose()");
        }

    }
}