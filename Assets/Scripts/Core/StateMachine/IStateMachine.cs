using System;
using System.Collections.Generic;

namespace KarenKrill.Core
{
    public delegate void StateTransitionDelegate<T>(IStateMachine<T> stateMachine, T state) where T : Enum;
    public interface IStateMachine<T> where T : Enum
    {
        public T State { get; }
        public event StateTransitionDelegate<T> StateEnter;
        public event StateTransitionDelegate<T> StateExit;
        public IEnumerable<T> ValidStateTransitions(T state);
        bool IsCanTransitTo(T state);
        /// <exception cref="InvalidStateMachineTransitionException"></exception>
        void TransitTo(T state);
        bool TryTransitTo(T state);
    }
}