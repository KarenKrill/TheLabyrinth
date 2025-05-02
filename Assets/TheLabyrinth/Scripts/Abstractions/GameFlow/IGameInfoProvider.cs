#nullable enable

using System;

namespace TheLabyrinth.GameFlow.Abstractions
{
    public interface IGameInfoProvider
    {
        LevelInfo CurrentLevel { get; }

        event Action<LevelInfo>? CurrentLevelChanged;
    }
}
