namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using StateMachine.Abstractions;
    using UI.Views.Abstractions;

    public class MainMenuState : IStateHandler<GameState>
    {
        public GameState State => GameState.MainMenu;

        public MainMenuState(IPresenter<IMainMenuView> mainMenuPresenter, IViewFactory viewFactory)
        {
            _mainMenuPresenter = mainMenuPresenter;
            _viewFactory = viewFactory;
        }

        public void Enter()
        {
            _mainMenuPresenter.View ??= _viewFactory.Create<IMainMenuView>();
            _mainMenuPresenter.Enable();
        }

        public void Exit()
        {
            _mainMenuPresenter.Disable();
        }

        private readonly IPresenter<IMainMenuView> _mainMenuPresenter;
        private readonly IViewFactory _viewFactory;
    }
}