using UnityEngine;
using KarenKrill.Core;
using KarenKrill.Core.UI.Presenters;
using KarenKrill.UI.Views;

namespace KarenKrill.UI.Presenters
{
    public class PauseMenuPresenter : IPresenter
    {
        readonly IPauseMenuView _view;
        readonly ILogger _logger;
        readonly IGameFlow _gameFlow;
        void OnRestart()
        {
            Disable();
            _gameFlow.StartGame();
        }
        void OnResume()
        {
            Disable();
            _gameFlow.PlayLevel();
        }
        void OnSettings()
        {
            _logger.Log("Settings shown");
            Disable();
            _gameFlow.PlayLevel();
        }
        void OnExit() => _gameFlow.EndGame();
        public PauseMenuPresenter(IPauseMenuView view, ILogger logger, IGameFlow gameFlow)
        {
            _view = view;
            _logger = logger;
            _gameFlow = gameFlow;
        }
        public void Enable()
        {
            _view.Resume += OnResume;
            _view.Restart += OnRestart;
            _view.Settings += OnSettings;
            _view.Exit += OnExit;
            _view.Show();
        }
        public void Disable()
        {
            _view.Close();
            _view.Resume -= OnResume;
            _view.Restart -= OnRestart;
            _view.Settings -= OnSettings;
            _view.Exit -= OnExit;
        }
    }
}