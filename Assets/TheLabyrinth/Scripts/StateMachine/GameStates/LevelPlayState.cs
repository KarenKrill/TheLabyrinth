namespace KarenKrill.TheLabyrinth.GameStates
{
    using StateMachine.Abstractions;
    using GameFlow.Abstractions;
    using UnityEngine;

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
        IGameFlow _gameFlow;
        IGameController _gameController;
    }
}
