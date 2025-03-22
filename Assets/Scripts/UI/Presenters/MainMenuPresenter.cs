using UnityEngine;
using KarenKrill.Core;
using KarenKrill.Core.UI.Presenters;
using KarenKrill.UI.Views;

namespace KarenKrill.UI.Presenters
{
    public class MainMenuPresenter : IPresenter<IMainMenuView>
    {
        public IMainMenuView View { get; }
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
        public MainMenuPresenter(IMainMenuView view, ILogger logger, IGameFlow gameFlow)
        {
            View = view;
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