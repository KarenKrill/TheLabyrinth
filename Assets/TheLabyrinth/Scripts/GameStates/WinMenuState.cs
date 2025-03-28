using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using UI.Views.Abstractions;

    public class WinMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.WinMenu;

        public WinMenuState(ILogger logger,
            IViewFactory viewFactory,
            IPresenter<IWinMenuView> winMenuPresenter,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IGameController gameController)
        {
            _logger = logger;
            _viewFactory = viewFactory;
            _winMenuPresenter = winMenuPresenter;
            _levelInfoPresenter = levelInfoPresenter;
            _gameController = gameController;
        }

        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _winMenuPresenter.View ??= _viewFactory.Create<IWinMenuView>();
            _winMenuPresenter.Enable();
            if (_levelInfoPresenter.View is not null)
            {
                _levelInfoPresenter.Disable();
            }
            _gameController.OnPlayerWin();
        }

        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _winMenuPresenter.Disable();
        }

        private readonly ILogger _logger;
        IViewFactory _viewFactory;
        IPresenter<IWinMenuView> _winMenuPresenter;
        IPresenter<IILevelInfoView> _levelInfoPresenter;
        IGameController _gameController;
    }
}
