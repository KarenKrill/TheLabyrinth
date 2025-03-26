using UnityEngine;
using KarenKrill.Common.UI.Presenters.Abstractions;

namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using KarenKrill.TheLabyrinth.GameFlow.Abstractions;
    using Views.Abstractions;

    public class MainMenuPresenter : IPresenter<IMainMenuView>
    {
        public IMainMenuView View { get; set; }
        readonly ILogger _logger;
        readonly IGameFlow _gameFlow;
        void OnNewGame()
        {
            Disable();
            _gameFlow.StartGame();
        }
        void OnSettings()
        {
            _logger.Log("Settings shown");
        }
        void OnExit() => _gameFlow.EndGame();
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
    }
}