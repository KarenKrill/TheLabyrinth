namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.UI.Presenters.Abstractions;
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class WinMenuPresenter : IPresenter<IWinMenuView>
    {
        public IWinMenuView View { get; set; }

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

        private readonly IGameFlow _gameFlow;

        private void OnRestart() => _gameFlow.StartGame();
        private void OnExit() => _gameFlow.EndGame();
    }
}