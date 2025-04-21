#nullable enable

using System;

namespace KarenKrill.Common.Diagnostics.Abstractions
{
    public interface IDiagnosticsProvider
    {
        PerfomanceInfo PerfomanceInfo { get; }

        event Action<PerfomanceInfo>? PerfomanceInfoChanged;
    }
}
