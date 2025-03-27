using UnityEngine;
using Zenject;
using KarenKrill.TheLabyrinth.StateMachine.Abstractions;

namespace KarenKrill.TheLabyrinth.GameStates
{
    public class InitialState : IGameState
    {
        [Inject]
        ILogger _logger;
        public void Enter()
        {

        }
        public void Exit()
        {
        }
    }
}