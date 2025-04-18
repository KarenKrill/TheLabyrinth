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
        void OnLevelFinish();
    }
}