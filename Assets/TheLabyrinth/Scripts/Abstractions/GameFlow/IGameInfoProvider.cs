#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.GameFlow.Abstractions
{
    public interface IGameInfoProvider
    {
        int CurrentLevelNumber { get; }
        string? CurrentLevelName { get; }

        event Action? CurrentLevelChanged;
    }
}
