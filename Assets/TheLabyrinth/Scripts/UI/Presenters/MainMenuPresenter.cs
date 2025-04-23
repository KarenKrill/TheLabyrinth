namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.UI.Presenters.Abstractions;
    using Common.UI.Views.Abstractions;
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class MainMenuPresenter : IPresenter<IMainMenuView>
    {
        public IMainMenuView View { get; set; }

        public MainMenuPresenter(IGameFlow gameFlow, IPresenter<ISettingsMenuView> settingsPresenter, IViewFactory viewFactory)
        {
            _gameFlow = gameFlow;
            _settingsPresenter = settingsPresenter;
            _viewFactory = viewFactory;
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

        private readonly IGameFlow _gameFlow;
        private readonly IPresenter<ISettingsMenuView> _settingsPresenter;
        private readonly IViewFactory _viewFactory;

        private void OnNewGame() => _gameFlow.StartGame();
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
        private void OnExit() => _gameFlow.EndGame();
    }
}