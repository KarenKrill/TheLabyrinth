#nullable enable

using System;

namespace KarenKrill.Core
{
    public interface IGameController
    {
        int CurrentLevelNumber { get; }
        string? CurrentLevelName { get; }
        event Action? CurrentLevelChanged;
    }
}