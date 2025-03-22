using System;

namespace KarenKrill.TheLabyrinth.GameFlow.Abstractions
{
    public enum GameState
    {
        Initial,
        GameStart, // ui manager (disable all menus, enable main menu), scene loader (load menu scene)
        MainMenu, // ui manager, player data (statistics, records, info to continue game, reset data for new game), level loader (load level)
        LevelLoad,
        LevelPlay,
        LevelFinish,
        PauseMenu,
        WinMenu,
        LooseMenu,
        GameEnd
    }
    public interface IGameFlow
    {
        GameState State { get; }
        event Action GameStart;
        event Action MainMenuLoad;
        event Action LevelLoad;
        event Action LevelPlay;
        event Action LevelFinish;
        event Action LevelPause;
        event Action PlayerWin;
        event Action PlayerLoose;
        event Action GameEnd;
        void LoadMainMenu();
        void LoadLevel();
        void PlayLevel();
        void PauseLevel();
        void FinishLevel();
        void StartGame();
        void EndGame();
        void WinGame();
        void LooseGame();
    }
}