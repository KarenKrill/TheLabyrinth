using UnityEngine;

using KarenKrill.StateSystem.Abstractions;
using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.UI.Views.Abstractions;

namespace TheLabyrinth.GameStates
{
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
        public void Enter(GameState prevState)
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
        }
        public void Exit(GameState nextState)
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
