namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.UI.Presenters.Abstractions;
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class LooseMenuPresenter : IPresenter<ILooseMenuView>
    {
        public ILooseMenuView View { get; set; }

        public LooseMenuPresenter(IGameFlow gameFlow)
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