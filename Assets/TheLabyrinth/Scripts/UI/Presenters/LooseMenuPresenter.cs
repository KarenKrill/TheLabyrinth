namespace KarenKrill.TheLabyrinth.UI.Presenters
{
    using Common.UI.Presenters.Abstractions;
    using GameFlow.Abstractions;
    using Views.Abstractions;

    public class LooseMenuPresenter : IPresenter<ILooseMenuView>
    {
        public ILooseMenuView View { get; set; }
        readonly IGameFlow _gameFlow;
        void OnRestart()
        {
            Disable();
            _gameFlow.StartGame();
        }
        void OnExit() => _gameFlow.EndGame();
        public LooseMenuPresenter(IGameFlow gameFlow)
        {
            _gameFlow = gameFlow;
        }
        public void Enable()
        {
            View.Restart += OnRestart;
            View.Exit += OnExit;
            View.Show();
        }
        public void Disable()
        {
            View.Close();
            View.Restart -= OnRestart;
            View.Exit -= OnExit;
        }
    }
}