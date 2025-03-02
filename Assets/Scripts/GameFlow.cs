using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KarenKrill
{
    public enum GameState
    {
        StartMenu,
        PauseMenu,
        WinMenu,
        LooseMenu,
        GameStart,
        LevelLoading,
        LevelStarted,
        LevelFinished,
        GameEnd
    }
    public class GameFlow : MonoBehaviour
    {
        private GameState _validState;
        [SerializeField]
        private GameState _state;
        public GameState State => _validState;
        private static IEnumerable<GameState> ValidStateTransitions(GameState state) => state switch
        {
            GameState.StartMenu => new GameState[] { GameState.GameStart },
            GameState.GameStart => new GameState[] { GameState.LevelLoading },
            GameState.LevelLoading => new GameState[] { GameState.LevelStarted },
            GameState.LevelStarted => new GameState[] { GameState.LevelFinished, GameState.PauseMenu },
            GameState.PauseMenu => new GameState[] { GameState.GameEnd, GameState.GameStart, GameState.LevelStarted },
            GameState.LevelFinished => new GameState[] { GameState.WinMenu, GameState.LooseMenu },
            GameState.GameEnd => new GameState[] { GameState.GameStart },
            GameState.WinMenu => new GameState[] { GameState.GameEnd, GameState.GameStart },
            GameState.LooseMenu => new GameState[] { GameState.GameEnd, GameState.GameStart },
            _ => null,
        };
        private static bool IsValidStateTransition(GameState from, GameState to)
        {
            var validStates = ValidStateTransitions(from);
            return validStates != null && validStates.Contains(to);
        }
        private static void ThrowIfInvalidStateTransition(GameState from, GameState to)
        {
            if (!IsValidStateTransition(from, to))
            {
                throw new InvalidOperationException($"Invalid state transition {from} -> {to}");
            }
        }
        private void ChangeState(GameState newState)
        {
            ThrowIfInvalidStateTransition(_validState, newState);
            _validState = newState;
        }
        private bool TryChangeState(GameState newState)
        {
            if(IsValidStateTransition(_validState, newState))
            {
                _validState = newState;
                return true;
            }
            return false;
        }

        private void OnValidate()
        {
            if (!TryChangeState(_state))
            {
                Debug.LogWarning($"Invalid state transition {_validState} -> {_state}");
                _state = _validState;
            }
        }

        public event Action LevelStarted;
        public event Action LevelFinished;

        public void FinishLevel()
        {
            ChangeState(GameState.LevelFinished);
            LevelFinished?.Invoke();
        }
    }
}