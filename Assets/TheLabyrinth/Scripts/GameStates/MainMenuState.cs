using UnityEngine;

using KarenKrill.StateSystem.Abstractions;
using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.UI.Views.Abstractions;

namespace TheLabyrinth.GameStates
{
    using GameFlow.Abstractions;
    using UI.Views.Abstractions;

    public class MainMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.MainMenu;

        public MainMenuState(ILogger logger,
            IPresenter<IMainMenuView> mainMenuPresenter,
            IPresenter<IILevelInfoView> levelInfoPresenter,
            IViewFactory viewFactory)
        {
            _logger = logger;
            _mainMenuPresenter = mainMenuPresenter;
            _levelInfoPresenter = levelInfoPresenter;
            _viewFactory = viewFactory;
        }
        public void Enter(GameState prevState)
        {
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Enter)}()");
            _mainMenuPresenter.View ??= _viewFactory.Create<IMainMenuView>();
            if (_levelInfoPresenter.View is not null)
            {
                _levelInfoPresenter.Disable();
            }
            _mainMenuPresenter.Enable();
        }
        public void Exit(GameState nextState)
        {
            _logger.Log($"{nameof(MainMenuState)}.{nameof(Exit)}()");
            _mainMenuPresenter.Disable();
        }
        
        private readonly ILogger _logger;
        private readonly IPresenter<IMainMenuView> _mainMenuPresenter;
        private readonly IPresenter<IILevelInfoView> _levelInfoPresenter;
        private readonly IViewFactory _viewFactory;
    }
}