using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using Input.Abstractions;
    using UI.Views.Abstractions;

    public class PauseMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.PauseMenu;

        public PauseMenuState(ILogger logger,
            IGameFlow gameFlow,
            IViewFactory viewFactory,
            IPresenter<IPauseMenuView> pauseMenuPresenter,
            IGameController gameController,
            IInputActionService inputActionService)
        {
            _logger = logger;
            _gameFlow = gameFlow;
            _viewFactory = viewFactory;
            _pauseMenuPresenter = pauseMenuPresenter;
            _gameController = gameController;
            _inputActionService = inputActionService;
        }
        public void Enter(GameState prevState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Enter)}()");
            _pauseMenuPresenter.View ??= _viewFactory.Create<IPauseMenuView>();
            _pauseMenuPresenter.Enable();
            _inputActionService.Back += OnResume;
            _inputActionService.SetActionMap(ActionMap.UI);
            _gameController.OnLevelPause();
        }
        public void Exit(GameState nextState)
        {
            _logger.Log($"{GetType().Name}.{nameof(Exit)}()");
            _inputActionService.Back -= OnResume;
            _pauseMenuPresenter.Disable();
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
        private readonly IViewFactory _viewFactory;
        private readonly IPresenter<IPauseMenuView> _pauseMenuPresenter;
        private readonly IGameController _gameController;
        private readonly IInputActionService _inputActionService;

        private void OnResume()
        {
            _gameFlow.PlayLevel();
        }
    }
}
