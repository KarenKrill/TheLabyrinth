#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.GameFlow.Abstractions
{
    public interface ILevelManager
    {
        event Action? LevelLoaded;
        event Action? LevelUnloaded;

        void OnLevelLoad();
        void OnLevelEnd();
    }
}
