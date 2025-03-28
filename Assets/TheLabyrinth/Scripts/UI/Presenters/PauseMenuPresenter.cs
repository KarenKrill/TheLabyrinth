using UnityEngine;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.UI.Presenters.Abstractions;
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class PauseMenuPresenter : IPresenter<IPauseMenuView>
    {
        public IPauseMenuView View { get; set; }
        readonly ILogger _logger;
        readonly IGameFlow _gameFlow;
        void OnRestart()
        {
            _gameFlow.StartGame();
        }
        void OnResume()
        {
            _gameFlow.PlayLevel();
        }
        void OnSettings()
        {
            _logger.Log("Settings shown");
            _gameFlow.PlayLevel();
        }
        void OnExit() => _gameFlow.EndGame();
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
    }
}