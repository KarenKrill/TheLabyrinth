using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using Input.Abstractions;
    using Movement.Abstractions;
    using UI.Views.Abstractions;

    public class LoadLevelState : IStateHandler<GameState>
    {
        public GameState State => GameState.LevelLoad;

        public LoadLevelState(ILogger logger,
            IGameFlow gameFlow,
            ILevelManager levelManager,
            IViewFactory viewFactory,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IGameController gameController,
            IInputActionService inputActionService,
            IPlayerMoveController playerMoveController,
            IPlayerInputMoveStrategy playerInputMoveStrategy)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _levelManager = levelManager;
            _viewFactory = viewFactory;
            _levelInfoPresenter = levelInfoPresenter;
            _gameController = gameController;
            _inputActionService = inputActionService;
            _playerMoveController = playerMoveController;
            _playerInputMoveStrategy = playerInputMoveStrategy;
        }
        public void Enter(GameState prevState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _levelManager.LevelLoaded += OnLevelLoaded;
            _previousPlayerMoveStrategy = _playerMoveController.MoveStrategy;
            _playerMoveController.MoveStrategy = _playerInputMoveStrategy;
            _levelManager.OnLevelLoad();
            _levelInfoPresenter.View ??= _viewFactory.Create<IILevelInfoView>();
            _levelInfoPresenter.Enable();
            _inputActionService.Disable();
            _gameController.OnLevelLoad();
        }
        public void Exit(GameState nextState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _levelManager.LevelLoaded -= OnLevelLoaded;
            _playerMoveController.MoveStrategy = _previousPlayerMoveStrategy;
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly ILevelManager _levelManager;
        private readonly IViewFactory _viewFactory;
        private readonly IPresenter<IILevelInfoView> _levelInfoPresenter;
        private readonly IGameController _gameController;
        private readonly IInputActionService _inputActionService;
        private readonly IPlayerMoveController _playerMoveController;
        private readonly IPlayerInputMoveStrategy _playerInputMoveStrategy;
        private IMoveStrategy _previousPlayerMoveStrategy;

        private void OnLevelLoaded()
        {
            _gameFlow.PlayLevel();
        }
    }
}
