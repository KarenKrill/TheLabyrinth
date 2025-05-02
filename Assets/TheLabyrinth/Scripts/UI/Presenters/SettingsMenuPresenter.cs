using KarenKrill.UI.Presenters.Abstractions;

namespace TheLabyrinth.UI.Presenters
{
    using TheLabyrinth.Abstractions;
    using Views.Abstractions;

    public class SettingsMenuPresenter : IPresenter<ISettingsMenuView>
    {
        public ISettingsMenuView View { get; set; }

        public SettingsMenuPresenter(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        public void Enable()
        {
            _gameSettings.ShowFpsChanged += OnModelShowFpsChanged;
            View.ShowFps = _gameSettings.ShowFps;
            View.Apply += OnApply;
            View.Cancel += OnCancel;
            View.Show();
        }

        public void Disable()
        {
            _gameSettings.ShowFpsChanged -= OnModelShowFpsChanged;
            View.Apply -= OnApply;
            View.Cancel -= OnCancel;
            View.Close();
        }

        private readonly GameSettings _gameSettings;

        private void OnModelShowFpsChanged(bool state) => View.ShowFps = state;
        private void OnApply()
        {
            _gameSettings.ShowFps = View.ShowFps;
            Disable();
        }
        private void OnCancel() => Disable();
    }
}