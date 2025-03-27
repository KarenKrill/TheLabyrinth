using UnityEngine;
using Zenject;

namespace KarenKrill.TheLabyrinth.GameStates
{
    using StateMachine.Abstractions;

    public class InitialState : IGameState
    {
        [Inject]
        ILogger _logger;
        public void Enter()
        {

        }
    }
}