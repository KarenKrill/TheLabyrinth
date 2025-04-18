#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.GameFlow.Abstractions
{
    public interface IGameController
    {
        LevelInfo CurrentLevel { get; set; }
        string? CurrentLevelName { get; }

        event Action? CurrentLevelChanged;

        void StartGame();
        void FinishLevel();
    }
}