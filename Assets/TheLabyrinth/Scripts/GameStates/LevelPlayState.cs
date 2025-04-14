using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using Common.Logging;
    using GameFlow.Abstractions;
    using Input.Abstractions;
    using Movement.Abstractions;

    public class LevelPlayState : IStateHandler<GameState>
    {
        public GameState State => GameState.LevelPlay;

        public LevelPlayState(ILogger logger,
            IGameFlow gameFlow,
            IGameController gameController,
            IInputActionService inputActionService,
            IPlayerMoveController playerMoveController,
            IPlayerInputMoveStrategy playerInputMoveStrategy,
            IAiMoveStrategy playerAiMoveStrategy)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _gameController = gameController;
            _inputActionService = inputActionService;
            _playerMoveController = playerMoveController;
            _playerInputMoveStrategy = playerInputMoveStrategy;
            _playerAiMoveStrategy = playerAiMoveStrategy;
        }
        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _playerMoveController.MoveStrategy ??= _playerInputMoveStrategy;
            _inputActionService.AutoPlayCheat += OnAutoPlayCheat;
            _inputActionService.Pause += OnPaused;
            _inputActionService.SetActionMap(ActionMap.InGame);
            _gameController.OnLevelPlay();
        }
        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _inputActionService.AutoPlayCheat -= OnAutoPlayCheat;
            _inputActionService.Pause -= OnPaused;
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IGameController _gameController;
        private readonly IInputActionService _inputActionService;
        private readonly IPlayerMoveController _playerMoveController;
        private readonly IPlayerInputMoveStrategy _playerInputMoveStrategy;
        private readonly IAiMoveStrategy _playerAiMoveStrategy;

        private void OnAutoPlayCheat()
        {
            switch (_playerMoveController.MoveStrategy)
            {
                case IPlayerInputMoveStrategy:
                    _playerMoveController.MoveStrategy = _playerAiMoveStrategy;
                    break;
                case IAiMoveStrategy:
                    _playerMoveController.MoveStrategy = _playerInputMoveStrategy;
                    break;
                default:
                    _logger.LogWarning(string.Format("{0} {1} isn't handled", _playerMoveController.MoveStrategy, nameof(_playerMoveController.MoveStrategy)));
                    _playerMoveController.MoveStrategy = _playerInputMoveStrategy;
                    break;
            }
        }
        private void OnPaused()
        {
            _gameFlow.PauseLevel();
        }
    }
}
