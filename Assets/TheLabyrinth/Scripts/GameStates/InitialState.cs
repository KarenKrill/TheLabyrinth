using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using Common.StateSystem.Abstractions;
    using GameFlow.Abstractions;

    public class InitialState : IStateHandler<GameState>
    {
        public GameState State => GameState.Initial;

        public InitialState(ILogger logger, IGameFlow gameFlow)
        {
            _logger = logger;
            _gameFlow = gameFlow;
        }

        public void Enter()
        {
            _logger.Log($"{nameof(InitialState)}.{nameof(Enter)}()");
            _gameFlow.LoadMainMenu();
        }

        public void Exit()
        {
            _logger.Log($"{nameof(InitialState)}.{nameof(Exit)}()");
        }

        private readonly ILogger _logger;
        private readonly IGameFlow _gameFlow;
    }
}