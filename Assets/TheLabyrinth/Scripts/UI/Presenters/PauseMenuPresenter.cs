using UnityEngine;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.UI.Presenters.Abstractions;
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class PauseMenuPresenter : IPresenter<IPauseMenuView>
    {
        public IPauseMenuView View { get; set; }

        public PauseMenuPresenter(ILogger logger, IGameFlow gameFlow)
        {
            _logger = logger;
            _gameFlow = gameFlow;
        }
        public void Enable()
        {
            View.Resume += OnResume;
            View.Restart += OnRestart;
            View.Settings += OnSettings;
            View.Exit += OnExit;
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            View.Resume -= OnResume;
            View.Restart -= OnRestart;
            View.Settings -= OnSettings;
            View.Exit -= OnExit;
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;

        private void OnRestart() => _gameFlow.StartGame();
        private void OnResume() => _gameFlow.PlayLevel();
        private void OnSettings()
        {
            _logger.Log("Settings shown");
        }
        private void OnExit() => _gameFlow.EndGame();
    }
}