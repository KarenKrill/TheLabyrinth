namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;
    using KarenKrill.Common.UI.Presenters.Abstractions;
    using KarenKrill.TheLabyrinth.UI.Views.Abstractions;
    using KarenKrill.Common.UI.Views.Abstractions;
    using UnityEngine;

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
        IGameFlow _gameFlow;
        IViewFactory _viewFactory;
        IPresenter<IPauseMenuView> _pauseMenuPresenter;
        IGameController _gameController;
    }
}
