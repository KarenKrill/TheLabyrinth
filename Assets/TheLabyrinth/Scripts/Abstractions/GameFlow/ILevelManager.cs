#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.GameFlow.Abstractions
{
    public interface ILevelManager
    {
        event Action? LevelLoaded;

        void Reset();
        void OnLevelLoad();
        void OnLevelEnd();
    }
}
