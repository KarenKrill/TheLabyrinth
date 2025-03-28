using System;
using System.Collections.Generic;
using System.Linq;

namespace KarenKrill.Common.StateSystem
{
    using Abstractions;

    public class ManagedStateMachine<T> : IManagedStateMachine<T> where T : Enum
    {
        public IStateMachine<T> StateMachine { get; }

        public ManagedStateMachine(IStateMachine<T> stateMachine, IEnumerable<IStateHandler<T>> stateHandlers)
        {
            StateMachine = stateMachine;
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
            StateMachine.StateSwitcher.TransitToInitial();
        }

        private Dictionary<T, IStateHandler<T>> _stateHandlers;

        private void OnStateEnter(T state)
        {
            _stateHandlers[state].Enter();
        }
        private void OnStateExit(T state)
        {
            _stateHandlers[state].Exit();
        }
    }
}
