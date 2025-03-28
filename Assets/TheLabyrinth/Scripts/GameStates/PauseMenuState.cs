using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using UI.Views.Abstractions;

    public class PauseMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.PauseMenu;

        public PauseMenuState(ILogger logger,
            IGameFlow gameFlow,
            IViewFactory viewFactory,
            IPresenter<IPauseMenuView> pauseMenuPresenter,
            IGameController gameController)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _viewFactory = viewFactory;
            _pauseMenuPresenter = pauseMenuPresenter;
            _gameController = gameController;
        }
        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _pauseMenuPresenter.View ??= _viewFactory.Create<IPauseMenuView>();
            _pauseMenuPresenter.Enable();
            _gameController.OnLevelPause();
        }
        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _pauseMenuPresenter.Disable();
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IViewFactory _viewFactory;
        private readonly IPresenter<IPauseMenuView> _pauseMenuPresenter;
        private readonly IGameController _gameController;
    }
}
