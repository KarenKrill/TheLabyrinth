using KarenKrill.Core;
using KarenKrill.Core.UI.Presenters;
using KarenKrill.UI.Views;

namespace KarenKrill.UI.Presenters
{
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