using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using Movement.Abstractions;
    using UI.Views.Abstractions;

    public class PauseMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.PauseMenu;

        public PauseMenuState(ILogger logger,
            IGameFlow gameFlow,
            IViewFactory viewFactory,
            IPresenter<IPauseMenuView> pauseMenuPresenter,
            IGameController gameController,
            IPlayerMoveController playerMoveController)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _viewFactory = viewFactory;
            _pauseMenuPresenter = pauseMenuPresenter;
            _gameController = gameController;
            _playerMoveController = playerMoveController;
        }
        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _pauseMenuPresenter.View ??= _viewFactory.Create<IPauseMenuView>();
            _pauseMenuPresenter.Enable();
            _prevMoveStrategy = _playerMoveController.MoveStrategy;
            _playerMoveController.MoveStrategy = null;
            _gameController.OnLevelPause();
        }
        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _playerMoveController.MoveStrategy = _prevMoveStrategy;
            _pauseMenuPresenter.Disable();
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IViewFactory _viewFactory;
        private readonly IPresenter<IPauseMenuView> _pauseMenuPresenter;
        private readonly IGameController _gameController;
        private readonly IPlayerMoveController _playerMoveController;
        private IMoveStrategy _prevMoveStrategy;
    }
}
