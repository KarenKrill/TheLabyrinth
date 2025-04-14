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

    public class WinMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.WinMenu;

        public WinMenuState(ILogger logger,
            IViewFactory viewFactory,
            IPresenter<IWinMenuView> winMenuPresenter,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IGameController gameController,
            IInputActionService inputActionService,
            IPlayerMoveController playerMoveController)
        {
            _logger = logger;
            _viewFactory = viewFactory;
            _winMenuPresenter = winMenuPresenter;
            _levelInfoPresenter = levelInfoPresenter;
            _gameController = gameController;
            _inputActionService = inputActionService;
            _playerMoveController = playerMoveController;
        }
        public void Enter()
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _playerMoveController.MoveStrategy = null;
            _winMenuPresenter.View ??= _viewFactory.Create<IWinMenuView>();
            _winMenuPresenter.Enable();
            if (_levelInfoPresenter.View is not null)
            {
                _levelInfoPresenter.Disable();
            }
            _inputActionService.SetActionMap(ActionMap.UI);
            _gameController.OnPlayerWin();
        }
        public void Exit()
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _winMenuPresenter.Disable();
        }

        private readonly ILogger _logger;
        private readonly IViewFactory _viewFactory;
        private readonly IPresenter<IWinMenuView> _winMenuPresenter;
        private readonly IPresenter<IILevelInfoView> _levelInfoPresenter;
        private readonly IGameController _gameController;
        private readonly IInputActionService _inputActionService;
        private readonly IPlayerMoveController _playerMoveController;
    }
}
