using System;

namespace KarenKrill.TheLabyrinth.StateMachine.Abstractions
{
    public interface IStateHandler<StateEnumType> where StateEnumType : Enum
    {
        /// <summary>
        /// Processable state
        /// </summary>
        public StateEnumType State { get; }
        void Enter();
        void Exit();
    }
}