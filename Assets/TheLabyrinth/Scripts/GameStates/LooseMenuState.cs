namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;
    using KarenKrill.Common.UI.Presenters.Abstractions;
    using KarenKrill.Common.UI.Views.Abstractions;
    using KarenKrill.TheLabyrinth.UI.Views.Abstractions;
    using UnityEngine;

    public class LooseMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.LooseMenu;

        public LooseMenuState(ILogger logger,
            IViewFactory viewFactory,
            IPresenter<ILooseMenuView> looseMenuPresenter,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IGameController gameController)
        {
            _logger = logger;
            _viewFactory = viewFactory;
            _looseMenuPresenter = looseMenuPresenter;
            _levelInfoPresenter = levelInfoPresenter;
            _gameController = gameController;
        }

        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _looseMenuPresenter.View ??= _viewFactory.Create<ILooseMenuView>();
            _looseMenuPresenter.Enable();
            if (_levelInfoPresenter.View is not null)
            {
                _levelInfoPresenter.Disable();
            }
            _gameController.OnPlayerLoose();
        }

        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _looseMenuPresenter.Disable();
        }

        private readonly ILogger _logger;
        IViewFactory _viewFactory;
        IPresenter<ILooseMenuView> _looseMenuPresenter;
        IPresenter<IILevelInfoView> _levelInfoPresenter;
        IGameController _gameController;
    }
}
