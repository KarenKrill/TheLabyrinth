#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.GameFlow.Abstractions
{
    public interface IGameController
    {
        int CurrentLevelNumber { get; }
        string? CurrentLevelName { get; }

        event Action? CurrentLevelChanged;

        void OnGameStart();
        void OnGameEnd();
        void OnLevelLoad();
        void OnLevelPlay();
        void OnLevelPause();
        void OnLevelFinish();
        void OnPlayerLoose();
        void OnPlayerWin();
    }
}