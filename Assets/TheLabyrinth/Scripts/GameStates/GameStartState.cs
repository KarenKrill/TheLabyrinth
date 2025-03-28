namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;
    using UnityEngine;

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
        IGameFlow _gameFlow;
        ILevelManager _levelManager;
        IGameController _gameController;
    }
}
