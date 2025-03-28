using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;

    public class GameFlowBehaviour : MonoBehaviour, IGameFlow
    {
        public GameState State => _gameFlow.State;

        [Inject]
        public void Initialize(IGameFlow gameFlow)
        {
            _gameFlow = gameFlow;
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

        IGameFlow _gameFlow;
    }
}
