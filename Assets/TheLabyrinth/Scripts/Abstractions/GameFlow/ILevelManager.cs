#nullable enable

using System;

namespace TheLabyrinth.GameFlow.Abstractions
{
    public interface ILevelManager
    {
        event Action? LevelLoaded;
        event Action? LevelUnloaded;

        void LoadLevel();
        void UnloadLevel();
    }
}
