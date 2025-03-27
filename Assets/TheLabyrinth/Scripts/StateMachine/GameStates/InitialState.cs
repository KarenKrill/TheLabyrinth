using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using GameFlow.Abstractions;
    using StateMachine.Abstractions;

    public class InitialState : IStateHandler<GameState>
    {
        public GameState State => GameState.Initial;

        public InitialState(ILogger logger)
        {
            _logger = logger;
        }

        public void Enter()
        {
        }

        public void Exit()
        {
        }

        private readonly ILogger _logger;
    }
}