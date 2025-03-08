using System;
using UnityEngine;
using Zenject;
using KarenKrill.Logging;

namespace KarenKrill.Core
{
    public enum GameState
    {
        Initial,
        GameStart,
        MainMenu,
        LevelLoad,
        LevelPlay,
        LevelFinish,
        PauseMenu,
        WinMenu,
        LooseMenu,
        GameEnd
    }
    public interface IGameFlow
    {
        GameState State { get; }
        event Action GameStart;
        event Action MainMenuLoad;
        event Action LevelLoad;
        event Action LevelPlay;
        event Action LevelFinish;
        event Action LevelPause;
        event Action PlayerWin;
        event Action PlayerLoose;
        event Action GameEnd;
        void LoadMainMenu();
        void LoadLevel();
        void PlayLevel();
        void PauseLevel();
        void FinishLevel();
        void StartGame();
        void EndGame();
        void WinGame();
        void LooseGame();
    }
    public class GameFlow : IGameFlow, IDisposable
    {
        [Inject]
        ILogger _logger;
        [Inject]
        IStateMachine<GameState> _stateMachine;
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