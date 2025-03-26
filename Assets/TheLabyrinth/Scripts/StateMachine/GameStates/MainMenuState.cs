using Zenject;
using KarenKrill.Common.UI.Views.Abstractions;
using KarenKrill.TheLabyrinth.UI.Views.Abstractions;
using KarenKrill.TheLabyrinth.StateMachine.Abstractions;
using KarenKrill.Common.UI.Presenters.Abstractions;

namespace KarenKrill.TheLabyrinth.GameStates
{
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