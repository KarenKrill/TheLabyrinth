namespace KarenKrill.TheLabyrinth.GameFlow
{
    using Abstractions;
    using StateSystem.Abstractions;

    public class GameFlow : IGameFlow
    {
        public GameState State => _stateSwitcher.State;

        public GameFlow(IStateSwitcher<GameState> stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
        public void FinishLevel() => _stateSwitcher.TransitTo(GameState.LevelFinish);
        public void LoadMainMenu() => _stateSwitcher.TransitTo(GameState.MainMenu);
        public void LoadLevel() => _stateSwitcher.TransitTo(GameState.LevelLoad);
        public void PauseLevel() => _stateSwitcher.TransitTo(GameState.PauseMenu);
        public void StartGame() => _stateSwitcher.TransitTo(GameState.GameStart);
        public void EndGame() => _stateSwitcher.TransitTo(GameState.GameEnd);
        public void WinGame() => _stateSwitcher.TransitTo(GameState.WinMenu);
        public void LooseGame() => _stateSwitcher.TransitTo(GameState.LooseMenu);
        public void PlayLevel() => _stateSwitcher.TransitTo(GameState.LevelPlay);

        private readonly IStateSwitcher<GameState> _stateSwitcher;
    }
}