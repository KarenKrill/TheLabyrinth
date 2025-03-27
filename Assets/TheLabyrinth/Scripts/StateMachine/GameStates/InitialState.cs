using UnityEngine;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using StateMachine.Abstractions;

    public class InitialState : IGameState
    {
        ILogger _logger;

        public InitialState(ILogger logger)
        {
            _logger = logger;
        }

        public void Enter()
        {

        }
    }
}