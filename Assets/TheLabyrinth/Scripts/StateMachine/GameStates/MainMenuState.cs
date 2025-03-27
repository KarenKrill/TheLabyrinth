namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using StateMachine.Abstractions;
    using UI.Views.Abstractions;

    public class MainMenuState : IGameState
    {
        IPresenter<IMainMenuView> _mainMenuPresenter;
        IViewFactory _viewFactory;

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
    }
}