using KarenKrill.Common.UI.Presenters.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using KarenKrill.TheLabyrinth.GameFlow.Abstractions;
    using Views.Abstractions;

    public class WinMenuPresenter : IPresenter
    {
        readonly IWinMenuView _view;
        readonly IGameFlow _gameFlow;
        void OnRestart()
        {
            Disable();
            _gameFlow.StartGame();
        }
        void OnExit() => _gameFlow.EndGame();
        public WinMenuPresenter(IWinMenuView view, IGameFlow gameFlow)
        {
            _view = view;
            _gameFlow = gameFlow;
        }
        public void Enable()
        {
            _view.Restart += OnRestart;
            _view.Exit += OnExit;
            _view.Show();
        }
        public void Disable()
        {
            _view.Close();
            _view.Restart -= OnRestart;
            _view.Exit -= OnExit;
        }
    }
}