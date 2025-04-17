#nullable enable

using System;

namespace KarenKrill.Common.GameInfo.Abstractions
{
    public interface IGameInfoProvider
    {
        int CurrentLevelNumber { get; }
        string? CurrentLevelName { get; }

        event Action? CurrentLevelChanged;
    }
}
