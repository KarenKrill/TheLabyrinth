using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using UI.Views.Abstractions;

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
        private readonly IViewFactory _viewFactory;
        private readonly IPresenter<ILooseMenuView> _looseMenuPresenter;
        private readonly IPresenter<IILevelInfoView> _levelInfoPresenter;
        private readonly IGameController _gameController;
    }
}
