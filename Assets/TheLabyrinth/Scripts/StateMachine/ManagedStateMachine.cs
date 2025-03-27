using System;
using System.Collections.Generic;
using System.Linq;

namespace KarenKrill.TheLabyrinth.StateMachine
{
    using Abstractions;

    public class ManagedStateMachine<T> : IManagedStateMachine<T> where T : Enum
    {
        public IStateMachine<T> StateMachine { get; }

        public ManagedStateMachine(IStateMachine<T> stateMachine, IEnumerable<IStateHandler<T>> stateHandlers, T initialState)
        {
            StateMachine = stateMachine;
            _initialState = initialState;
            _stateHandlers = stateHandlers.ToDictionary(stateHandler => stateHandler.State, stateHandler => stateHandler);
        }

        public void AddStateHandler(IStateHandler<T> stateHandler)
        {
            _stateHandlers[stateHandler.State] = stateHandler;
        }

        public void RemoveStateHandler(T state)
        {
            _stateHandlers.Remove(state);
        }

        public void Start()
        {
            StateMachine.StateEnter += OnStateEnter;
            StateMachine.StateExit += OnStateExit;
            StateMachine.TransitTo(_initialState);
        }

        private void OnStateEnter(IStateMachine<T> stateMachine, T state)
        {
            _stateHandlers[state].Enter();
        }

        private void OnStateExit(IStateMachine<T> stateMachine, T state)
        {
            _stateHandlers[state].Exit();
        }

        private T _initialState;

        private Dictionary<T, IStateHandler<T>> _stateHandlers;
    }
}
