using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;

    public class LevelFinishState : IStateHandler<GameState>
    {
        public GameState State => GameState.LevelFinish;

        public LevelFinishState(ILogger logger, IGameFlow gameFlow, ILevelManager levelManager, IGameController gameController)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _levelManager = levelManager;
            _gameController = gameController;
        }
        public void Enter(GameState prevState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _levelManager.LevelUnloaded += OnLevelUnloaded;
            _levelManager.OnLevelEnd();
        }
        public void Exit(GameState nextState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _levelManager.LevelUnloaded -= OnLevelUnloaded;
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly ILevelManager _levelManager;
        private readonly IGameController _gameController;

        private void OnLevelUnloaded()
        {
            _gameController.OnLevelFinish();
        }
    }
}
