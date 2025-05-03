using KarenKrill.UI.Presenters.Abstractions;
using KarenKrill.UI.Views.Abstractions;

namespace TheLabyrinth.UI.Presenters
{
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class PauseMenuPresenter : IPresenter<IPauseMenuView>
    {
        public IPauseMenuView View { get; set; }

        public PauseMenuPresenter(IGameFlow gameFlow, IPresenter<ISettingsMenuView> settingsPresenter, IViewFactory viewFactory)
        {
            _gameFlow = gameFlow;
            _settingsPresenter = settingsPresenter;
            _viewFactory = viewFactory;
        }
        public void Enable()
        {
            View.Resume += OnResume;
            View.Restart += OnRestart;
            View.Settings += OnSettings;
            View.MainMenuExit += OnMainMenuExit;
            View.Exit += OnExit;
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            View.Resume -= OnResume;
            View.Restart -= OnRestart;
            View.Settings -= OnSettings;
            View.MainMenuExit -= OnMainMenuExit;
            View.Exit -= OnExit;
        }

        private readonly IGameFlow _gameFlow;
        private readonly IPresenter<ISettingsMenuView> _settingsPresenter;
        private readonly IViewFactory _viewFactory;

        private void OnRestart() => _gameFlow.StartGame();
        private void OnResume() => _gameFlow.PlayLevel();
        private void OnSettings()
        {
            _settingsPresenter.View ??= _viewFactory.Create<ISettingsMenuView>();
            _settingsPresenter.View.Apply += OnClosing;
            _settingsPresenter.View.Cancel += OnClosing;
            View.Close();
            _settingsPresenter.Enable();
        }
        private void OnClosing()
        {
            _settingsPresenter.View.Apply -= OnClosing;
            _settingsPresenter.View.Cancel -= OnClosing;
            View.Show();
        }
        private void OnMainMenuExit() => _gameFlow.LoadMainMenu();
        private void OnExit() => _gameFlow.EndGame();
    }
}