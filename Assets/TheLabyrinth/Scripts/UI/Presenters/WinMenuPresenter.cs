using KarenKrill.UI.Presenters.Abstractions;

namespace TheLabyrinth.UI.Presenters
{
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
            View.MainMenuExit += OnMainMenuExit;
            View.Exit += OnExit;
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            View.Restart -= OnRestart;
            View.MainMenuExit -= OnMainMenuExit;
            View.Exit -= OnExit;
        }

        private readonly IGameFlow _gameFlow;

        private void OnRestart() => _gameFlow.StartGame();
        private void OnMainMenuExit() => _gameFlow.LoadMainMenu();
        private void OnExit() => _gameFlow.EndGame();
    }
}