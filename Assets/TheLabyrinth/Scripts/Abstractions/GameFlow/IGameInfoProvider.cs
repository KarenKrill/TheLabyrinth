#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.GameFlow.Abstractions
{
    public interface IGameInfoProvider
    {
        LevelInfo CurrentLevel { get; }

        event Action<LevelInfo>? CurrentLevelChanged;
    }
}
