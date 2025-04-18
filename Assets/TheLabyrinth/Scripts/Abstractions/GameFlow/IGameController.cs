#nullable enable

using System;

namespace KarenKrill.TheLabyrinth.GameFlow.Abstractions
{
    public interface IGameController
    {
        LevelInfo CurrentLevel { get; set; }

        void StartGame();
        void FinishLevel();
    }
}