using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;
    
    public class GameStartState : IStateHandler<GameState>
    {
        public GameState State => GameState.GameStart;

        public GameStartState(ILogger logger, IGameFlow gameFlow, ILevelManager levelManager, IGameController gameController)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _levelManager = levelManager;
            _gameController = gameController;
        }
        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _levelManager.Reset();
            _gameFlow.LoadLevel();
            _gameController.OnGameStart();
        }
        public void Exit() { _logger.Log($"{GetType().Name}.{nameof(Exit)}()"); }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly ILevelManager _levelManager;
        private readonly IGameController _gameController;
    }
}
