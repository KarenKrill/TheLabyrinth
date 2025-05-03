#nullable enable

namespace TheLabyrinth.GameFlow.Abstractions
{
    public interface IGameController
    {
        LevelInfo CurrentLevel { get; set; }

        void StartGame();
        void FinishLevel();
    }
}