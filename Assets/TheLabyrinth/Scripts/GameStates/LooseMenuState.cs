using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using StateSystem.Abstractions;
    using UI.Presenters.Abstractions;
    using UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using Input.Abstractions;
    using Movement.Abstractions;
    using UI.Views.Abstractions;

    public class LooseMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.LooseMenu;

        public LooseMenuState(ILogger logger,
            IViewFactory viewFactory,
            IPresenter<ILooseMenuView> looseMenuPresenter,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IGameController gameController,
            IInputActionService inputActionService,
            IPlayerMoveController playerMoveController)
        {
            _logger = logger;
            _viewFactory = viewFactory;
            _looseMenuPresenter = looseMenuPresenter;
            _levelInfoPresenter = levelInfoPresenter;
            _gameController = gameController;
            _inputActionService = inputActionService;
            _playerMoveController = playerMoveController;
        }
        public void Enter(GameState prevState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _playerMoveController.MoveStrategy = null;
            _looseMenuPresenter.View ??= _viewFactory.Create<ILooseMenuView>();
            _looseMenuPresenter.Enable();
            if (_levelInfoPresenter.View is not null)
            {
                _levelInfoPresenter.Disable();
            }
            _inputActionService.SetActionMap(ActionMap.UI);
        }
        public void Exit(GameState nextState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _looseMenuPresenter.Disable();
        }

        private readonly ILogger _logger;
        private readonly IViewFactory _viewFactory;
        private readonly IPresenter<ILooseMenuView> _looseMenuPresenter;
        private readonly IPresenter<IILevelInfoView> _levelInfoPresenter;
        private readonly IGameController _gameController;
        private readonly IInputActionService _inputActionService;
        private readonly IPlayerMoveController _playerMoveController;
    }
}
