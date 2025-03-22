using System;
using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;

    public class GameFlowBehaviour : MonoBehaviour, IGameFlow
    {
        [Inject]
        IGameFlow _gameFlow;

        public GameState State => _gameFlow.State;

        public event Action GameStart
        {
            add
            {
                _gameFlow.GameStart += value;
            }
            remove
            {
                _gameFlow.GameStart -= value;
            }
        }
        public event Action LevelPause
        {
            add
            {
                _gameFlow.LevelPause += value;
            }
            remove
            {
                _gameFlow.LevelPause -= value;
            }
        }
        public event Action GameEnd
        {
            add
            {
                _gameFlow.GameEnd += value;
            }
            remove
            {
                _gameFlow.GameEnd -= value;
            }
        }
        public event Action MainMenuLoad
        {
            add
            {
                _gameFlow.MainMenuLoad += value;
            }
            remove
            {
                _gameFlow.MainMenuLoad -= value;
            }
        }
        public event Action LevelLoad
        {
            add
            {
                _gameFlow.LevelLoad += value;
            }
            remove
            {
                _gameFlow.LevelLoad -= value;
            }
        }
        public event Action LevelFinish
        {
            add
            {
                _gameFlow.LevelFinish += value;
            }
            remove
            {
                _gameFlow.LevelFinish -= value;
            }
        }
        public event Action PlayerWin
        {
            add
            {
                _gameFlow.PlayerWin += value;
            }
            remove
            {
                _gameFlow.PlayerWin -= value;
            }
        }
        public event Action PlayerLoose
        {
            add
            {
                _gameFlow.PlayerLoose += value;
            }
            remove
            {
                _gameFlow.PlayerLoose -= value;
            }
        }

        public event Action LevelPlay
        {
            add
            {
                _gameFlow.LevelPlay += value;
            }
            remove
            {
                _gameFlow.LevelPlay -= value;
            }
        }

        public void FinishLevel() => _gameFlow.FinishLevel();
        public void LoadLevel() => _gameFlow.LoadLevel();
        public void LoadMainMenu() => _gameFlow.LoadMainMenu();
        public void PauseLevel() => _gameFlow.PauseLevel();
        public void StartGame() => _gameFlow.StartGame();
        public void LooseGame() => _gameFlow.LooseGame();
        public void WinGame() => _gameFlow.WinGame();
        public void PlayLevel() => _gameFlow.PlayLevel();
        public void EndGame() => _gameFlow.EndGame();
    }
}
