using KarenKrill.Common.UI.Presenters.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using KarenKrill.TheLabyrinth.GameFlow.Abstractions;
    using Views.Abstractions;

    public class WinMenuPresenter : IPresenter<IWinMenuView>
    {
        public IWinMenuView View { get; set; }
        readonly IGameFlow _gameFlow;
        void OnRestart()
        {
            Disable();
            _gameFlow.StartGame();
        }
        void OnExit() => _gameFlow.EndGame();
        public WinMenuPresenter(IGameFlow gameFlow)
        {
            _gameFlow = gameFlow;
        }
        public void Enable()
        {
            View.Restart += OnRestart;
            View.Exit += OnExit;
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            View.Restart -= OnRestart;
            View.Exit -= OnExit;
        }
    }
}