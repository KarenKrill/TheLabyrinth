using UnityEngine;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.UI.Presenters.Abstractions;
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class MainMenuPresenter : IPresenter<IMainMenuView>
    {
        public IMainMenuView View { get; set; }

        public MainMenuPresenter(ILogger logger, IGameFlow gameFlow)
        {
            _logger = logger;
            _gameFlow = gameFlow;
        }
        public void Enable()
        {
            View.NewGame += OnNewGame;
            View.Settings += OnSettings;
            View.Exit += OnExit;
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            View.NewGame -= OnNewGame;
            View.Settings -= OnSettings;
            View.Exit -= OnExit;
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;

        private void OnNewGame() => _gameFlow.StartGame();
        private void OnSettings()
        {
            _logger.Log("Settings shown");
        }
        private void OnExit() => _gameFlow.EndGame();
    }
}