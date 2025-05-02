using UnityEngine;

using KarenKrill.StateSystem.Abstractions;

namespace TheLabyrinth.GameStates
{
    using GameFlow.Abstractions;
    using Input.Abstractions;

    public class GameStartState : IStateHandler<GameState>
    {
        public GameState State => GameState.GameStart;

        public GameStartState(ILogger logger, IGameFlow gameFlow, ILevelManager levelManager, IGameController gameController, IInputActionService inputActionService)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _levelManager = levelManager;
            _gameController = gameController;
            _inputActionService = inputActionService;
        }
        public void Enter(GameState prevState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _inputActionService.Disable();
            _gameController.StartGame();
            _gameFlow.LoadLevel();
        }
        public void Exit(GameState nextState) { _logger.Log($"{GetType().Name}.{nameof(Exit)}()"); }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly ILevelManager _levelManager;
        private readonly IGameController _gameController;
        private readonly IInputActionService _inputActionService;
    }
}
