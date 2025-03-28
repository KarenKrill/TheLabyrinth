using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;

    public class LevelPlayState : IStateHandler<GameState>
    {
        public GameState State => GameState.LevelPlay;

        public LevelPlayState(ILogger logger, IGameFlow gameFlow, IGameController gameController)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _gameController = gameController;
        }
        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _gameController.OnLevelPlay();
        }
        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IGameController _gameController;
    }
}
