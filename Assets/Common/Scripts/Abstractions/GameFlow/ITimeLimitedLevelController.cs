﻿using KarenKrill.Common.Abstractions;

namespace KarenKrill.Common.GameFlow.Abstractions
{
    public interface ITimeLimitedLevelController : ILevelController, IAbility
    {
        float MaxCompleteTime { get; set; }
        float RemainingTime { get; set; }
        float WarningTime { get; set; }
        float LastWarningTime { get; set; }
        void OnLevelPlay();
    }
}