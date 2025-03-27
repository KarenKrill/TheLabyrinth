using Zenject;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using StateMachine.Abstractions;
    using UI.Views.Abstractions;

    public class MainMenuState : IGameState
    {
        [Inject]
        IPresenter<IMainMenuView> _mainMenuPresenter;
        [Inject]
        IViewFactory _viewFactory;
        public void Enter()
        {
            _mainMenuPresenter.View ??= _viewFactory.Create<IMainMenuView>();
            _mainMenuPresenter.Enable();
        }
    }
}